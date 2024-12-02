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

        public async Task<Admin?> GetAdminByIdAsync(int id)
        {
            return await ar.GetAdminByIdAsync(id);
        }
        public async Task<AdminKPIResponse> GetAdminKPIAsync(AdminKPIRequest request)
        {
            DateOnly startDate = request.StartDate;
            DateOnly endDate = request.EndDate;
            return await ar.GetAdminKPIAsync(startDate, endDate);
        }
    }
}