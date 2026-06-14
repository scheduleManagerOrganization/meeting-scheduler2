using System.Collections.Generic;
using System.Text.Json.Serialization;
using UI_Forms.Models;

namespace UI_Forms.Models
{
    public class SlotDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }  // ← 새로 추가

        [JsonPropertyName("start")]
        public string Start { get; set; }

        [JsonPropertyName("end")]
        public string End { get; set; }
    }

    public class AvailabilityData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("slots")]
        public List<SlotDto> Slots { get; set; }
    }

    public class TeamAvailabilityData
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        [JsonPropertyName("slots")]
        public List<SlotDto> Slots { get; set; }
    }
}
//














// 새로 추가한 부분
public class RangeScheduleData
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; }

    [JsonPropertyName("days")]
    public int Days { get; set; }

    [JsonPropertyName("schedules")]
    public List<ScheduleItem> Schedules { get; set; }
}

public class ScheduleItem
{
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("slots")]
    public List<SlotDto> Slots { get; set; }  // ← SlotDto 재사용 (Title 포함)
}

// 📌 새로 추가하는 팀 캘린더 범위 조회용 DTO 데이터 모델
public class TeamRangeScheduleData
{
    [JsonPropertyName("team_id")]
    public string TeamId { get; set; }

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; }

    [JsonPropertyName("days")]
    public int Days { get; set; }

    [JsonPropertyName("schedules")]
    public List<TeamScheduleItem> Schedules { get; set; } // 팀 전용 아이템 사용
}

public class TeamScheduleItem
{
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    [JsonPropertyName("slots")]
    public List<SlotDto> Slots { get; set; } // 이미 작성하신 SlotDto 재사용
}