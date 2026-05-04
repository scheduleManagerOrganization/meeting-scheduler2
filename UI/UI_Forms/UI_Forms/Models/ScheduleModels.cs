using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UI_Forms.Models
{
    public class SlotDto
    {
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
