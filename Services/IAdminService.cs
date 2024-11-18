using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface IAdminService
    {
        Task<List<Admin>> GetAllAdminsAsync();

        Task<Admin?> GetAdminByIdAsync(int id);

    }
}