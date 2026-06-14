using System;
using System.Text.Json.Serialization;

namespace UI_Forms.Models
{
    // 1. 미팅 생성 (POST /api/meetings) 응답용 DTO
    public class MeetingCreateData
    {
        [JsonPropertyName("meeting_id")] // API 명세에 맞춰 snake_case 매핑
        public string MeetingId { get; set; }
    }

    // 2. 팀 미팅 목록 조회 (GET /api/meetings/team/{teamId}) 응답용 DTO
    public class MeetingDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("durationMinutes")] // API 명세 기준 camelCase
        public int DurationMinutes { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } // proposing, scheduled 등

        [JsonPropertyName("deadlineDate")]
        public string DeadlineDate { get; set; }
    }

    // 3. 추천 시간대 조회 (GET /api/slots/{meetingId}) 응답용 DTO
    public class MeetingSlotDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("meetingId")]
        public string MeetingId { get; set; }

        [JsonPropertyName("startTime")] // API 명세 기준 camelCase
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endTime")] // API 명세 기준 camelCase
        public DateTime EndTime { get; set; }

        [JsonPropertyName("aiScore")]
        public double AiScore { get; set; }
    }
}