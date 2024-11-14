using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Data;
using Microsoft.Extensions.Configuration.UserSecrets;
using MySqlConnector;

namespace api.Repositories
{
    public class AuthRepository(Database database)
    {
        private readonly Database db = database;

        public async Task<bool> StoreTokenAsync(int userId, string userType, string token, DateTime expiresAt)
        {
            var checkQuery = @"SELECT Token FROM SessionTokens WHERE UserId = @userId AND UserType = @userType";
            var checkParams = new[]{
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@userType", userType)
            };

            var existingToken = await db.ExecuteQueryAsync(checkQuery, reader => reader.GetString("Token"), checkParams);

            if (existingToken.Any())
            {
                var deleteQuery = @"DELETE FROM SessionTokens WHERE UserId = @userId AND UserType = @userType";
                await db.ExecuteNonQueryAsync(deleteQuery, checkParams);
            }

            string query = "INSERT INTO SessionTokens (UserId, UserType, Token, ExpiresAt) VALUES (@userID, @userType, @token, @expiresAt)";
            var parameters = new[]{
                new MySqlParameter("@userID", userId),
                new MySqlParameter("@userType", userType),
                new MySqlParameter("@token", token),
                new MySqlParameter("@expiresAt", expiresAt)
            };
            var response = await db.ExecuteNonQueryAsync(query, parameters);
            return response > 0;
        }

        public async Task<(int userId, string userType)?> ValidateTokenAsync(string token)
        {
            var query = @"SELECT UserId, UserType 
                      FROM SessionTokens 
                      WHERE Token = @token AND ExpiresAt > NOW()";

            var parameters = new[]{
                new MySqlParameter("@token", token)
            };

            var response = await db.ExecuteQueryAsync(query, reader => (
                userId: reader.GetInt32("UserId"),
                userType: reader.GetString("UserType")
            ), parameters);

            return response.FirstOrDefault();
        }

        public async Task<bool> DeleteTokenAsync(string token)
        {
            string query = "DELETE FROM SessionTokens WHERE Token = @token";
            var parameters = new[]{
                new MySqlParameter("@token", token)
            };

            var resopnse = await db.ExecuteNonQueryAsync(query, parameters);
            return resopnse > 0;
        }
    }
}