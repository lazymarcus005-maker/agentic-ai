using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using AgenticAI.Models;
using AgenticAI.Services;
using Azure;
using static Evaluator;

namespace AgenticAI.Evaluation;

public class EvalRunner
{
    private readonly IWebHostEnvironment _env;
    private readonly OrchestratorService _orchestratorService;

    public EvalRunner(IWebHostEnvironment env, OrchestratorService orchestratorService)
    {
        _env = env;
        _orchestratorService = orchestratorService;
    }

    public async Task<List<CaseReport2>> RunAllAsync(CancellationToken ct, TestCase[] tests = null, string idRun = null)
    {
        var sw = Stopwatch.StartNew();

        // 1️⃣ โหลด TestCase ทั้งหมด
        if (tests == null)
        {
            var filePath = Path.Combine(_env.ContentRootPath, "Evaluation", "question.json");
            if (!File.Exists(filePath))
                throw new Exception($"File not found: {filePath}");

            var json = await File.ReadAllTextAsync(filePath, ct);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            tests = JsonSerializer.Deserialize<TestCase[]>(json, options)
                        ?? throw new Exception("Failed to parse question.json");
        }

        var reports = new ConcurrentBag<CaseReport>();
        var evd = new ConcurrentBag<ChatResponse>();
        var newReports = new ConcurrentBag<EvaluationReport>();
        var newReports2 = new ConcurrentBag<CaseReport2>();

        var semaphore = new SemaphoreSlim(7); // จำกัดรันพร้อมกัน 5 งาน

        var tasks = tests.Select(async test =>
        {
            await semaphore.WaitAsync(ct);
            try
            {
                var runId = Guid.NewGuid().ToString();
                Console.WriteLine($"######{test.Id}");

                async Task SendStatus(string status)
                {
                    var data = $"data: {status}\n\n";
                    Console.WriteLine(data);
                    await Task.CompletedTask;
                }

                var resp = await _orchestratorService.ExecWithProgress(
                    new ChatRequest(test.Request.Prompt, test.Id, runId),
                    ct,
                    SendStatus,
                    test.Policy
                );

                evd.Add(resp);

                // ประเมินผลด้วย Evaluator
                var eval = Evaluator.Evaluate(resp, test);
                newReports.Add(eval);

                newReports2.Add(new CaseReport2
                {
                    TestCaseId = test.Id,
                    Tools = eval.Tools,
                    Replanning = eval.Replanning,
                    RunComplete = !string.IsNullOrEmpty(resp.Final)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in test {test.Id}: {ex.Message}");

                newReports2.Add(new CaseReport2
                {
                    TestCaseId = test.Id,
                    Tools = null,
                    Replanning = null,
                    RunComplete = false
                });
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);

        var suite = Evaluator.ComputeSuiteStability(newReports.ToList());

        var ts = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");

        StabilityExporter.SaveAsJson(
            newReports2.ToList(),
            $"1_gpt_report_{idRun}_{ts}.json"
        );

        StabilityExporter.SaveAsJson(
            evd.ToList(),
            $"1_gpt_chatresponse_{idRun}_{ts}.json"
        );

        // 4️⃣ สรุปรวม
        PrintSummary(reports.ToList());

        sw.Stop();
        Console.WriteLine($"######################################");
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");

        return newReports2.ToList();










        //// 2️⃣ เตรียมที่เก็บผล
        //var reports = new List<CaseReport>();
        //var evd = new List<ChatResponse>();
        //var newReports = new List<EvaluationReport>();
        //var newReports2 = new List<CaseReport2>();

        //foreach (var test in tests)
        //{
        //    var runId = Guid.NewGuid().ToString();
        //    Console.WriteLine($"######{test.Id}");
        //    try
        //    {
        //        async Task SendStatus(string status)
        //        {
        //            var data = $"data: {status}\n\n";
        //            Console.WriteLine(data);
        //        }
        //        var resp = await _orchestratorService.ExecWithProgress(
        //           new ChatRequest(test.Request.Prompt, test.Id, runId),
        //           ct, SendStatus, test.Policy
        //       );
        //        evd.Add(resp);
        //        // ประเมินผลด้วย Evaluator
        //        var eval = Evaluator.Evaluate(resp, test);
        //        newReports.Add(eval);
        //        newReports2.Add(new CaseReport2
        //        {
        //            TestCaseId = test.Id,
        //            Tools = eval.Tools,
        //            Replanning = eval.Replanning,
        //            RunComplete = !string.IsNullOrEmpty(resp.Final)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        newReports2.Add(new CaseReport2
        //        {
        //            TestCaseId = test.Id,
        //            Tools = null,
        //            Replanning = null,
        //            RunComplete = false
        //        });
        //    }
        //}


        //var suite = Evaluator.ComputeSuiteStability(newReports);

        //var ts = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");

        //StabilityExporter.SaveAsJson(
        //    newReports2,
        //    $"gpt_report_{idRun}_{ts}.json"
        //);
        //StabilityExporter.SaveAsJson(
        //    evd,
        //    $"gpt_chatresponse_{idRun}_{ts}.json"
        //);
        //// 4️⃣ สรุปรวม
        //PrintSummary(reports);

        //sw.Stop();
        //Console.WriteLine($"######################################");
        //Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
        //return newReports2;
    }

    private static void PrintSummary(List<CaseReport> reports)
    {
        Console.WriteLine("\n=========== Evaluation Summary ===========");
        foreach (var r in reports)
        {
            var status = r.Passed ? "✅ PASS" : "❌ FAIL";
            Console.WriteLine($"{r.TestCaseId} ({r.RunId}) → {status} | " +
                              $"WTR={r.Wtr:P0}, Validity={r.PlanValidity:P0}, Accuracy={r.Accuracy:P0}");
        }

        var total = reports.Count;
        var passed = reports.Count(r => r.Passed);
        Console.WriteLine($"------------------------------------------");
        Console.WriteLine($"TOTAL: {total}, PASSED: {passed}, FAILED: {total - passed}");
        Console.WriteLine($"==========================================\n");
    }
}

public class CaseReport
{
    public string TestCaseId { get; set; } = "";
    public string RunId { get; set; } = "";
    public bool Passed { get; set; }
    public double Wtr { get; set; }
    public double PlanValidity { get; set; }
    public double Accuracy { get; set; }
    public string Summary { get; set; } = "";

    public double OverallScore { get; set; }
    public bool OverallPassed { get; set; }
    public double FinalComplianceScore { get; set; }
    public double WtrScore { get; set; }
    public double ChannelScore { get; set; }

    public ChatResponse? Evdt { get; set; }
    public EvaluationReport? Detail { get; set; }
}

public class CaseReport2
{
    public string TestCaseId { get; set; } = "";
    public bool RunComplete { get; set; } = false;
    public ToolSection Tools { get; set; } = new();
    public ReplanningSection Replanning { get; set; } = new(); // NEW
}


public static class StabilityExporter
{
    public static void SaveAsJson(
        SuiteStabilitySummary stability,
        string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(stability, options);
        File.WriteAllText(filePath, json);
    }
    public static void SaveAsJson(
        List<CaseReport2> stability,
        string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };


        var json = JsonSerializer.Serialize(stability, options);
        File.WriteAllText(filePath, json);
    }
    public static void SaveAsJson(
        List<EvaluationReport> stability,
        string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var json = JsonSerializer.Serialize(stability, options);
        File.WriteAllText(filePath, json);
    }
    public static void SaveAsJson(
        List<ChatResponse> stability,
        string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(stability, options);
        File.WriteAllText(filePath, json);
    }
}

