using System.Text.Json.Serialization;

namespace UI_Forms.Models
{
    public class TeamDto
    {
        [JsonPropertyName("team_id")]
        public string TeamId { get; set; }

        [JsonPropertyName("team_name")]
        public string TeamName { get; set; }

        [JsonPropertyName("join_code")]
        public string JoinCode { get; set; }

        [JsonPropertyName("member_count")]
        public int MemberCount { get; set; }
    }
}