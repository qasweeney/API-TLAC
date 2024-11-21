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

        public async Task<List<Session>> GetSessionsByTrainerIdAsync(int id)
        {
            return await sr.GetSessionsByTrainerIdAsync(id);
        }

        public async Task<List<Session>> SessionSearchAsync(SessionSearch request)
        {
            DateTime date = DateTime.Parse(request.Date);
            TimeSpan? time = string.IsNullOrEmpty(request.Time) ? null : TimeSpan.Parse(request.Time);
            return await sr.SessionSearchAsync(date, time);
        }

        public async Task<bool> RegisterMemberForSessionAsync(int sessionId, int memberId, DateTime? date)
        {
            return await sr.RegisterMemberForSessionAsync(sessionId, memberId, date);
        }

        // public async Task<Session> UpdateSessionAsync(Session session){
        //     return await sr.
        // }
    }
}