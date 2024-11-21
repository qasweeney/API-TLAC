using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface ISessionService
    {
        Task<List<Session>> GetAllSessionsAsync();
        Task<Session?> GetSessionByIdAsync(int id);
        Task<Session> CreateSessionAsync(Session session);
        Task<List<Session>> GetSessionsByTrainerIdAsync(int id);

        // Task<Session> UpdateSessionAsync(int id, Session updatedSession);
        // Task<bool> DeleteSessionAsync(int id);
    }
}