using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MeetingScheduler.Services;
using MeetingScheduler.DTOs;
using MeetingScheduler.Models;

namespace MeetingScheduler.Controllers;

[ApiController]
[Route("api/availability")]
public class AvailabilityController : ControllerBase
{
    private readonly MongoDBService _mongoDB;

    public AvailabilityController(MongoDBService mongoDB)
    {
        _mongoDB = mongoDB;
    }

    [HttpPost]
    public async Task<IActionResult> SetAvailability([FromBody] SetAvailabilityRequest request)
    {
        try
        {
            // 업데이트할 필드만 지정
            var update = Builders<UserCalendar>.Update
                .Set(x => x.UserId, request.UserId)
                .Set(x => x.Date, request.Date)
                .Set(x => x.Slots, request.Slots.Select(s => new TimeSlot { Title = s.Title, Start = s.Start, End = s.End }).ToList()) //title 반영
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            // Upsert: 있으면 업데이트, 없으면 생성
            var result = await _mongoDB.UserCalendars.UpdateOneAsync(
                x => x.UserId == request.UserId && x.Date == request.Date,
                update,
                new UpdateOptions { IsUpsert = true }
            );

            return Ok(new { success = true, message = "일정이 저장되었습니다" });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }

    [HttpGet("{userId}/{date}")]
    public async Task<IActionResult> GetAvailability(string userId, string date)
    {
        try
        {
            var availability = await _mongoDB.UserCalendars
                .Find(x => x.UserId == userId && x.Date == date)
                .FirstOrDefaultAsync();

            return Ok(new { success = true, data = availability });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }

    [HttpGet("team/{teamId}/{date}")]
    public async Task<IActionResult> GetTeamAvailability(string teamId, string date)
    {
        try
        {
            Console.WriteLine($"=== GetTeamAvailability called ===");
            Console.WriteLine($"TeamId: '{teamId}', Date: '{date}'");

            if (string.IsNullOrEmpty(teamId))
                return BadRequest(new { success = false, error = "INVALID_TEAM_ID" });

            var team = await _mongoDB.Teams.Find(x => x.Id == teamId).FirstOrDefaultAsync();
            if (team == null)
                return NotFound(new { success = false, error = "TEAM_NOT_FOUND" });

            var memberIds = team.Members
                .Select(m => m.UserId)
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            Console.WriteLine($"Member IDs: [{string.Join(", ", memberIds)}]");

            if (memberIds.Count == 0)
                return Ok(new { success = true, data = new List<object>() });

            var availabilities = await _mongoDB.UserCalendars
                .Find(x => memberIds.Contains(x.UserId) && x.Date == date)
                .ToListAsync();

            Console.WriteLine($"Found {availabilities.Count} availabilities");

            var users = await _mongoDB.Users
                .Find(x => memberIds.Contains(x.Id))
                .ToListAsync();

            var userDict = users.ToDictionary(u => u.Id, u => u.Name);

            var result = availabilities.Select(a => new
            {
                user_id = a.UserId,
                user_name = userDict.GetValueOrDefault(a.UserId, "Unknown"),
                slots = a.Slots
            });

            return Ok(new { success = true, data = result });
        }
        catch (Exception e)
        {
            Console.WriteLine($"❌ Error in GetTeamAvailability: {e.Message}");
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }

    /// <summary>
    /// 📌 NEW: 여러 날짜의 가용시간을 한 번에 조회 (캘린더 최적화)
    /// </summary>
    [HttpGet("{userId}/range")]
    public async Task<IActionResult> GetAvailabilitiesRange(string userId, [FromQuery] string startDate, [FromQuery] int days = 42)
    {
        try
        {
            Console.WriteLine($"[GetAvailabilitiesRange] UserId: {userId}, StartDate: {startDate}, Days: {days}");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(startDate))
                return BadRequest(new { success = false, error = "MISSING_PARAMETERS", message = "userId와 startDate는 필수입니다" });

            if (!DateTime.TryParse(startDate, out var start))
                return BadRequest(new { success = false, error = "INVALID_DATE_FORMAT", message = "startDate는 yyyy-MM-dd 형식이어야 합니다" });

            if (days <= 0 || days > 365)
                return BadRequest(new { success = false, error = "INVALID_DAYS", message = "days는 1~365 사이여야 합니다" });

            // 날짜 범위 생성
            var dateRange = Enumerable.Range(0, days)
                .Select(d => start.AddDays(d).ToString("yyyy-MM-dd"))
                .ToList();

            Console.WriteLine($"[GetAvailabilitiesRange] 조회 범위: {dateRange.First()} ~ {dateRange.Last()} ({days}일)");

            // 범위 내 모든 일정 조회
            var availabilities = await _mongoDB.UserCalendars
                .Find(x => x.UserId == userId && dateRange.Contains(x.Date))
                .ToListAsync();

            Console.WriteLine($"[GetAvailabilitiesRange] 조회 결과: {availabilities.Count}개 항목");

            // 날짜별로 그룹화해서 반환
            var groupedByDate = availabilities
                .GroupBy(a => a.Date)
                .Select(g => new
                {
                    date = g.Key,
                    slots = g.SelectMany(x => x.Slots ?? new List<TimeSlot>()).ToList()
                })
                .OrderBy(x => x.date)
                .ToList();

            return Ok(new
            {
                success = true,
                data = new
                {
                    user_id = userId,
                    start_date = startDate,
                    days = days,
                    schedules = groupedByDate
                }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[GetAvailabilitiesRange] ❌ Error: {e.Message}");
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }


    /// <summary>
    /// 📌 NEW: 특정 팀원들의 여러 날짜 가용시간을 한 번에 조회 (팀 캘린더 최적화)
    /// </summary>
    [HttpGet("team/{teamId}/range")]
    public async Task<IActionResult> GetTeamAvailabilitiesRange(string teamId, [FromQuery] string startDate, [FromQuery] int days = 42)
    {
        try
        {
            Console.WriteLine($"[GetTeamAvailabilitiesRange] TeamId: {teamId}, StartDate: {startDate}, Days: {days}");

            if (string.IsNullOrEmpty(teamId) || string.IsNullOrEmpty(startDate))
                return BadRequest(new { success = false, error = "MISSING_PARAMETERS", message = "teamId와 startDate는 필수입니다" });

            if (!DateTime.TryParse(startDate, out var start))
                return BadRequest(new { success = false, error = "INVALID_DATE_FORMAT", message = "startDate는 yyyy-MM-dd 형식이어야 합니다" });

            // 1. 팀 정보 및 팀원 ID 목록 조회
            var team = await _mongoDB.Teams.Find(x => x.Id == teamId).FirstOrDefaultAsync();
            if (team == null)
                return NotFound(new { success = false, error = "TEAM_NOT_FOUND" });

            var memberIds = team.Members
                .Select(m => m.UserId)
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            if (memberIds.Count == 0)
                return Ok(new { success = true, data = new List<object>() });

            // 2. 날짜 범위 생성
            var dateRange = Enumerable.Range(0, days)
                .Select(d => start.AddDays(d).ToString("yyyy-MM-dd"))
                .ToList();

            // 3. 범위 내 '모든 팀원'의 일정 조회
            var availabilities = await _mongoDB.UserCalendars
                .Find(x => memberIds.Contains(x.UserId) && dateRange.Contains(x.Date))
                .ToListAsync();

            // 4. 유저 이름 매핑용 딕셔너리 빌드
            var users = await _mongoDB.Users
                .Find(x => memberIds.Contains(x.Id))
                .ToListAsync();
            var userDict = users.ToDictionary(u => u.Id, u => u.Name);

            // 5. 날짜별로 그룹화 및 유저 정보 조인
            var result = availabilities.Select(a => new
            {
                date = a.Date,
                user_id = a.UserId,
                user_name = userDict.GetValueOrDefault(a.UserId, "Unknown"),
                slots = a.Slots ?? new List<TimeSlot>()
            })
            .OrderBy(x => x.date)
            .ToList();

            return Ok(new
            {
                success = true,
                data = new
                {
                    team_id = teamId,
                    start_date = startDate,
                    days = days,
                    schedules = result
                }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[GetTeamAvailabilitiesRange] ❌ Error: {e.Message}");
            return StatusCode(500, new { success = false, error = e.Message });
        }
    }
}
