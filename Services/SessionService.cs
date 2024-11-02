using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Repositories;

namespace api.Services
{
    public class SessionService : ISessionService
    {
        private readonly SessionRepository sr;

        public SessionService(SessionRepository sessionRepository)
        {
            sr = sessionRepository;
        }

        public async Task<List<Session>> GetAllSessionsAsync()
        {
            return await sr.GetAllSessionsAsync();
        }

        public async Task<Session?> GetSessionByIdAsync(int id)
        {
            return await sr.GetSessionByIdAsync(id);
        }

        public async Task<Session> CreateSessionAsync(Session session)
        {
            return await sr.CreateSessionAsync(session);
        }
    }
}