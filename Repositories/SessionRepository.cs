using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using MySqlConnector;

namespace api.Repositories
{
    public class SessionRepository(Database database)
    {
        private readonly Database db = database;

        public async Task<List<Session>> GetAllSessionsAsync()
        {
            var query = @"
            SELECT s.SessionID, s.DayOfWeek, s.Date, s.StartTime, s.SessionType, s.SessionStatus, 
                s.Price, s.TrainerID, s.MemberID,
                t.TrainerID AS Trainer_TrainerID, t.FirstName AS Trainer_FirstName, 
                t.LastName AS Trainer_LastName, t.Email AS Trainer_Email,
                t.RegistrationDate AS Trainer_RegistrationDate, 
                t.SessionPrice AS Trainer_SessionPrice, t.Phone AS Trainer_Phone,
                m.MemberID AS Member_MemberID, m.FirstName AS Member_FirstName, 
                m.LastName AS Member_LastName, m.Email AS Member_Email,
                m.RegistrationDate AS Member_RegistrationDate, 
                m.Phone AS Member_Phone,
                r.RatingID, r.RatingValue
            FROM session s
            LEFT JOIN Trainer t ON s.TrainerID = t.TrainerID
            LEFT JOIN Member m ON s.MemberID = m.MemberID
            LEFT JOIN Rating r ON s.SessionID = r.SessionID";


            return await db.ExecuteQueryAsync(query, reader => new Session
            {
                SessionID = reader.GetInt32("SessionID"),
                DayOfWeek = reader.IsDBNull("DayOfWeek") ? null : Enum.Parse<DayOfWeek>(reader.GetString("DayOfWeek")),
                Date = reader.IsDBNull("Date") ? null : reader.GetDateTime("Date"),
                StartTime = reader.GetTimeSpan("StartTime"),
                SessionType = Enum.Parse<SessionType>(reader.GetString("SessionType")),
                SessionStatus = Enum.Parse<SessionStatus>(reader.GetString("SessionStatus")),
                Price = reader.GetDecimal("Price"),
                TrainerID = reader.GetInt32("TrainerID"),
                Trainer = new Trainer
                {
                    TrainerID = reader.GetInt32("Trainer_TrainerID"),
                    FirstName = reader.GetString("Trainer_FirstName"),
                    LastName = reader.GetString("Trainer_LastName"),
                    Email = reader.GetString("Trainer_Email"),
                    RegistrationDate = reader.GetDateTime("Trainer_RegistrationDate"),
                    SessionPrice = reader.GetDecimal("Trainer_SessionPrice"),
                    Phone = reader.GetString("Trainer_Phone")
                },
                MemberID = reader.IsDBNull("MemberID") ? null : reader.GetInt32("MemberID"),
                Member = reader.IsDBNull("Member_MemberID") ? null : new Member
                {
                    MemberID = reader.GetInt32("Member_MemberID"),
                    FirstName = reader.GetString("Member_FirstName"),
                    LastName = reader.GetString("Member_LastName"),
                    Email = reader.GetString("Member_Email"),
                    RegistrationDate = reader.GetDateTime("Member_RegistrationDate"),
                    Phone = reader.GetString("Member_Phone")
                },
                RatingID = reader.IsDBNull("RatingID") ? null : reader.GetInt32("RatingID"),
                Rating = reader.IsDBNull("RatingID") ? null : new Rating
                {
                    RatingID = reader.GetInt32("RatingID"),
                    RatingValue = reader.GetDecimal("RatingValue"),
                    SessionID = reader.GetInt32("SessionID")
                }
            });
        }

        public async Task<Session?> GetSessionByIdAsync(int id)
        {
            var query = @"
            SELECT s.SessionID, s.DayOfWeek, s.Date, s.StartTime, s.SessionType, s.SessionStatus, 
                s.Price, s.TrainerID, s.MemberID,
                t.TrainerID AS Trainer_TrainerID, t.FirstName AS Trainer_FirstName, 
                t.LastName AS Trainer_LastName, t.Email AS Trainer_Email,
                t.RegistrationDate AS Trainer_RegistrationDate, 
                t.SessionPrice AS Trainer_SessionPrice, t.Phone AS Trainer_Phone,
                m.MemberID AS Member_MemberID, m.FirstName AS Member_FirstName, 
                m.LastName AS Member_LastName, m.Email AS Member_Email,
                m.RegistrationDate AS Member_RegistrationDate, 
                m.Phone AS Member_Phone,
                r.RatingID, r.RatingValue, r.SessionID AS Rating_SessionID
            FROM session s
            LEFT JOIN Trainer t ON s.TrainerID = t.TrainerID
            LEFT JOIN Member m ON s.MemberID = m.MemberID
            LEFT JOIN Rating r ON s.SessionID = r.SessionID
            WHERE s.SessionID = @SessionID";

            var parameters = new MySqlParameter[]{
                new MySqlParameter("@SessionID", id)
            };
            var sessions = await db.ExecuteQueryAsync(query, reader => new Session
            {
                SessionID = reader.GetInt32("SessionID"),
                Date = reader.IsDBNull("Date") ? (DateTime?)null : reader.GetDateTime("Date"),
                DayOfWeek = reader.IsDBNull("DayOfWeek") ? (DayOfWeek?)null : Enum.Parse<DayOfWeek>(reader.GetString("DayOfWeek")),
                StartTime = reader.GetTimeSpan("StartTime"),
                SessionType = Enum.Parse<SessionType>(reader.GetString("SessionType")),
                SessionStatus = Enum.Parse<SessionStatus>(reader.GetString("SessionStatus")),
                Price = reader.GetDecimal("Price"),
                TrainerID = reader.GetInt32("TrainerID"),
                MemberID = reader.IsDBNull("MemberID") ? (int?)null : reader.GetInt32("MemberID"),

                Trainer = new Trainer
                {
                    TrainerID = reader.GetInt32("Trainer_TrainerID"),
                    FirstName = reader.GetString("Trainer_FirstName"),
                    LastName = reader.GetString("Trainer_LastName"),
                    Email = reader.GetString("Trainer_Email"),
                    RegistrationDate = reader.GetDateTime("Trainer_RegistrationDate"),
                    SessionPrice = reader.GetDecimal("Trainer_SessionPrice"),
                    Phone = reader.GetString("Trainer_Phone")
                },

                Member = reader.IsDBNull("Member_MemberID") ? null : new Member
                {
                    MemberID = reader.GetInt32("Member_MemberID"),
                    FirstName = reader.GetString("Member_FirstName"),
                    LastName = reader.GetString("Member_LastName"),
                    Email = reader.GetString("Member_Email"),
                    RegistrationDate = reader.GetDateTime("Member_RegistrationDate"),
                    Phone = reader.GetString("Member_Phone")
                },
                RatingID = reader.IsDBNull("RatingID") ? null : reader.GetInt32("RatingID"),
                Rating = reader.IsDBNull("RatingID") ? null : new Rating
                {
                    RatingID = reader.GetInt32("RatingID"),
                    RatingValue = reader.GetDecimal("RatingValue"),
                    SessionID = reader.GetInt32("SessionID")
                }
            },
        parameters);
            return sessions.FirstOrDefault();
        }
    }
}