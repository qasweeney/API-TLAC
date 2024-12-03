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

        public async Task<List<Trainer>> GetPendingTrainersAsync()
        {
            return await tr.GetPendingTrainersAsync();
        }

        public async Task<Trainer> ApprovePendingTrainerAsync(int trainerId)
        {
            return await tr.ApprovePendingTrainerAsync(trainerId);
        }

        public async Task<bool> BanTrainerAsync(int trainerId)
        {
            return await tr.BanTrainerAsync(trainerId);
        }

        public async Task<Trainer?> GetTrainerByIdAsync(int id)
        {
            return await tr.GetTrainerByIdAsync(id);
        }

        public async Task<Trainer> UpdateTrainerProfileAsync(int id, string bio, string profilePic)
        {
            return await tr.UpdateTrainerProfileAsync(id, bio, profilePic);
        }

        public async Task<decimal> GetTrainerAverageRatingAsync(int id)
        {
            return await tr.GetTrainerAverageRatingAsync(id);
        }

        public async Task<bool> RegisterTrainerAsync(Trainer trainer)
        {
            return await tr.RegisterTrainerAsync(trainer.FirstName, trainer.LastName, trainer.Email, trainer.Phone, trainer.Bio, trainer.Password, (decimal)trainer.SessionPrice);
        }
    }
}