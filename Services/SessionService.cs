using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public async Task<List<Session>> GetSessionsByMemberIdAsync(int id)
        {
            return await sr.GetSessionsByMemberIdAsync(id);
        }

        public async Task<Session> EditSessionRatingAsync(decimal rating, int sessionId)
        {
            return await sr.EditSessionRatingAsync(rating, sessionId);
        }

        public async Task<bool> EditScheduleRemoveAsync(int sessionId)
        {
            return await sr.EditScheduleRemoveAsync(sessionId);
        }

        public async Task<bool> EditScheduleAddAsync(AddRecurring recurring)
        {
            return await sr.EditScheduleAddAsync(recurring);
        }

        public async Task<bool> SessionUnregisterAsync(int id)
        {
            return await sr.SessionUnregisterAsync(id);
        }

        public async Task<List<Session>> SessionSearchAsync(SessionSearch request)
        {
            DateTime date = DateTime.Parse(request.Date);
            TimeSpan? time = string.IsNullOrEmpty(request.Time) ? null : TimeSpan.Parse(request.Time);
            return await sr.SessionSearchAsync(date, time, request.AvailableOnly);
        }

        public async Task<bool> RegisterMemberForSessionAsync(int sessionId, int memberId, DateTime? date)
        {
            return await sr.RegisterMemberForSessionAsync(sessionId, memberId, date);
        }

        public async Task<List<ScheduleEntry>> GetTrainerScheduleAsync(int trainerID)
        {
            return await sr.GetTrainerScheduleAsync(trainerID);
        }

        // public async Task<Session> UpdateSessionAsync(Session session){
        //     return await sr.
        // }
    }
}