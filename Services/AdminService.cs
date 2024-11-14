using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Repositories;

namespace api.Services
{
    public class AdminService : IAdminService
    {
        private readonly AdminRepository ar;
        public AdminService(AdminRepository adminRepository)
        {
            ar = adminRepository;
        }
        public async Task<List<Admin>> GetAllAdminsAsync()
        {
            return await ar.GetAllAdminsAsnyc();
        }
    }
}