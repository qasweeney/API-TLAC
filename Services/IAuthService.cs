using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public interface IAuthService
    {
        Task<(bool IsSuccess, int UserId, string UserType)> AuthenticateAsync(string userName, string password, string userType);
        public Task<string?> GenerateTokenAsync(int userId, string userType);
        public Task<(int userId, string userType)?> ValidateTokenAsync(string token);
        public Task<bool> LogoutAsync(string token);
    }
}