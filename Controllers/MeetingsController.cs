using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MeetingScheduler.Services;
using MeetingScheduler.DTOs;
using MeetingScheduler.Models;

namespace MeetingScheduler.Controllers;


[ApiController]
[Route("api/meetings")]
public class MeetingsController : ControllerBase
{
    private readonly MongoDBService _mongoDB;
    
    public MeetingsController(MongoDBService mongoDB)
    {
        _mongoDB = mongoDB;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingRequest request)
    {
        try
        {
            var meeting = new Meeting
            {
                TeamId = request.TeamId,
                Title = request.Title,
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                CreatorId = request.CreatorId,
                Status = "proposing",
                DeadlineDate = request.DeadlineDate,
                CreatedAt = DateTime.UtcNow
            };
            
            await _mongoDB.Meetings.InsertOneAsync(meeting);
            
            return Ok(new
            {
                success = true,
                data = new { meeting_id = meeting.Id }
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }
    
    [HttpGet("{meetingId}")]
    public async Task<IActionResult> GetMeeting(string meetingId)
    {
        try
        {
            var meeting = await _mongoDB.Meetings.Find(x => x.Id == meetingId).FirstOrDefaultAsync();
            if (meeting == null)
                return NotFound(new { success = false, error = "MEETING_NOT_FOUND" });
            
            return Ok(new { success = true, data = meeting });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }
    
    [HttpGet("team/{teamId}")]
    public async Task<IActionResult> GetTeamMeetings(string teamId)
    {
        try
        {
            var meetings = await _mongoDB.Meetings
                .Find(x => x.TeamId == teamId)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
            
            return Ok(new { success = true, data = meetings });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }

    [HttpPost("{meetingId}/finalize")]
    public async Task<IActionResult> FinalizeMeeting(string meetingId, [FromBody] FinalizeMeetingRequest request)
    {
        try
        {
            // 1. 해당 미팅이 존재하는지 확인
            var meeting = await _mongoDB.Meetings.Find(x => x.Id == meetingId).FirstOrDefaultAsync();
            if (meeting == null)
                return NotFound(new { success = false, error = "MEETING_NOT_FOUND" });

            // 2. 확정하려는 슬롯이 존재하는지 확인 (선택 사항이지만 안전을 위해)
            var slot = await _mongoDB.ProposedSlots // 서비스에 ProposedSlots 컬렉션이 등록되어 있다고 가정
                .Find(x => x.Id == request.SlotId && x.MeetingId == meetingId)
                .FirstOrDefaultAsync();

            if (slot == null)
                return NotFound(new { success = false, error = "SLOT_NOT_FOUND" });

            // 3. 미팅 업데이트 (Status 변경 및 FinalizedSlotId 저장)
            var meetingUpdate = Builders<Meeting>.Update
                .Set(m => m.Status, "finalized") // 상태를 '확정됨'으로 변경
                .Set(m => m.FinalizedSlotId, request.SlotId);

            await _mongoDB.Meetings.UpdateOneAsync(x => x.Id == meetingId, meetingUpdate);

            // 4. 슬롯 업데이트 (IsFinalized = true)
            var slotUpdate = Builders<ProposedSlot>.Update
                .Set(s => s.IsFinalized, true);

            await _mongoDB.ProposedSlots
                .UpdateOneAsync(x => x.Id == request.SlotId, slotUpdate);

            return Ok(new { success = true, message = "Meeting finalized successfully." });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }


}
