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
                s.Price, s.TrainerID, s.MemberID, s.Rating,
                t.TrainerID AS Trainer_TrainerID, t.FirstName AS Trainer_FirstName, 
                t.LastName AS Trainer_LastName, t.Email AS Trainer_Email,
                t.RegistrationDate AS Trainer_RegistrationDate, 
                t.SessionPrice AS Trainer_SessionPrice, t.Phone AS Trainer_Phone,
                m.MemberID AS Member_MemberID, m.FirstName AS Member_FirstName, 
                m.LastName AS Member_LastName, m.Email AS Member_Email,
                m.RegistrationDate AS Member_RegistrationDate, 
                m.Phone AS Member_Phone
            FROM session s
            LEFT JOIN Trainer t ON s.TrainerID = t.TrainerID
            LEFT JOIN Member m ON s.MemberID = m.MemberID";


            return await db.ExecuteQueryAsync(query, reader => new Session
            {
                SessionID = reader.GetInt32("SessionID"),
                DayOfWeek = reader.IsDBNull("DayOfWeek") ? null : reader.GetString("DayOfWeek"),
                Date = reader.IsDBNull("Date") ? null : reader.GetDateTime("Date"),
                StartTime = reader.GetTimeSpan("StartTime"),
                SessionType = reader.GetString("SessionType"),
                SessionStatus = reader.GetString("SessionStatus"),
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
                Rating = reader.IsDBNull("Rating") ? null : reader.GetDecimal("Rating")
            });
        }

        public async Task<Session?> GetSessionByIdAsync(int id)
        {
            var query = @"
            SELECT s.SessionID, s.DayOfWeek, s.Date, s.StartTime, s.SessionType, s.SessionStatus, 
                s.Price, s.TrainerID, s.MemberID, s.Rating,
                t.TrainerID AS Trainer_TrainerID, t.FirstName AS Trainer_FirstName, 
                t.LastName AS Trainer_LastName, t.Email AS Trainer_Email,
                t.RegistrationDate AS Trainer_RegistrationDate, 
                t.SessionPrice AS Trainer_SessionPrice, t.Phone AS Trainer_Phone,
                m.MemberID AS Member_MemberID, m.FirstName AS Member_FirstName, 
                m.LastName AS Member_LastName, m.Email AS Member_Email,
                m.RegistrationDate AS Member_RegistrationDate, 
                m.Phone AS Member_Phone
            FROM session s
            LEFT JOIN Trainer t ON s.TrainerID = t.TrainerID
            LEFT JOIN Member m ON s.MemberID = m.MemberID
            WHERE s.SessionID = @SessionID";

            var parameters = new MySqlParameter[]{
                new MySqlParameter("@SessionID", id)
            };
            var sessions = await db.ExecuteQueryAsync(query, reader => new Session
            {
                SessionID = reader.GetInt32("SessionID"),
                Date = reader.IsDBNull("Date") ? (DateTime?)null : reader.GetDateTime("Date"),
                DayOfWeek = reader.IsDBNull("DayOfWeek") ? null : reader.GetString("DayOfWeek"),
                StartTime = reader.GetTimeSpan("StartTime"),
                SessionType = reader.GetString("SessionType"),
                SessionStatus = reader.GetString("SessionStatus"),
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
                Rating = reader.IsDBNull("Rating") ? null : reader.GetDecimal("Rating")
                // RatingID = reader.IsDBNull("RatingID") ? null : reader.GetInt32("RatingID"),
                // Rating = reader.IsDBNull("RatingID") ? null : new Rating
                // {
                //     RatingID = reader.GetInt32("RatingID"),
                //     RatingValue = reader.GetDecimal("RatingValue"),
                //     SessionID = reader.GetInt32("SessionID")
                // }
            },
        parameters);
            return sessions.FirstOrDefault();
        }

        public async Task<Session> CreateSessionAsync(Session session)
        {
            var query = @"
        INSERT INTO Session (DayOfWeek, Date, StartTime, SessionType, SessionStatus, Price, TrainerID)
        VALUES (@DayOfWeek, @Date, @StartTime, @SessionType, @SessionStatus, @Price, @TrainerID);
        SELECT LAST_INSERT_ID();";
            var parameters = new[]
            {
                new MySqlParameter("@DayOfWeek", session.DayOfWeek),
                new MySqlParameter("@Date", session.Date.HasValue ? session.Date.Value : DBNull.Value),
                new MySqlParameter("@StartTime", session.StartTime),
                new MySqlParameter("@SessionType", session.SessionType),
                new MySqlParameter("@SessionStatus", session.SessionStatus),
                new MySqlParameter("@Price", session.Price),
                new MySqlParameter("@TrainerID", session.TrainerID)
            };

            var sessionId = Convert.ToInt32(await db.ExecuteScalarAsync(query, parameters));
            session.SessionID = sessionId;

            return session;
        }

        public async Task<List<Session>> GetSessionsByTrainerIdAsync(int id)
        {
            var query = @"
            SELECT s.SessionID, s.DayOfWeek, s.Date, s.StartTime, s.SessionType, s.SessionStatus, 
                s.Price, s.TrainerID, s.MemberID, s.Rating,
                t.TrainerID AS Trainer_TrainerID, t.FirstName AS Trainer_FirstName, 
                t.LastName AS Trainer_LastName, t.Email AS Trainer_Email,
                t.RegistrationDate AS Trainer_RegistrationDate, 
                t.SessionPrice AS Trainer_SessionPrice, t.Phone AS Trainer_Phone,
                m.MemberID AS Member_MemberID, m.FirstName AS Member_FirstName, 
                m.LastName AS Member_LastName, m.Email AS Member_Email,
                m.RegistrationDate AS Member_RegistrationDate, 
                m.Phone AS Member_Phone
            FROM session s
            LEFT JOIN Trainer t ON s.TrainerID = t.TrainerID
            LEFT JOIN Member m ON s.MemberID = m.MemberID
            WHERE s.TrainerID = @TrainerID";

            var parameters = new MySqlParameter[]{
                new MySqlParameter("@TrainerID", id)
            };
            var sessions = await db.ExecuteQueryAsync(query, reader => new Session
            {
                SessionID = reader.GetInt32("SessionID"),
                Date = reader.IsDBNull("Date") ? (DateTime?)null : reader.GetDateTime("Date"),
                DayOfWeek = reader.IsDBNull("DayOfWeek") ? null : reader.GetString("DayOfWeek"),
                StartTime = reader.GetTimeSpan("StartTime"),
                SessionType = reader.GetString("SessionType"),
                SessionStatus = reader.GetString("SessionStatus"),
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
                Rating = reader.IsDBNull("Rating") ? null : reader.GetDecimal("Rating")
            },
        parameters);
            return sessions;
        }

        public async Task<List<Session>> GetSessionsByMemberIdAsync(int id)
        {
            var query = @"
            SELECT s.SessionID, s.DayOfWeek, s.Date, s.StartTime, s.SessionType, s.SessionStatus, 
                s.Price, s.TrainerID, s.MemberID, s.Rating,
                t.TrainerID AS Trainer_TrainerID, t.FirstName AS Trainer_FirstName, 
                t.LastName AS Trainer_LastName, t.Email AS Trainer_Email,
                t.RegistrationDate AS Trainer_RegistrationDate, 
                t.SessionPrice AS Trainer_SessionPrice, t.Phone AS Trainer_Phone,
                m.MemberID AS Member_MemberID, m.FirstName AS Member_FirstName, 
                m.LastName AS Member_LastName, m.Email AS Member_Email,
                m.RegistrationDate AS Member_RegistrationDate, 
                m.Phone AS Member_Phone
            FROM session s
            LEFT JOIN Trainer t ON s.TrainerID = t.TrainerID
            LEFT JOIN Member m ON s.MemberID = m.MemberID
            WHERE s.MemberID = @MemberID";

            var parameters = new MySqlParameter[]{
                new MySqlParameter("@MemberID", id)
            };
            var sessions = await db.ExecuteQueryAsync(query, reader => new Session
            {
                SessionID = reader.GetInt32("SessionID"),
                Date = reader.IsDBNull("Date") ? (DateTime?)null : reader.GetDateTime("Date"),
                DayOfWeek = reader.IsDBNull("DayOfWeek") ? null : reader.GetString("DayOfWeek"),
                StartTime = reader.GetTimeSpan("StartTime"),
                SessionType = reader.GetString("SessionType"),
                SessionStatus = reader.GetString("SessionStatus"),
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
                Rating = reader.IsDBNull("Rating") ? null : reader.GetDecimal("Rating")
            },
        parameters);
            return sessions;
        }

        public async Task<List<Session>> SessionSearchAsync(DateTime date, TimeSpan? time, bool availableOnly)
        {
            var query = @"
                SELECT s.SessionID, s.DayOfWeek, s.Date, s.StartTime, s.SessionType, s.SessionStatus, 
                    s.Price, s.TrainerID, s.MemberID, s.Rating,
                    t.TrainerID AS Trainer_TrainerID, t.FirstName AS Trainer_FirstName, 
                    t.LastName AS Trainer_LastName, t.Email AS Trainer_Email,
                    t.RegistrationDate AS Trainer_RegistrationDate, 
                    t.SessionPrice AS Trainer_SessionPrice, t.Phone AS Trainer_Phone,
                    m.MemberID AS Member_MemberID, m.FirstName AS Member_FirstName, 
                    m.LastName AS Member_LastName, m.Email AS Member_Email,
                    m.RegistrationDate AS Member_RegistrationDate, 
                    m.Phone AS Member_Phone
                FROM session s
                LEFT JOIN Trainer t ON s.TrainerID = t.TrainerID
                LEFT JOIN Member m ON s.MemberID = m.MemberID
                WHERE 
                    (s.Date = @Date OR (s.Date IS NULL AND s.DayOfWeek = @DayOfWeek))
                    AND (@Time IS NULL OR s.StartTime = @Time)
                    AND (@AvailableOnly = 0 
                        OR (s.SessionStatus = 'Available' AND s.MemberID IS NULL 
                            AND NOT EXISTS (
                                SELECT 1 
                                FROM session r 
                                WHERE r.ParentSessionID = s.SessionID AND r.Date = @Date
                            )))";

            string dayOfWeek = date.DayOfWeek.ToString();
            System.Console.WriteLine(dayOfWeek);
            var parameters = new[]
            {
                new MySqlParameter("@Date", date),
                new MySqlParameter("@DayOfWeek", dayOfWeek),
                new MySqlParameter("@Time", time.HasValue ? (object)time.Value : DBNull.Value),
                new MySqlParameter("@AvailableOnly", availableOnly)
            };

            var sessions = await db.ExecuteQueryAsync(query, reader => new Session
            {
                SessionID = reader.GetInt32("SessionID"),
                Date = reader.IsDBNull("Date") ? (DateTime?)null : reader.GetDateTime("Date"),
                DayOfWeek = reader.IsDBNull("DayOfWeek") ? null : reader.GetString("DayOfWeek"),
                StartTime = reader.GetTimeSpan("StartTime"),
                SessionType = reader.GetString("SessionType"),
                SessionStatus = reader.GetString("SessionStatus"),
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
                Rating = reader.IsDBNull("Rating") ? null : reader.GetDecimal("Rating")
            }, parameters);

            return sessions;
        }

        public async Task<bool> RegisterMemberForSessionAsync(int sessionId, int memberId, DateTime? date)
        {
            var session = await GetSessionByIdAsync(sessionId);
            if (session == null)
            {
                return false;
            }
            if (session.Date == null)
            {
                var query = @"
                    INSERT INTO session (DayOfWeek, Date, StartTime, SessionType, SessionStatus, Price, TrainerID, MemberID, Rating, ParentSessionID)
                    SELECT DayOfWeek, @Date, StartTime, SessionType, @SessionStatus, Price, TrainerID, @MemberID, Rating, @SessionID
                    FROM session
                    WHERE SessionID = @SessionID";

                var parameters = new[]
                {
                    new MySqlParameter("@SessionID", sessionId),
                    new MySqlParameter("@Date", date),
                    new MySqlParameter("@MemberID", memberId),
                    new MySqlParameter("@SessionStatus", "Registered")
                };

                var rowsInserted = await db.ExecuteNonQueryAsync(query, parameters);

                return rowsInserted > 0;
            }
            else
            {
                var updateQuery = @"
                    UPDATE session
                    SET MemberID = @MemberID, SessionStatus = @NewStatus
                    WHERE SessionID = @SessionID AND MemberID IS NULL";

                var updateParameters = new[]
                {
                    new MySqlParameter("@SessionID", sessionId),
                    new MySqlParameter("@MemberID", memberId),
                    new MySqlParameter("@NewStatus", "Registered")
                };

                var rowsUpdated = await db.ExecuteNonQueryAsync(updateQuery, updateParameters);

                return rowsUpdated > 0;
            }
        }

        public async Task<Session> EditSessionRatingAsync(decimal rating, int sessionId)
        {

            var updateQuery = "UPDATE Session SET Rating = @Rating WHERE SessionID = @SessionID";
            var selectQuery = @"
            SELECT s.SessionID, s.DayOfWeek, s.Date, s.StartTime, s.SessionType, s.SessionStatus, 
                s.Price, s.TrainerID, s.MemberID, s.Rating,
                t.TrainerID AS Trainer_TrainerID, t.FirstName AS Trainer_FirstName, 
                t.LastName AS Trainer_LastName, t.Email AS Trainer_Email,
                t.RegistrationDate AS Trainer_RegistrationDate, 
                t.SessionPrice AS Trainer_SessionPrice, t.Phone AS Trainer_Phone,
                m.MemberID AS Member_MemberID, m.FirstName AS Member_FirstName, 
                m.LastName AS Member_LastName, m.Email AS Member_Email,
                m.RegistrationDate AS Member_RegistrationDate, 
                m.Phone AS Member_Phone
            FROM session s
            LEFT JOIN Trainer t ON s.TrainerID = t.TrainerID
            LEFT JOIN Member m ON s.MemberID = m.MemberID
            WHERE s.SessionID = @SessionID";
            var parameters = new[]{
                new MySqlParameter("@SessionID", sessionId),
                new MySqlParameter("@Rating", rating)
            };

            await db.ExecuteNonQueryAsync(updateQuery, parameters);

            var sessions = await db.ExecuteQueryAsync(selectQuery, reader => new Session
            {
                SessionID = reader.GetInt32("SessionID"),
                Date = reader.IsDBNull("Date") ? (DateTime?)null : reader.GetDateTime("Date"),
                DayOfWeek = reader.IsDBNull("DayOfWeek") ? null : reader.GetString("DayOfWeek"),
                StartTime = reader.GetTimeSpan("StartTime"),
                SessionType = reader.GetString("SessionType"),
                SessionStatus = reader.GetString("SessionStatus"),
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
                Rating = reader.IsDBNull("Rating") ? null : reader.GetDecimal("Rating")
            }, parameters);
            return sessions.FirstOrDefault();


            // var rowsInserted = await db.ExecuteNonQueryAsync(query, parameters);

            // return rowsInserted > 0;
        }

        // var query = @"
        //     UPDATE session
        //     SET MemberID = @MemberID, SessionStatus = @NewStatus
        //     WHERE SessionID = @SessionID AND MemberID IS NULL";

        // var parameters = new[]
        // {
        //     new MySqlParameter("@SessionID", sessionId),
        //     new MySqlParameter("@MemberID", memberId),
        //     new MySqlParameter("@NewStatus", "Registered")
        // };

        // var rowsAffected = await db.ExecuteNonQueryAsync(query, parameters);

        // return rowsAffected > 0;
    }

}