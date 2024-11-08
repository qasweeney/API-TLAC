using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Repositories;

namespace api.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly TrainerRepository tr;

        public TrainerService(TrainerRepository trainerRepository)
        {
            tr = trainerRepository;
        }

        public async Task<List<Trainer>> GetAllTrainersAsync()
        {
            return await tr.GetAllTrainersAsync();
        }
    }
}