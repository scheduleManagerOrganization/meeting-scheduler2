using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MeetingScheduler.Services;
using MeetingScheduler.DTOs;
using MeetingScheduler.Models;

namespace MeetingScheduler.Controllers;

[ApiController]
[Route("api")]
public class SlotsController : ControllerBase
{
    private readonly MongoDBService _mongoDB;
    private readonly GeminiService _geminiService;
    
    public SlotsController(MongoDBService mongoDB, GeminiService geminiService)
    {
        _mongoDB = mongoDB;
        _geminiService = geminiService;
    }
    
    [HttpPost("suggest-slots")]
    public async Task<IActionResult> SuggestSlots([FromBody] SuggestSlotsRequest request)
    {
        try
        {
            if (request == null)
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                Console.WriteLine($"Raw request body: {body}");
                return BadRequest(new { success = false, error = "INVALID_REQUEST" });
            }
            
            var meetingId = request.MeetingId;
            
            if (string.IsNullOrWhiteSpace(meetingId) || meetingId.Length != 24 || 
                !System.Text.RegularExpressions.Regex.IsMatch(meetingId, "^[0-9a-fA-F]{24}$"))
            {
                return BadRequest(new { 
                    success = false, 
                    error = "INVALID_MEETING_ID",
                    message = $"Meeting ID must be a 24-character hex string." 
                });
            }
            
            var meeting = await _mongoDB.Meetings.Find(x => x.Id == meetingId).FirstOrDefaultAsync();
            if (meeting == null)
                return NotFound(new { success = false, error = "MEETING_NOT_FOUND" });
            
            var team = await _mongoDB.Teams.Find(x => x.Id == meeting.TeamId).FirstOrDefaultAsync();
            if (team == null)
                return NotFound(new { success = false, error = "TEAM_NOT_FOUND" });
            
            if (team.Members == null || !team.Members.Any())
                return BadRequest(new { success = false, error = "NO_TEAM_MEMBERS" });
            
            var memberIds = team.Members.Select(m => m.UserId).Where(id => !string.IsNullOrEmpty(id)).ToList();
            
            // 🔧 날짜 범위를 미리 리스트로 생성
            var startDate = DateTime.UtcNow;
            var dateRange = new List<string>();
            for (int d = 0; d < 7; d++)
            {
                dateRange.Add(startDate.AddDays(d).ToString("yyyy-MM-dd"));
            }
            
            // 🔧 string.Compare 대신 dateRange.Contains 사용 (MongoDB $in 쿼리로 변환 가능)
            var availabilities = await _mongoDB.UserCalendars
                .Find(x => memberIds.Contains(x.UserId) && dateRange.Contains(x.Date))
                .ToListAsync();
            
            var suggestedSlots = new List<ProposedSlot>();
            
            for (int day = 0; day < 7; day++)
            {
                var currentDate = startDate.AddDays(day).ToString("yyyy-MM-dd");
                
                foreach (var hour in new[] { 10, 14, 16 })
                {
                    var startTime = DateTime.Parse($"{currentDate} {hour}:00");
                    var endTime = startTime.AddMinutes(meeting.DurationMinutes);
                    
                    var availableCount = 0;
                    foreach (var av in availabilities)
                    {
                        if (av.Date == currentDate && av.Slots != null)
                        {
                            foreach (var slot in av.Slots)
                            {
                                if (slot != null && !string.IsNullOrEmpty(slot.Start))
                                {
                                    var startHour = int.Parse(slot.Start.Split(':')[0]);
                                    var endHour = int.Parse(slot.End.Split(':')[0]);
                                    if (startHour <= hour && hour < endHour)
                                    {
                                        availableCount++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    
                    if (availableCount > 0 && memberIds.Count > 0)
                    {
                        suggestedSlots.Add(new ProposedSlot
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            MeetingId = meeting.Id,
                            StartTime = startTime,
                            EndTime = endTime,
                            AiScore = Math.Round((double)availableCount / memberIds.Count * 100, 2),
                            Responses = new List<SlotResponse>(),
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
            }
            
            if (suggestedSlots.Any())
                await _mongoDB.ProposedSlots.InsertManyAsync(suggestedSlots);
            
            return Ok(new { success = true, data = new { suggested_count = suggestedSlots.Count } });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }
    
    [HttpGet("slots/{meetingId}")]
    public async Task<IActionResult> GetSlots(string meetingId)
    {
        try
        {
            if (string.IsNullOrEmpty(meetingId))
                return BadRequest(new { success = false, error = "INVALID_MEETING_ID" });
            
            var slots = await _mongoDB.ProposedSlots
                .Find(x => x.MeetingId == meetingId)
                .ToListAsync();
            
            return Ok(new { success = true, data = slots });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }
    
    [HttpPost("respond-slot")]
    public async Task<IActionResult> RespondToSlot([FromBody] RespondSlotRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.SlotId) || request.SlotId.Length != 24)
                return BadRequest(new { success = false, error = "INVALID_SLOT_ID" });
            
            var slot = await _mongoDB.ProposedSlots.Find(x => x.Id == request.SlotId).FirstOrDefaultAsync();
            if (slot == null)
                return NotFound(new { success = false, error = "SLOT_NOT_FOUND" });
            
            var filter = Builders<ProposedSlot>.Filter.Eq(x => x.Id, request.SlotId);
            //-------기존 코드 에러발생으로 아래 코드로 수정 6.15
            //var update = Builders<ProposedSlot>.Update
            //    .PullFilter(x => x.Responses, r => r.UserId == request.UserId)
            //    .Push(x => x.Responses, new SlotResponse
            //    {
            //        UserId = request.UserId,
            //        Response = request.Response,
            //        RespondedAt = DateTime.UtcNow
            //    });

            //await _mongoDB.ProposedSlots.UpdateOneAsync(filter, update);
            //---수정한 코드
            var pullUpdate = Builders<ProposedSlot>.Update
            .PullFilter(x => x.Responses, r => r.UserId == request.UserId);
            await _mongoDB.ProposedSlots.UpdateOneAsync(filter, pullUpdate);
            // 새로운 응답 객체를 배열에 추가 (Push)
            var pushUpdate = Builders<ProposedSlot>.Update
                .Push(x => x.Responses, new SlotResponse
                {
                    UserId = request.UserId,
                    Response = request.Response,
                    RespondedAt = DateTime.UtcNow
                });
            await _mongoDB.ProposedSlots.UpdateOneAsync(filter, pushUpdate);


            return Ok(new { success = true, message = "응답이 저장되었습니다" });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }

    [HttpPost("ai-recommend-slots")]
    [HttpPost("ai/recommend-slots")]
    [HttpPost("ai-recommend-slot")]
    public async Task<IActionResult> AiRecommendSlots([FromBody] SuggestSlotsRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrWhiteSpace(request.MeetingId))
                return BadRequest(new { success = false, error = "INVALID_REQUEST" });

            var meeting = await _mongoDB.Meetings
                .Find(x => x.Id == request.MeetingId)
                .FirstOrDefaultAsync();

            if (meeting == null)
                return NotFound(new { success = false, error = "MEETING_NOT_FOUND" });

            var team = await _mongoDB.Teams
                .Find(x => x.Id == meeting.TeamId)
                .FirstOrDefaultAsync();

            if (team?.Members == null || !team.Members.Any())
                return BadRequest(new { success = false, error = "NO_TEAM_MEMBERS" });

            var memberIds = team.Members.Select(m => m.UserId).ToList();

            // 조회 범위 결정: DeadlineDate를 고려
            var startDate = DateTime.UtcNow;
            int dayRange = 7; // 기본값

            if (!string.IsNullOrEmpty(meeting.DeadlineDate) && 
                DateTime.TryParse(meeting.DeadlineDate, out var deadline))
            {
                var daysUntilDeadline = (deadline.Date - startDate.Date).Days;
                if (daysUntilDeadline > 0)
                {
                    dayRange = daysUntilDeadline + 1;
                    Console.WriteLine($"[AI 추천] 데드라인 감지: {meeting.DeadlineDate} ({dayRange}일 범위)");
                }
                else if (daysUntilDeadline == 0)
                {
                    dayRange = 1;
                    Console.WriteLine($"[AI 추천] 오늘이 데드라인입니다.");
                }
                else
                {
                    Console.WriteLine($"[AI 추천] ❌ 데드라인 초과: {meeting.DeadlineDate}");
                    return BadRequest(new { 
                        success = false, 
                        error = "DEADLINE_PASSED",
                        message = "데드라인이 이미 지났습니다."
                    });
                }
            }

            var dateRange = Enumerable.Range(0, dayRange)
                .Select(d => startDate.AddDays(d).ToString("yyyy-MM-dd"))
                .ToList();

            var availabilities = await _mongoDB.UserCalendars
                .Find(x => memberIds.Contains(x.UserId) && dateRange.Contains(x.Date))
                .ToListAsync();

            // ✅ Gemini AI 추천 호출
            Console.WriteLine($"[AI 추천] 시작: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            var recommendations = await _geminiService.RecommendBestSlots(meeting, availabilities, topN: 5);
            Console.WriteLine($"[AI 추천] 완료: {recommendations.Count}개 추천 - {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");

            if (!recommendations.Any())
            {
                Console.WriteLine("[AI 추천] ⚠️ 추천 결과 없음");
                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        recommended_slots = new List<object>(),
                        message = "AI 추천 결과가 없습니다"
                    }
                });
            }

            // 추천된 시간대로 ProposedSlot 생성
            var slots = new List<ProposedSlot>();
            foreach (var recommendation in recommendations)
            {
                if (DateTime.TryParse(recommendation.Time, out var startTime))
                {
                    slots.Add(new ProposedSlot
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        MeetingId = meeting.Id,
                        StartTime = startTime,
                        EndTime = startTime.AddMinutes(meeting.DurationMinutes),
                        AiScore = 95.0, // ✅ AI 추천이라 높은 점수
                        AiReason = recommendation.Reason,
                        Responses = new List<SlotResponse>(),
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    Console.WriteLine($"[AI 추천] ⚠️ DateTime 파싱 실패: {recommendation.Time}");
                }
            }

            if (slots.Any())
                await _mongoDB.ProposedSlots.InsertManyAsync(slots);

            return Ok(new
            {
                success = true,
                data = new
                {
                    recommended_count = slots.Count,
                    slots = slots
                }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[AI 추천] ❌ 에러: {e.GetType().Name} - {e.Message}");
            Console.WriteLine($"[AI 추천] 스택 트레이스: {e.StackTrace}");
            return StatusCode(500, new { success = false, error = e.Message, details = e.StackTrace });
        }
    }   
}
