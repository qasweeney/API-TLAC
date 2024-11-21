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

        [HttpPost("search")]
        public async Task<ActionResult<List<Session>>> SessionSearch([FromBody] SessionSearch request)
        {
            var sessions = await sessionService.SessionSearchAsync(request);
            return Ok(sessions);
        }
        [HttpPut("register")]
        public async Task<ActionResult> RegisterMemberForSession(RegisterSessionRequest request)
        {
            // System.Console.WriteLine(request.Date);
            var success = await sessionService.RegisterMemberForSessionAsync(request.SessionID, request.MemberID, request.Date);
            return Ok(success);
        }
    }
}
