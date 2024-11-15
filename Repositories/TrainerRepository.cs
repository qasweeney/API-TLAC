using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.VisualBasic;
using MySqlConnector;

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
        public async Task<Trainer?> GetTrainerByEmailAsync(string email)
        {
            var query = "SELECT * FROM trainer WHERE Email = @Email";
            var parameters = new[]{
                new MySqlParameter("@Email", email)
            };
            var trainer = await db.ExecuteQueryAsync(query, reader => new Trainer
            {
                TrainerID = reader.GetInt32("TrainerID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                Password = reader.GetString("Password"),
                SessionPrice = reader.GetDecimal("SessionPrice"),
                Phone = reader.GetString("Phone")
            }, parameters);
            return trainer.FirstOrDefault();
        }

        public async Task<List<Trainer>> GetPendingTrainersAsync()
        {
            var query = "SELECT * FROM trainer WHERE Active = 0";
            var trainers = await db.ExecuteQueryAsync(query, reader => new Trainer
            {
                TrainerID = reader.GetInt32("TrainerID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                Password = reader.GetString("Password"),
                SessionPrice = reader.GetDecimal("SessionPrice"),
                Phone = reader.GetString("Phone")
            }, []);

            return trainers;
        }

        public async Task<Trainer> ApprovePendingTrainerAsync(int trainerId)
        {
            var updateQuery = "UPDATE Trainer SET Active = 1 WHERE TrainerID = @TrainerID";
            var selectQuery = "SELECT * FROM Trainer WHERE TrainerID = @TrainerID";
            var parameters = new[]{
                new MySqlParameter("@TrainerID", trainerId
                )
            };

            await db.ExecuteNonQueryAsync(updateQuery, parameters);
            var trainers = await db.ExecuteQueryAsync(selectQuery, reader => new Trainer
            {
                TrainerID = reader.GetInt32("TrainerID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                Password = reader.GetString("Password"),
                SessionPrice = reader.GetDecimal("SessionPrice"),
                Phone = reader.GetString("Phone")
            }, parameters);
            // System.Console.WriteLine(trainer);
            return trainers.FirstOrDefault();
        }
    }
}