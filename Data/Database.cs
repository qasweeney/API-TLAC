using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MySqlConnector;

namespace api.Data
{
    public class Database
    {
        private readonly string cs;

        public Database(string connectionString)
        {
            cs = connectionString;
        }

        private MySqlConnection OpenConnection()
        {
            var connection = new MySqlConnection(cs);
            connection.Open();
            return connection;
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string query, Func<MySqlDataReader, T> mappingFunction, params MySqlParameter[] parameters)
        {
            using var connection = OpenConnection();
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddRange(parameters);

            var results = new List<T>();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(mappingFunction(reader));
            }

            return results;
        }

        public async Task<int> ExecuteNonQueryAsync(string query, params MySqlParameter[] parameters)
        {
            using var connection = OpenConnection();
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddRange(parameters);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<object> ExecuteScalarAsync(string query, params MySqlParameter[] parameters)
        {
            using var connection = OpenConnection();
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddRange(parameters);

            return await command.ExecuteScalarAsync();
        }
    }
}