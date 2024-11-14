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

    }
}