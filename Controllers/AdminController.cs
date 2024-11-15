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

        public AdminController(IAdminService admnServ, ITrainerService trainerServ)
        {
            adminService = admnServ;
            trainerService = trainerServ;
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
            var trainer = await trainerService.ApprovePendingTrainerAsync(id);
            return Ok(trainer);
        }
    }
}