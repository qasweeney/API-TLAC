using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Models;
using api.Repositories;

namespace api.Services
{
    public class AuthService : IAuthService
    {
        private readonly MemberRepository mr;
        private readonly TrainerRepository tr;
        private readonly AdminRepository ar;
        private readonly AuthRepository authRep;

        public AuthService(MemberRepository memberRepository, TrainerRepository trainerRepository, AdminRepository adminRepository, AuthRepository authRepository)
        {
            mr = memberRepository;
            tr = trainerRepository;
            ar = adminRepository;
            authRep = authRepository;
        }

        public async Task<(bool IsSuccess, int UserId, string UserType)> AuthenticateAsync(string userName, string password, string userType)
        {
            switch (userType.ToLower())
            {
                case "member":
                    var member = await mr.GetMemberByEmailAsync(userName);
                    if (member != null && member.Password == password && member.Banned == 0)
                    {
                        return (true, member.MemberID, "member");
                    }
                    else
                    {
                        return (false, 0, "");
                    }
                case "trainer":
                    var trainer = await tr.GetTrainerByEmailAsync(userName);
                    if (trainer != null && trainer.Password == password && trainer.Banned == 0)
                    {
                        return (true, trainer.TrainerID, "trainer");
                    }
                    else
                    {
                        return (false, 0, "");
                    }
                case "admin":
                    var admin = await ar.GetAdminByEmailAsync(userName);
                    if (admin != null && admin.Password == password)
                    {
                        return (true, admin.AdminID, "admin");
                    }
                    else
                    {
                        return (false, 0, "");
                    }
                default:
                    throw new ArgumentException("Invalid user type");
            }
        }

        public async Task<string?> GenerateTokenAsync(int userId, string userType)
        {
            string token = Guid.NewGuid().ToString();
            DateTime expiresAt = DateTime.UtcNow.AddHours(2);

            bool isStored = await authRep.StoreTokenAsync(userId, userType, token, expiresAt);
            return isStored ? token : null;
        }

        public async Task<(int userId, string userType)?> ValidateTokenAsync(string token)
        {
            return await authRep.ValidateTokenAsync(token);
        }

        public async Task<bool> LogoutAsync(string token)
        {
            return await authRep.DeleteTokenAsync(token);
        }

    }
}