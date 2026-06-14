using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MeetingScheduler.Models;

public class TimeSlot // 기존 가용시간 슬롯에서 개인 일정 슬롯으로 변경
{

    public string Title { get; set; } = string.Empty; // 일정 타이틀
    public string Start { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
}

public class UserCalendar
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.ObjectId)]  // 🔧 추가
    public string? UserId { get; set; }
    
    [BsonElement("date")]
    public string Date { get; set; } = string.Empty;
    
    [BsonElement("slots")]
    public List<TimeSlot> Slots { get; set; } = new();
    
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
