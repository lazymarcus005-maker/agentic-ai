using System.Text.Json;
using AgenticAI.Evaluation;
using AgenticAI.Models;
using AgenticAI.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AgenticAI.Controllers;

[ApiController]
[Route("[controller]")]
public class EvaluationController(EvalRunner evaluator, IWebHostEnvironment _env) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CaseReport2>> Get(CancellationToken ct)
    {
        //var results = await evaluator.RunAllAsync(ct);
        var tasks = Enumerable.Range(0, 1)
            .Select(_ =>
            {
                var filePath = Path.Combine(_env.ContentRootPath, "Evaluation", "question.json");
                if (!System.IO.File.Exists(filePath))
                    throw new Exception($"File not found: {filePath}");

                var json = System.IO.File.ReadAllTextAsync(filePath, ct).Result;
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var tests = JsonSerializer.Deserialize<TestCase[]>(json, options)
                            ?? throw new Exception("Failed to parse question.json");
                var id = Guid.NewGuid().ToString()[..8];
                return evaluator.RunAllAsync(ct, tests, id);
            });


        var results = await Task.WhenAll(tasks);

        var merged = results.SelectMany(x => x).ToList(); // รวมทุก result

        return Ok(merged);
    }
}
