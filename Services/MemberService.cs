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
    }
}