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
    private const string GeminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-3-flash:generateContent";

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
            // 1. 프롬프트 생성 (사람이 읽을 수 있게)
            var prompt = BuildPrompt(meeting, memberAvailabilities);

            // 2. Gemini API 호출
            var response = await CallGeminiApi(prompt);

            // 3. 응답 파싱 (추천된 시간대 추출)
            var recommendedTimes = ParseGeminiResponse(response);

            return recommendedTimes.Take(topN).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gemini API Error: {ex.Message}");
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
        sb.AppendLine("반드시 다음 JSON 형식으로만 답변해줘. 추가 설명은 하지 말고 JSON만 출력해줘:");
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
                maxOutputTokens = 1000
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            $"{GeminiApiUrl}?key={_apiKey}",
            content
        );

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        return responseString;
    }

    private List<string> ParseGeminiResponse(string jsonResponse)
    {
        try
        {
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;

            // Gemini 응답 구조: candidates[0].content.parts[0].text
            var text = root
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "[]";

            // JSON 응답에서 time 필드 추출
            using JsonDocument parsedJson = JsonDocument.Parse(text);
            var recommendations = parsedJson.RootElement;

            var times = new List<string>();
            if (recommendations.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var item in recommendations.EnumerateArray())
                {
                    if (item.TryGetProperty("time", out var timeElement))
                    {
                        var timeStr = timeElement.GetString();
                        if (!string.IsNullOrEmpty(timeStr))
                        {
                            times.Add(timeStr);
                        }
                    }
                }
            }

            return times;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Parse Error: {ex.Message}");
            return new List<string>();
        }
    }
}