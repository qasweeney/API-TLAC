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
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainer>> GetTrainerById(int id)
        {
            var trainer = await trainerService.GetTrainerByIdAsync(id);
            return Ok(trainer);
        }

        [HttpPut("{id}/profile")]
        public async Task<ActionResult> UpdateTrainerProfile(TrainerProfileUpdate profileUpdate, int id)
        {
            var result = await trainerService.UpdateTrainerProfileAsync(id, profileUpdate.Bio, profileUpdate.ProfilePic);
            return Ok(result);
        }
        [HttpGet("{id}/average-rating")]
        public async Task<ActionResult<decimal>> GetTrainerAverageRating(int id)
        {
            var result = await trainerService.GetTrainerAverageRatingAsync(id);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> TrainerRegister([FromBody] Trainer trainer)
        {
            bool result = await trainerService.RegisterTrainerAsync(trainer);
            return result;
        }
    }
}