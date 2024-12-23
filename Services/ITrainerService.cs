using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface ITrainerService
    {
        Task<List<Trainer>> GetAllTrainersAsync();

        Task<List<Trainer>> GetPendingTrainersAsync();

        Task<Trainer> ApprovePendingTrainerAsync(int trainerId);
        Task<Trainer?> GetTrainerByIdAsync(int id);
        Task<Trainer> UpdateTrainerProfileAsync(int id, string bio, string profilePic);
        Task<decimal> GetTrainerAverageRatingAsync(int id);
        Task<bool> RegisterTrainerAsync(Trainer trainer);
        Task<bool> BanTrainerAsync(int trainerId);
    }
}