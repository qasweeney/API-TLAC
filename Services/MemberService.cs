using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Repositories;

namespace api.Services
{
    public class MemberService : IMemberService
    {
        private readonly MemberRepository mr;

        public MemberService(MemberRepository memberRepository)
        {
            mr = memberRepository;
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await mr.GetAllMembersAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await mr.GetMemberByIdAsync(id);
        }
        public async Task<bool> RegisterMemberAsync(Member member)
        {
            return await mr.RegisterMemberAsync(member);
        }

        public async Task<bool> BanMemberAsync(int id)
        {
            return await mr.BanMemberAsync(id);
        }
    }
}