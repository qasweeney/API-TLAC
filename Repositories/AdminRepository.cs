using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.VisualBasic;
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

        public async Task<Admin?> GetAdminByIdAsync(int id)
        {
            var query = "SELECT * FROM admin WHERE AdminId = @id";
            var parameters = new[]{
                new MySqlParameter("@id", id)
            };
            var admin = await db.ExecuteQueryAsync(query, reader => new Admin
            {
                AdminID = reader.GetInt32("AdminID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                Password = ""
            }, parameters);
            return admin.FirstOrDefault();
        }

        public async Task<AdminKPIResponse> GetAdminKPIAsync(DateOnly start, DateOnly end)
        {
            string query = @"
                SELECT
                    COUNT(*) AS TotalSessions,
                    SUM(Price) AS TotalRevenue,
                    COUNT(DISTINCT MemberID) AS ActiveMembers,
                    COUNT(DISTINCT TrainerID) AS ActiveTrainers
                FROM 
                    Session
                WHERE 
                    SessionStatus = 'Registered'
                    AND Date BETWEEN @startDate AND @endDate;
            ";

            var parameters = new[]{
                new MySqlParameter("@startDate", start),
                new MySqlParameter("@endDate", end)
            };
            var response = await db.ExecuteQueryAsync(query, reader => new AdminKPIResponse
            {
                TotalRevenue = reader.IsDBNull(reader.GetOrdinal("TotalRevenue"))
               ? 0m
               : reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),

                TotalSessions = reader.IsDBNull(reader.GetOrdinal("TotalSessions"))
                ? 0
                : reader.GetInt32(reader.GetOrdinal("TotalSessions")),

                ActiveMembers = reader.IsDBNull(reader.GetOrdinal("ActiveMembers"))
                ? 0
                : reader.GetInt32(reader.GetOrdinal("ActiveMembers")),

                ActiveTrainers = reader.IsDBNull(reader.GetOrdinal("ActiveTrainers"))
                 ? 0
                 : reader.GetInt32(reader.GetOrdinal("ActiveTrainers")),

            }, parameters);

            return response.FirstOrDefault();
        }
    }
}