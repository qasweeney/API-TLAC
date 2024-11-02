using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllSessionsAsync();
        Task<Session> GetSessionByIdAsync(int id);
        Task<Session> AddSessionAsync(Session session);
        Task<Session> UpdateSessionAsync(Session session);
        Task<bool> DeleteSessionAsync(int id);
    }
}