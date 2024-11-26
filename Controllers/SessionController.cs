using System.Runtime.Intrinsics.X86;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService ss)
        {
            sessionService = ss;
        }

        [HttpGet]
        public async Task<ActionResult<List<Session>>> GetSessions()
        {
            var sessions = await sessionService.GetAllSessionsAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> GetSessionById(int id)
        {
            var session = await sessionService.GetSessionByIdAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        [HttpPost]
        public async Task<ActionResult<Session>> CreateSession([FromBody] Session session)
        {
            var newSession = await sessionService.CreateSessionAsync(session);
            return CreatedAtAction(nameof(GetSessionById), new { id = newSession.SessionID }, newSession);

        }
        [HttpGet("trainer/{trainerId}")]
        public async Task<ActionResult<List<Session>>> GetSessionsByTrainerId(int trainerId)
        {
            var sessions = await sessionService.GetSessionsByTrainerIdAsync(trainerId);
            return Ok(sessions);
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<List<Session>>> GetSessionsByMemberId(int memberId)
        {
            var sessions = await sessionService.GetSessionsByMemberIdAsync(memberId);
            return Ok(sessions);
        }

        [HttpPost("search")]
        public async Task<ActionResult<List<Session>>> SessionSearch([FromBody] SessionSearch request)
        {
            var sessions = await sessionService.SessionSearchAsync(request);
            return Ok(sessions);
        }
        [HttpPut("register")]
        public async Task<ActionResult> RegisterMemberForSession(RegisterSessionRequest request)
        {
            var success = await sessionService.RegisterMemberForSessionAsync(request.SessionID, request.MemberID, request.Date);
            return Ok(success);
        }

        [HttpPut("rating")]
        public async Task<ActionResult> EditSessionRating([FromBody] EditSessionRating ratingRequest)
        {
            var success = await sessionService.EditSessionRatingAsync(ratingRequest.Rating, ratingRequest.SessionID);
            return Ok(success);
        }

        [HttpPut("edit-schedule/remove/{sessionID}")]
        public async Task<ActionResult> EditScheduleRemove(int sessionID)
        {
            var success = await sessionService.EditScheduleRemoveAsync(sessionID);
            return Ok(success);
        }
        [HttpPost("edit-schedule/add")]
        public async Task<ActionResult> EditScheduleAdd([FromBody] AddRecurring recurring)
        {
            var success = await sessionService.EditScheduleAddAsync(recurring);
            return Ok(success);
        }

        [HttpGet("schedule/{trainerID}")]
        public async Task<ActionResult> GetTrainerSchedule(int trainerID)
        {
            var schedule = await sessionService.GetTrainerScheduleAsync(trainerID);
            return Ok(schedule);
        }
    }
}
