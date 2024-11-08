using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/trainers")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService ts)
        {
            trainerService = ts;
        }
        [HttpGet]
        public async Task<ActionResult<List<Trainer>>> GetTrainers()
        {
            var trainers = await trainerService.GetAllTrainersAsync();
            return Ok(trainers);
        }
    }
}