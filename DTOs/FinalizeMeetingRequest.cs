using System.Text.Json.Serialization;

namespace MeetingScheduler.DTOs;

public class FinalizeMeetingRequest
{
    [JsonPropertyName("slot_id")]
    public string SlotId { get; set; } = string.Empty;
}