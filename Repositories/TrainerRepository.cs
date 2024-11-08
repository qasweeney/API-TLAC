using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;

namespace api.Repositories
{
    public class TrainerRepository(Database database)
    {
        private readonly Database db = database;

        public async Task<List<Trainer>> GetAllTrainersAsync()
        {
            var query = @"SELECT * FROM trainer";

            return await db.ExecuteQueryAsync(query, reader => new Trainer
            {
                TrainerID = reader.GetInt32("TrainerID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                Password = reader.GetString("Password"),
                SessionPrice = reader.GetDecimal("SessionPrice"),
                Phone = reader.GetString("Phone")
            });
        }
    }
}