using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;

public static class FunctionResultExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static ApiResult GetCustomerResult(this FunctionResult functionResult)
    {
        if (functionResult is null) throw new ArgumentNullException(nameof(functionResult));

        var raw = ExtractRawJsonOrText(functionResult);

        if (string.IsNullOrWhiteSpace(raw))
            throw new InvalidOperationException("FunctionResult value is empty.");

        // raw อาจเป็น wrapper หรือเป็น ApiResult ตรง ๆ
        if (TryDeserialize<ApiWrapper>(raw, out var wrapper) && wrapper is not null && wrapper.content is not null)
        {
            if (wrapper.isError)
                throw new InvalidOperationException("API returned error flag (isError = true).");

            if (wrapper.content.Count == 0)
                throw new InvalidOperationException("API content is empty.");

            var firstContent = wrapper.content
                .FirstOrDefault(c => string.Equals(c.type, "text", StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException("No valid text content found in API response.");

            if (string.IsNullOrWhiteSpace(firstContent.text))
                throw new InvalidOperationException("No valid text content found in API response.");

            var innerJson = NormalizePossiblyEscapedJson(firstContent.text);

            var result = JsonSerializer.Deserialize<ApiResult>(innerJson, JsonOptions)
                         ?? throw new InvalidOperationException("Inner API result is null.");

            ValidateResult(result);
            return result;
        }
        else
        {
            // ไม่ใช่ wrapper ก็ถือว่าเป็น ApiResult ตรง ๆ
            var innerJson = NormalizePossiblyEscapedJson(raw);

            var result = JsonSerializer.Deserialize<ApiResult>(innerJson, JsonOptions)
                         ?? throw new InvalidOperationException("Inner API result is null.");

            ValidateResult(result);
            return result;
        }
    }

    private static string ExtractRawJsonOrText(FunctionResult functionResult)
    {
        // 1) BEST: Value เป็น JsonElement (ตรงกับ error ที่เจอ)
        try
        {
            var je = functionResult.GetValue<JsonElement>();
            if (je.ValueKind != JsonValueKind.Undefined && je.ValueKind != JsonValueKind.Null)
            {
                return je.GetRawText(); // ดิบ ๆ ไม่ escape ซ้อน
            }
        }
        catch { /* ignore */ }

        // 2) Value เป็น string
        try
        {
            var s = functionResult.GetValue<string>();
            if (!string.IsNullOrWhiteSpace(s)) return s!;
        }
        catch { /* ignore */ }

        // 3) Value เป็น ChatMessage
        try
        {
            var msg = functionResult.GetValue<ChatMessage>();
            if (msg is not null)
            {
                var text = msg.Contents.OfType<Microsoft.SemanticKernel.TextContent>().FirstOrDefault()?.Text;
                if (!string.IsNullOrWhiteSpace(text)) return text!;
                var fallback = msg.ToString();
                if (!string.IsNullOrWhiteSpace(fallback)) return fallback;
            }
        }
        catch { /* ignore */ }

        // 4) Value เป็น ChatResponse
        try
        {
            var resp = functionResult.GetValue<ChatResponse>();
            if (resp is not null && resp.Messages.Count > 0)
            {
                var last = resp.Messages.Last();
                var text = last.Contents.OfType<Microsoft.SemanticKernel.TextContent>().FirstOrDefault()?.Text;
                if (!string.IsNullOrWhiteSpace(text)) return text!;
                var fallback = last.ToString();
                if (!string.IsNullOrWhiteSpace(fallback)) return fallback;
            }
        }
        catch { /* ignore */ }

        // 5) LAST resort
        return functionResult.ToString();
    }

    private static bool TryDeserialize<T>(string json, out T? value)
    {
        try
        {
            value = JsonSerializer.Deserialize<T>(json, JsonOptions);
            return value is not null;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    private static string NormalizePossiblyEscapedJson(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return s;

        // ถ้าเป็น JSON string ที่ถูก escape (\u0022, \") ให้แกะ
        var u1 = SafeUnescape(s);
        if (LooksLikeJson(u1)) return u1;

        // บางที escape ซ้อน 2 ชั้น
        var u2 = SafeUnescape(u1);
        if (LooksLikeJson(u2)) return u2;

        return s;
    }

    private static string SafeUnescape(string s)
    {
        try { return Regex.Unescape(s); }
        catch { return s; }
    }

    private static bool LooksLikeJson(string s)
    {
        s = s.Trim();
        return (s.StartsWith("{") && s.EndsWith("}")) || (s.StartsWith("[") && s.EndsWith("]"));
    }

    private static void ValidateResult(ApiResult result)
    {
        if (result.returnValue != 0)
            throw new InvalidOperationException($"API returnValue is not success (returnValue = {result.returnValue}).");

        if (result.rows is null || result.rows.Count == 0)
            throw new InvalidOperationException("API rows is empty.");
    }
}

public class ApiWrapper
{
    public List<ApiContent>? content { get; set; }
    public bool isError { get; set; }
}

public class ApiContent
{
    public string? type { get; set; }
    public string? text { get; set; }
}

public class ApiResult
{
    public List<dynamic>? rows { get; set; }
    public int returnValue { get; set; }
}
