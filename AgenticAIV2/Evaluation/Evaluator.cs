using System.Text;
using AgenticAI.Evaluation;
using AgenticAI.Models;
using Grpc.Core;

public static class Evaluator
{
    // Entry point
    public static EvaluationReport Evaluate(ChatResponse resp, TestCase tc)
    {
        var report = new EvaluationReport
        {
            TestCaseId = tc.Id,
            RunId = resp.RunId
        };

        // 1) WTR
        try
        {
            report.Tools = ComputeWtr2(resp, tc.Policy.Tools);
        }
        catch (Exception)
        {

        }
        // 1) WTR
        try
        {
            // 1.5) Re-planning (NEW)
            report.Replanning = ComputeReplanning(resp);
        }
        catch (Exception)
        {

        }


        //// 2) Plan Validity (L2)
        //report.PlanValidity = ComputePlanValidityL2(resp, requireAllSucceeded: tc.Metrics.PlanValidity.RequireAllSucceeded);

        //// 3) Accuracy (containment on final)
        //report.Accuracy = ComputeAccuracyContainment(resp.Final ?? string.Empty, tc.Metrics.Accuracy);

        //// 4) Channels (email simple check: allowed/required & present)
        //report.Channels = EvaluateChannels(resp, tc.Channels);

        //// 5) Overall score (NEW: ใช้คะแนนทศนิยมรวมทุก metric)
        //ComputeOverallScore(report, tc);

        return report;
    }

    #region WTR


    private static ToolSection ComputeWtr(ChatResponse resp, ToolsPolicyBlock policy)
    {
        var result = new ToolSection();
        var plan = resp.Plan ?? new Plan();

        var toolSteps = (plan.Steps ?? new())
            .Where(s => string.Equals(s.Type, "tool", StringComparison.OrdinalIgnoreCase))
            .ToList();

        result.InvokedCount = toolSteps.Count;

        HashSet<string> used = new();

        // NEW: นับ "จำนวน step ที่ fail" ตามนิยามใหม่ (ผิด tool OR exec failed)
        var wtrFailSteps = 0;

        foreach (var s in toolSteps)
        {
            var fullname = $"{s.Plugin}.{s.Tool}";
            used.Add(fullname);

            // 1) Wrong tool selection (เลือกผิด)
            var isWrongTool = false;

            if (policy.Forbidden.Contains(fullname))
            {
                isWrongTool = true;
                result.Violations.Add(new Violation { StepId = s.Id, ToolFullName = fullname, Reason = "tool_forbidden" });
            }
            else if (!policy.Allowed.Contains(fullname))
            {
                isWrongTool = true;
                result.Violations.Add(new Violation { StepId = s.Id, ToolFullName = fullname, Reason = "tool_not_allowed" });
            }

            if (isWrongTool)
            {
                result.WrongInvocations++;
            }

            // 2) Tool Usage Failure (เลือกถูกแต่พารามิเตอร์ผิดจน Error / เรียกใช้ไม่สำเร็จ)
            var resultOfStep = resp.Journal?.Steps?.FirstOrDefault(_ => _.Id == s.Id);
            //var isExecFailed = string.Equals(resultOfStep?.Status, "failed", StringComparison.OrdinalIgnoreCase);

            //// NEW: นับแบบไม่ซ้ำ (1 step = 1 fail สูงสุด)
            //if (isWrongTool || isExecFailed)
            //{
            //    wtrFailSteps++;
            //}

            // NEW: นับแบบไม่ซ้ำ (1 step = 1 fail สูงสุด)
            if (isWrongTool)
            {
                wtrFailSteps++;
            }
        }

        result.UsedSet = used;
        result.ForbiddenUsed = used.Intersect(policy.Forbidden).Any();

        // NEW: WTR ตามสูตรใหม่
        // WTR = (เลือกผิด + เลือกถูกแต่พารามิเตอร์ผิดจน Error + เรียกใช้ไม่สำเร็จ) / จำนวนครั้งทั้งหมด
        result.WtrStep = result.InvokedCount == 0 ? 0 : (double)wtrFailSteps / result.InvokedCount;

        var reqHit = used.Intersect(policy.Required).Count();
        result.RequiredCoverage = policy.Required.Count == 0 ? 1.0 : (double)reqHit / policy.Required.Count;

        result.ReplanCount = Math.Max(0, (resp.PlanHistories?.Count ?? 0) - 1);

        // FinalCompliance: ไม่มี fail ตามนิยามใหม่ และไม่ใช้ forbidden
        result.FinalCompliance = (result.WtrStep == 0) && !result.ForbiddenUsed;

        return result;
    }
    private static ToolSection ComputeWtr2(ChatResponse resp, ToolsPolicyBlock policy)
    {
        var result = new ToolSection();

        var allInvokedCount = 0;
        var allWtrFailSteps = 0;
        HashSet<string> allUsedSet = new();

        foreach (var p in (resp.PlanHistories ?? new List<Plan>()))
        {
            var plan = p ?? new Plan();

            var toolSteps = (plan.Steps ?? new())
                .Where(s => string.Equals(s.Type, "tool", StringComparison.OrdinalIgnoreCase))
                .ToList();

            allInvokedCount += toolSteps.Count;

            foreach (var s in toolSteps)
            {
                var fullname = $"{s.Plugin}.{s.Tool}";
                allUsedSet.Add(fullname);

                // 1) wrong tool
                var isWrongTool = false;
                if (policy.Forbidden.Contains(fullname))
                {
                    isWrongTool = true;
                    result.Violations.Add(new Violation { StepId = s.Id, ToolFullName = fullname, Reason = "tool_forbidden" });
                }
                else if (!policy.Allowed.Contains(fullname))
                {
                    isWrongTool = true;
                    result.Violations.Add(new Violation { StepId = s.Id, ToolFullName = fullname, Reason = "tool_not_allowed" });
                }

                if (isWrongTool) result.WrongInvocations++;

                // 2) exec failed
                var resultOfStep = resp.Journal?.Steps?.FirstOrDefault(_ => _.Id == s.Id);
                var isExecFailed = string.Equals(resultOfStep?.Status, "failed", StringComparison.OrdinalIgnoreCase);

                // WTR fail per step (no double count)
                if (isWrongTool || isExecFailed)
                    allWtrFailSteps++;
            }
        }

        result.InvokedCount = allInvokedCount;
        result.UsedSet = allUsedSet;
        result.ForbiddenUsed = allUsedSet.Intersect(policy.Forbidden).Any();

        // WTR ใหม่
        result.WtrStep = allInvokedCount == 0 ? 0 : (double)allWtrFailSteps / allInvokedCount;

        // (optional) Wrong Tool Rate (ถ้าคุณต้องการค่าแยก)
        // var wrongToolRate = allInvokedCount == 0 ? 0 : (double)result.WrongInvocations / allInvokedCount;

        var reqHit = allUsedSet.Intersect(policy.Required).Count();
        result.RequiredCoverage = policy.Required.Count == 0 ? 1.0 : (double)reqHit / policy.Required.Count;

        result.ReplanCount = Math.Max(0, (resp.PlanHistories?.Count ?? 0) - 1);
        result.FinalCompliance = (result.WtrStep == 0) && !result.ForbiddenUsed;

        return result;
    }

    #endregion

    #region Plan Validity L2
    private static ValiditySection ComputePlanValidityL2(ChatResponse resp, bool requireAllSucceeded)
    {
        var plan = resp.Plan ?? new Plan();
        var steps = plan.Steps ?? new List<PlanStep>();
        var order = steps.Select(s => s.Id).ToList();

        var dependsOn = steps.ToDictionary(
            s => s.Id,
            s => (s.DependsOn ?? new List<string>()));

        var journalMap = (resp.Journal?.Steps ?? new List<JournalStep>())
            .GroupBy(j => j.Id)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.DurationMs).First()); // last by duration as tie-break

        bool Succeeded(string id) =>
            journalMap.TryGetValue(id, out var j) &&
            string.Equals(j.Status, "succeeded", StringComparison.OrdinalIgnoreCase);

        var indexById = order.Select((id, idx) => (id, idx)).ToDictionary(x => x.id, x => x.idx);

        int total = order.Count;
        int validCount = 0;
        var invalid = new List<string>();

        foreach (var sid in order)
        {
            bool ok = true;

            // self succeeded
            if (!Succeeded(sid)) ok = false;

            // deps: exist, come before, succeeded
            foreach (var dep in dependsOn[sid])
            {
                if (!indexById.ContainsKey(dep)) { ok = false; break; }
                if (indexById[dep] >= indexById[sid]) { ok = false; break; }
                if (!Succeeded(dep)) { ok = false; break; }
            }

            if (ok) validCount++; else invalid.Add(sid);
        }

        var score = total == 0 ? 1.0 : (double)validCount / total;
        var isValid = invalid.Count == 0;

        // ไม่บีบให้ fail ทั้งเคสจากตรงนี้แล้ว
        // requireAllSucceeded ยังเก็บไว้เป็น flag แต่นำไปใช้ใน overall score แทน
        return new ValiditySection
        {
            Score = score,
            IsValid = isValid,
            RequiredAllSucceeded = requireAllSucceeded,
            InvalidSteps = invalid
        };
    }
    #endregion

    #region Accuracy (Containment)
    private static AccuracySection ComputeAccuracyContainment(string finalText, AccuracyConfig cfg)
    {
        var text = finalText ?? string.Empty;
        if (cfg.NormalizeWhitespace) text = NormalizeWs(text);

        var comp = cfg.CaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        int total = cfg.Containment.MustContain?.Count ?? 0;
        if (total == 0) return new AccuracySection { Coverage = 1.0, Passed = true };

        int found = 0;
        var missing = new List<string>();
        foreach (var token in cfg.Containment.MustContain)
        {
            var t = cfg.NormalizeWhitespace ? NormalizeWs(token ?? "") : token ?? "";
            if (!string.IsNullOrWhiteSpace(t) && text.Contains(t, comp)) found++;
            else missing.Add(token);
        }

        var forbiddenHit = new List<string>();
        foreach (var f in cfg.Containment.MustNotContain ?? new List<string>())
        {
            var t = cfg.NormalizeWhitespace ? NormalizeWs(f ?? "") : f ?? "";
            if (!string.IsNullOrWhiteSpace(t) && text.Contains(t, comp)) forbiddenHit.Add(f);
        }

        double coverage = (double)found / total;
        bool passed = coverage >= cfg.Threshold && forbiddenHit.Count == 0;

        return new AccuracySection
        {
            Coverage = coverage,
            Passed = passed,
            Missing = missing,
            ForbiddenHit = forbiddenHit
        };
    }

    private static string NormalizeWs(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        var sb = new StringBuilder(s.Length);
        bool inWs = false;
        foreach (var ch in s)
        {
            if (char.IsWhiteSpace(ch))
            {
                if (!inWs) { sb.Append(' '); inWs = true; }
            }
            else { sb.Append(ch); inWs = false; }
        }
        return sb.ToString().Trim();
    }
    #endregion

    #region Channels
    private static ChannelSection EvaluateChannels(ChatResponse resp, ChannelsBlock ch)
    {
        bool emailPresent = resp.Email != null;
        return new ChannelSection
        {
            EmailAllowed = ch.Email.Allowed,
            EmailRequired = ch.Email.Required,
            EmailSatisfied = emailPresent
        };
    }
    #endregion

    //#endregion
    #region Overall Score (NEW)
    private static void ComputeOverallScore(EvaluationReport report, TestCase tc)
    {
        var tools = report.Tools;
        var plan = report.PlanValidity;
        var acc = report.Accuracy;
        var ch = report.Channels;

        // 1) WTR → 0..1
        double wtrScore = tools.InvokedCount == 0 ? 1.0 : 1.0 - tools.WtrStep;
        if (wtrScore < 0) wtrScore = 0.0;

        // 2) Final-compliance แบบทศนิยม (ใช้สะท้อนคุณภาพการใช้ tools)
        double finalComplianceScore = 0.5 * wtrScore + 0.5 * tools.RequiredCoverage;

        // 3) semantic coverage จาก containment (ความครอบคลุม/ถูกต้องตาม expected)
        double coverageScore = acc.Coverage;      // 0..1

        // 4) Plan validity
        double planScore = plan.Score;            // 0..1

        // 5) Channels (เน้นเรื่อง email ตาม config)
        double channelScore = 1.0;
        if (ch.EmailRequired)
        {
            channelScore = ch.EmailSatisfied ? 1.0 : 0.0;
        }
        else if (!ch.EmailAllowed && ch.EmailSatisfied)
        {
            // มีส่ง email ทั้งที่ not allowed → หักหน่อย
            channelScore = 0.5;
        }

        // 6) รวมเป็น "final accuracy" ที่เอา containment + plan + tools + channels มาคิดรวม
        double finalAccuracy =
            0.15 * coverageScore +
            0.3 * wtrScore +
            0.35 * planScore +
            0.15 * finalComplianceScore +
            0.05 * channelScore;

        var summaryTxt = $@"0.15 * {coverageScore} +
            0.3 * {wtrScore} +
            0.35 * {planScore} +
            0.15 * {finalComplianceScore} +
            0.05 * {channelScore};";
        // hard fail rule
        bool hardFail =
            tools.ForbiddenUsed ||
            (acc.ForbiddenHit != null && acc.ForbiddenHit.Count > 0);

        // ใช้ Threshold เดิมจาก accuracy config
        double threshold = tc.Metrics?.Accuracy?.Threshold ?? 0.75;
        bool passed = !hardFail && finalAccuracy >= threshold;

        // map กลับเข้า report
        report.OverallScore = finalAccuracy;
        report.OverallPassed = passed;
        report.FinalComplianceScore = finalComplianceScore;
        report.WtrScore = wtrScore;
        report.ChannelScore = channelScore;

        // ใช้ Accuracy เป็นตัวแทน "ความถูกต้องรวม" ตามที่ต้องการ
        report.Accuracy.Coverage = finalAccuracy;
        report.Accuracy.Passed = passed;
        report.OverallScoreTxt = summaryTxt;
    }
    #endregion

    private static ReplanningSection ComputeReplanning(ChatResponse resp)
    {
        // NOTE: บางระบบเก็บ plan ปัจจุบันใน resp.Plan และ history ใน resp.PlanHistories
        // ถ้า PlanHistories ว่าง/null ให้ถือว่าไม่มี replan
        int histCount = resp.PlanHistories?.Count ?? 0;

        // ถ้าระบบคุณใส่ "plan ล่าสุด" ใน PlanHistories ด้วย (เช่น [plan1, plan2, plan3])
        // replanCount = histCount - 1
        // แต่ถ้า PlanHistories เก็บแค่ "previous plans" ไม่รวมปัจจุบัน อันนี้ต้องปรับเอง
        int replanCount = Math.Max(0, histCount - 1);

        return new ReplanningSection
        {
            PlanHistoryCount = histCount,
            ReplanCount = replanCount,
            Replanned = replanCount > 0
        };
    }
    public static SuiteStabilitySummary ComputeSuiteStability(IEnumerable<EvaluationReport> reports)
    {
        var list = reports?.ToList() ?? new List<EvaluationReport>();
        int totalRuns = list.Count;

        int runsWithReplan = 0;
        int totalReplanCount = 0;

        int totalInvoked = 0;
        int totalWrong = 0;

        double sumWtrPerRun = 0.0;

        foreach (var r in list)
        {
            // Replan
            var rp = r.Replanning ?? new ReplanningSection();
            if (rp.Replanned) runsWithReplan++;
            totalReplanCount += rp.ReplanCount;

            // WTR (micro)
            var t = r.Tools ?? new ToolSection();
            totalInvoked += t.InvokedCount;
            totalWrong += t.WrongInvocations;

            // WTR (macro)
            sumWtrPerRun += t.InvokedCount == 0 ? 0.0 : t.WtrStep;
        }

        double replanRate = totalRuns == 0 ? 0.0 : (double)runsWithReplan / totalRuns;
        double wtrRate = totalInvoked == 0 ? 0.0 : (double)totalWrong / totalInvoked;
        double avgWtrPerRun = totalRuns == 0 ? 0.0 : sumWtrPerRun / totalRuns;

        return new SuiteStabilitySummary
        {
            TotalRuns = totalRuns,
            RunsWithReplan = runsWithReplan,
            TotalReplanCount = totalReplanCount,
            ReplanRate = replanRate,

            TotalToolInvocations = totalInvoked,
            TotalWrongToolInvocations = totalWrong,
            WtrRate = wtrRate,

            AvgWtrPerRun = avgWtrPerRun
        };
    }

    public class SuiteStabilitySummary
    {
        public int TotalRuns { get; set; }

        // Replanning
        public int RunsWithReplan { get; set; }
        public int TotalReplanCount { get; set; }
        public double ReplanRate { get; set; } // runsWithReplan / totalRuns

        // WTR
        public int TotalToolInvocations { get; set; }
        public int TotalWrongToolInvocations { get; set; }
        public double WtrRate { get; set; } // totalWrong / totalInvoked (micro-average)

        // optional: macro average WTR per run
        public double AvgWtrPerRun { get; set; }
    }

    public class ReplanningSection
    {
        public int PlanHistoryCount { get; set; }
        public int ReplanCount { get; set; }      // planHistories.Count - 1 (>=0)
        public bool Replanned { get; set; }       // ReplanCount > 0
    }
    //public class EvaluationReport
    //{
    //    public string TestCaseId { get; set; } = "";
    //    public string RunId { get; set; } = "";

    //    public ToolSection Tools { get; set; } = new();
    //    public ReplanningSection Replanning { get; set; } = new(); // NEW

    //    public ValiditySection PlanValidity { get; set; } = new();
    //    public AccuracySection Accuracy { get; set; } = new();
    //    public ChannelSection Channels { get; set; } = new();

    //    public double OverallScore { get; set; }
    //    public bool OverallPassed { get; set; }

    //    public double FinalComplianceScore { get; set; }
    //    public double WtrScore { get; set; }
    //    public double ChannelScore { get; set; }
    //    public string OverallScoreTxt { get; set; } = "";
    //}

}
