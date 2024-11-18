using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/admins")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly ITrainerService trainerService;
        private readonly IAuthService authService;

        public AdminController(IAdminService admnServ, ITrainerService trainerServ, IAuthService authServ)
        {
            adminService = admnServ;
            trainerService = trainerServ;
            authService = authServ;
        }

        [HttpGet]
        public async Task<ActionResult<List<Admin>>> GetAdmins()
        {
            var admins = await adminService.GetAllAdminsAsync();
            return Ok(admins);
        }

        [HttpGet("applications")]
        public async Task<ActionResult<List<Trainer>>> GetTrainerApplications()
        {
            var trainers = await trainerService.GetPendingTrainersAsync();
            return Ok(trainers);
        }
        [HttpPut("applications/approve/{id}")]
        public async Task<ActionResult<Trainer>> ApproveTrainerApplication([FromRoute] int id)
        {
            if (!Request.Cookies.TryGetValue("SessionToken", out var sessionToken))
            {
                return Unauthorized("Session token missing.");
            }

            var user = await authService.ValidateTokenAsync(sessionToken);
            if (user == null || user.Value.userType != "Admin")
            {
                return Unauthorized("Invalid session token or insufficient permissions.");
            }

            var trainer = await trainerService.ApprovePendingTrainerAsync(id);
            return Ok(trainer);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainer>> GetAdminById(int id)
        {
            var trainer = await adminService.GetAdminByIdAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }

            return Ok(trainer);
        }
    }
}