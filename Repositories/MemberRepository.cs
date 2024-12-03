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
            var query = @"SELECT * FROM Member";
            return await db.ExecuteQueryAsync(query, reader => new Member
            {
                MemberID = reader.GetInt32("MemberID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                Password = reader.GetString("Password"),
                Phone = reader.GetString("Phone"),
                Banned = reader.GetInt16("Banned")
            });
        }
        public async Task<Member?> GetMemberByEmailAsync(string email)
        {
            var query = "SELECT * FROM Member WHERE Email = @Email";
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
                Password = reader.GetString("Password"),
                Banned = reader.GetInt16("Banned")
            }, parameters);
            return member.FirstOrDefault();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            var query = "SELECT * FROM Member WHERE MemberID = @id";
            var parameters = new[]{
                new MySqlParameter("@id", id)
            };
            var member = await db.ExecuteQueryAsync(query, reader => new Member
            {
                MemberID = reader.GetInt32("MemberID"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Phone = reader.GetString("Phone"),
                Password = ""
            }, parameters);
            return member.FirstOrDefault();
        }
        public async Task<bool> RegisterMemberAsync(Member member)
        {
            var query = @"
        INSERT INTO Member (Email, RegistrationDate, Password, FirstName, LastName, Phone)
        VALUES (@Email, @RegistrationDate, @Password, @FirstName, @LastName, @Phone);
        SELECT LAST_INSERT_ID();";
            var parameters = new[]
            {
                new MySqlParameter("@Email", member.Email),
                new MySqlParameter("@RegistrationDate", member.RegistrationDate),
                new MySqlParameter("@Password", member.Password),
                new MySqlParameter("@FirstName", member.FirstName),
                new MySqlParameter("@LastName", member.LastName),
                new MySqlParameter("@Phone", member.Phone)
            };

            var result = await db.ExecuteNonQueryAsync(query, parameters);


            return result > 0;
        }

        public async Task<bool> BanMemberAsync(int id)
        {
            string query = @"
                UPDATE Member
                SET Banned = CASE 
                            WHEN Banned = 1 THEN 0 
                            ELSE 1 
                            END
                WHERE MemberID = @MemberID;
                ";
            var parameters = new[]{
                new MySqlParameter("@MemberID", id)
            };

            var result = await db.ExecuteNonQueryAsync(query, parameters);

            return result > 0;
        }
    }
}