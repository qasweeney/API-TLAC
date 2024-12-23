using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService ms)
        {
            memberService = ms;
        }

        [HttpGet]
        public async Task<ActionResult<List<Member>>> GetMembers()
        {
            var members = await memberService.GetAllMembersAsync();
            return Ok(members);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMemberById(int id)
        {
            var member = await memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }
        [HttpPost("register")]
        public async Task<ActionResult<bool>> MemberRegister([FromBody] Member member)
        {
            bool result = await memberService.RegisterMemberAsync(member);
            return result;

        }
    }
}