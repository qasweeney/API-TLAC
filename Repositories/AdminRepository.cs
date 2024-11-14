using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using MySqlConnector;

namespace api.Repositories
{
    public class AdminRepository(Database database)
    {
        private readonly Database db = database;

        public async Task<List<Admin>> GetAllAdminsAsnyc()
        {
            var query = "SELECT * FROM admin";
            return await db.ExecuteQueryAsync(query, reader => new Admin
            {
                AdminID = reader.GetInt32("AdminID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                Password = reader.GetString("Password")
            });
        }
        public async Task<Admin?> GetAdminByEmailAsync(string email)
        {
            var query = "SELECT * FROM admin WHERE Email = @Email";
            var parameters = new[]{
                new MySqlParameter("@Email", email)
            };
            var admin = await db.ExecuteQueryAsync(query, reader => new Admin
            {
                AdminID = reader.GetInt32("AdminID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                Password = reader.GetString("Password")
            }, parameters);
            return admin.FirstOrDefault();
        }
    }
}