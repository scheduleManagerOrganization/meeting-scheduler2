using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MeetingScheduler.Models;

namespace MeetingScheduler.Services;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    // Gemini 3.0 Flash (또는 최신 Flash 모델)
    private const string GeminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-3-flash-preview:generateContent";

    public GeminiService(IConfiguration config, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = config["GEMINI_API_KEY"] 
            ?? throw new InvalidOperationException("GEMINI_API_KEY not configured");
    }

    /// <summary>
    /// Gemini AI를 사용해 최적의 회의 시간 추천
    /// </summary>
    public async Task<List<string>> RecommendBestSlots(
        Meeting meeting, 
        List<UserCalendar> memberAvailabilities,
        int topN = 3)
    {
        try
        {
            Console.WriteLine($"[GeminiService] 시작 - Meeting: {meeting.Title}");
            Console.WriteLine($"[GeminiService] 참여자 수: {memberAvailabilities.Count}");

            // 1. 프롬프트 생성 (사람이 읽을 수 있게)
            var prompt = BuildPrompt(meeting, memberAvailabilities);
            Console.WriteLine($"[GeminiService] 프롬프트 생성 완료 ({prompt.Length} chars)");

            // 2. Gemini API 호출
            Console.WriteLine($"[GeminiService] Gemini API 호출 시작...");
            var response = await CallGeminiApi(prompt);
            Console.WriteLine($"[GeminiService] Gemini API 응답 수신 ({response.Length} chars)");

            // 3. 응답 파싱 (추천된 시간대 추출)
            var recommendedTimes = ParseGeminiResponse(response);
            Console.WriteLine($"[GeminiService] 파싱 완료: {recommendedTimes.Count}개 시간대 추출");

            if (recommendedTimes.Any())
            {
                foreach (var time in recommendedTimes.Take(3))
                {
                    Console.WriteLine($"[GeminiService] - {time}");
                }
            }

            return recommendedTimes.Take(topN).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GeminiService] ❌ 에러: {ex.GetType().Name}");
            Console.WriteLine($"[GeminiService] 메시지: {ex.Message}");
            Console.WriteLine($"[GeminiService] 스택: {ex.StackTrace}");
            return new List<string>(); // 실패 시 빈 리스트
        }
    }

    private string BuildPrompt(Meeting meeting, List<UserCalendar> availabilities)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"회의명: {meeting.Title}");
        sb.AppendLine($"회의 설명: {meeting.Description}");
        sb.AppendLine($"소요 시간: {meeting.DurationMinutes}분");
        sb.AppendLine();
        sb.AppendLine("참여자들의 가용 시간:");


        foreach (var av in availabilities)
        {
            sb.AppendLine($"  - 사용자: {av.UserId}");
            if (av.Slots != null)
            {
                foreach (var slot in av.Slots)
                {
                    sb.AppendLine($"    {av.Date} {slot.Start}~{slot.End}");
                }
            }
        }

        sb.AppendLine();
        sb.AppendLine("위 정보를 기반으로 모든 참여자가 참석 가능한 '가장 최적의 회의 시간' TOP 3를 추천해줘.");
        sb.AppendLine("반드시 다음 JSON 형식으로만 답변해줘. 이유(reason)는 한 문장으로 짧게 작성해:");
        sb.AppendLine("[");
        sb.AppendLine("  { \"time\": \"YYYY-MM-DD HH:MM\", \"reason\": \"이유\" },");
        sb.AppendLine("  { \"time\": \"YYYY-MM-DD HH:MM\", \"reason\": \"이유\" },");
        sb.AppendLine("  { \"time\": \"YYYY-MM-DD HH:MM\", \"reason\": \"이유\" }");
        sb.AppendLine("]");

        return sb.ToString();
    }

    private async Task<string> CallGeminiApi(string prompt)
    {
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            generationConfig = new
            {
                responseMimeType = "application/json", // JSON 응답 강제
                temperature = 0.2, // 낮은 온도로 안정적인 JSON 출력
                topK = 40,
                topP = 0.95,
                maxOutputTokens = 2000
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        try
        {
            Console.WriteLine($"[Gemini API] 요청 시작: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");

            var response = await _httpClient.PostAsync(
                $"{GeminiApiUrl}?key={_apiKey}",
                content
            );

            Console.WriteLine($"[Gemini API] 응답 받음: {response.StatusCode} ({DateTime.UtcNow:yyyy-MM-dd HH:mm:ss})");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[Gemini API] 에러 응답: {errorContent}");
                throw new Exception($"Gemini API 호출 실패: {response.StatusCode} - {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[Gemini API] 응답 파싱 완료: {responseString.Length} bytes");

            return responseString;
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"[Gemini API] 타임아웃 에러: {ex.Message}");
            throw new Exception("Gemini API 요청 타임아웃 (120초 초과)", ex);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[Gemini API] HTTP 에러: {ex.Message}");
            throw;
        }
    }

    private List<string> ParseGeminiResponse(string jsonResponse)
    {
        try
        {
            Console.WriteLine($"[GeminiService.Parse] 시작 (응답 크기: {jsonResponse.Length} bytes)");

            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;

            Console.WriteLine($"[GeminiService.Parse] Root element kind: {root.ValueKind}");

            // Gemini 응답 구조: candidates[0].content.parts[0].text
            if (!root.TryGetProperty("candidates", out var candidates))
            {
                Console.WriteLine("[GeminiService.Parse] ❌ 'candidates' 필드 없음");
                return new List<string>();
            }

            Console.WriteLine($"[GeminiService.Parse] candidates 배열 길이: {candidates.GetArrayLength()}");

            if (candidates.GetArrayLength() == 0)
            {
                Console.WriteLine("[GeminiService.Parse] ❌ candidates 배열이 비어있음");
                return new List<string>();
            }

            var candidate = candidates[0];
            var content = candidate.GetProperty("content");
            var parts = content.GetProperty("parts");

            // parts 배열에 text가 여러 조각으로 들어오는 경우가 있어 모두 합침
            var textBuilder = new StringBuilder();
            foreach (var part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("text", out var textElement))
                {
                    textBuilder.Append(textElement.GetString());
                }
            }
            var text = textBuilder.Length > 0 ? textBuilder.ToString() : "[]";

            Console.WriteLine($"[GeminiService.Parse] AI 응답 텍스트: {text.Substring(0, Math.Min(200, text.Length))}...");

            // Gemini가 간헐적으로 ```json ... ``` 형태로 반환하거나 끝이 잘린 경우 정제
            var cleaned = ExtractJsonPayload(text);

            // JSON 응답에서 time 필드 추출
            using JsonDocument parsedJson = JsonDocument.Parse(cleaned);
            var recommendations = parsedJson.RootElement;

            var times = new List<string>();
            if (recommendations.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in recommendations.EnumerateArray())
                {
                    AddTimeFromItem(item, times);
                }
            }
            else if (recommendations.ValueKind == JsonValueKind.Object)
            {
                // 모델이 { "recommendations": [...] } 또는 { "slots": [...] }로 감싸서 반환하는 경우 대응
                if (recommendations.TryGetProperty("recommendations", out var wrappedArray) && wrappedArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in wrappedArray.EnumerateArray())
                        AddTimeFromItem(item, times);
                }
                else if (recommendations.TryGetProperty("slots", out var slotsArray) && slotsArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in slotsArray.EnumerateArray())
                        AddTimeFromItem(item, times);
                }
                else
                {
                    AddTimeFromItem(recommendations, times);
                }
            }

            Console.WriteLine($"[GeminiService.Parse] 추출된 시간: {times.Count}개");
            return times.Distinct().ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GeminiService.Parse] ❌ 파싱 에러: {ex.GetType().Name}");
            Console.WriteLine($"[GeminiService.Parse] 메시지: {ex.Message}");
            Console.WriteLine($"[GeminiService.Parse] 응답 프리뷰: {jsonResponse.Substring(0, Math.Min(500, jsonResponse.Length))}");
            return new List<string>();
        }
    }

    private static void AddTimeFromItem(JsonElement item, List<string> times)
    {
        string? raw = null;

        if (item.ValueKind == JsonValueKind.String)
        {
            raw = item.GetString();
        }
        else if (item.ValueKind == JsonValueKind.Object)
        {
            if (item.TryGetProperty("time", out var timeElement)) raw = timeElement.GetString();
            else if (item.TryGetProperty("startTime", out var startTime)) raw = startTime.GetString();
            else if (item.TryGetProperty("datetime", out var dateTime)) raw = dateTime.GetString();
            else if (item.TryGetProperty("slot", out var slot)) raw = slot.GetString();
        }

        if (string.IsNullOrWhiteSpace(raw))
            return;

        // downstream DateTime.TryParse 성공률을 높이기 위한 표준화
        if (DateTime.TryParse(raw, out var parsed))
        {
            times.Add(parsed.ToString("yyyy-MM-dd HH:mm"));
        }
        else
        {
            // 파싱 안 되는 문자열도 로그 확인을 위해 우선 전달
            times.Add(raw.Trim());
        }
    }

    private static string ExtractJsonPayload(string text)
    {
        var trimmed = text.Trim();

        if (trimmed.StartsWith("```"))
        {
            var firstNewLine = trimmed.IndexOf('\n');
            var lastFence = trimmed.LastIndexOf("```", StringComparison.Ordinal);
            if (firstNewLine >= 0 && lastFence > firstNewLine)
            {
                trimmed = trimmed.Substring(firstNewLine + 1, lastFence - firstNewLine - 1).Trim();
            }
        }

        if (TryExtractBalancedJson(trimmed, '[', ']', out var arrayPayload))
            return arrayPayload;

        if (TryExtractBalancedJson(trimmed, '{', '}', out var objectPayload))
            return objectPayload;

        return trimmed;
    }

    private static bool TryExtractBalancedJson(string source, char openChar, char closeChar, out string payload)
    {
        payload = string.Empty;

        var start = source.IndexOf(openChar);
        if (start < 0)
            return false;

        var depth = 0;
        var inString = false;
        var escaped = false;

        for (var i = start; i < source.Length; i++)
        {
            var c = source[i];

            if (escaped)
            {
                escaped = false;
                continue;
            }

            if (c == '\\')
            {
                escaped = true;
                continue;
            }

            if (c == '"')
            {
                inString = !inString;
                continue;
            }

            if (inString)
                continue;

            if (c == openChar)
                depth++;
            else if (c == closeChar)
                depth--;

            if (depth == 0)
            {
                payload = source.Substring(start, i - start + 1).Trim();
                return true;
            }
        }

        return false;
    }
}
