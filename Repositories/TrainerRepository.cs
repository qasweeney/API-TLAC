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
            var query = @"SELECT * FROM Trainer";

            return await db.ExecuteQueryAsync(query, reader => new Trainer
            {
                TrainerID = reader.GetInt32("TrainerID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                Password = "",
                SessionPrice = reader.GetDecimal("SessionPrice"),
                Phone = reader.GetString("Phone"),
                Bio = reader.GetString("TrainerBio"),
                ProfilePic = reader.GetString("TrainerPic")
            });
        }
        public async Task<Trainer?> GetTrainerByEmailAsync(string email)
        {
            var query = "SELECT * FROM Trainer WHERE Email = @Email";
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
                Phone = reader.GetString("Phone"),
                Bio = reader.GetString("TrainerBio"),
                ProfilePic = reader.GetString("TrainerPic")
            }, parameters);
            return trainer.FirstOrDefault();
        }

        public async Task<List<Trainer>> GetPendingTrainersAsync()
        {
            var query = "SELECT * FROM Trainer WHERE Active = 0";
            var trainers = await db.ExecuteQueryAsync(query, reader => new Trainer
            {
                TrainerID = reader.GetInt32("TrainerID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                Password = "",
                SessionPrice = reader.GetDecimal("SessionPrice"),
                Phone = reader.GetString("Phone"),
                Bio = reader.GetString("TrainerBio"),
                ProfilePic = reader.GetString("TrainerPic")
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
                Password = "",
                SessionPrice = reader.GetDecimal("SessionPrice"),
                Phone = reader.GetString("Phone"),
                Bio = reader.GetString("TrainerBio"),
                ProfilePic = reader.GetString("TrainerPic")
            }, parameters);
            // System.Console.WriteLine(trainer);
            return trainers.FirstOrDefault();
        }

        public async Task<Trainer?> GetTrainerByIdAsync(int id)
        {
            string query = "SELECT * FROM Trainer WHERE TrainerID = @id";
            var parameters = new[]{
                new MySqlParameter("@id", id)
            };



            var trainer = await db.ExecuteQueryAsync(query, reader => new Trainer
            {
                TrainerID = reader.GetInt32("TrainerID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                RegistrationDate = reader.GetDateTime("RegistrationDate"),
                Password = "",
                SessionPrice = reader.GetDecimal("SessionPrice"),
                Phone = reader.GetString("Phone"),
                Bio = reader.GetString("TrainerBio"),
                ProfilePic = reader.GetString("TrainerPic"),
                IsActive = reader.GetInt16("Active")
            }, parameters);

            return trainer.FirstOrDefault();
        }

        public async Task<Trainer> UpdateTrainerProfileAsync(int id, string bio, string profilePic)
        {
            string updateQuery = "UPDATE Trainer SET TrainerBio=@Bio, TrainerPic=@Pic WHERE TrainerID=@ID";
            if (bio == null)
            {
                updateQuery = "UPDATE Trainer SET TrainerPic=@Pic WHERE TrainerID=@ID";
            }
            else if (profilePic == null)
            {
                updateQuery = "UPDATE Trainer SET TrainerBio=@Bio WHERE TrainerID=@ID";
            }

            var parameters = new[]{
                new MySqlParameter("@Bio", bio),
                new MySqlParameter("@Pic", profilePic),
                new MySqlParameter("@ID", id)
            };
            await db.ExecuteNonQueryAsync(updateQuery, parameters);
            var trainer = await GetTrainerByIdAsync(id);
            return trainer;
        }
    }
}