using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMembersAsync();

        Task<Member?> GetMemberByIdAsync(int it);
        Task<bool> RegisterMemberAsync(Member member);

    }
}