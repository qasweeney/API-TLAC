using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using MySqlConnector;

namespace api.Repositories
{
    public class MemberRepository(Database database)
    {
        private readonly Database db = database;

        public async Task<List<Member>> GetAllMembersAsync()
        {
            var query = @"SELECT * FROM member";
            return await db.ExecuteQueryAsync(query, reader => new Member
            {
                MemberID = reader.GetInt32("MemberID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                Password = reader.GetString("Password"),
                Phone = reader.GetString("Phone")
            });
        }
        public async Task<Member?> GetMemberByEmailAsync(string email)
        {
            var query = "SELECT * FROM member WHERE Email = @Email";
            var parameters = new[]{
                new MySqlParameter("@Email", email)
            };
            var member = await db.ExecuteQueryAsync(query, reader => new Member
            {
                MemberID = reader.GetInt32("MemberID"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Phone = reader.GetString("Phone"),
                Password = reader.GetString("Password")
            }, parameters);
            return member.FirstOrDefault();
        }
    }
}