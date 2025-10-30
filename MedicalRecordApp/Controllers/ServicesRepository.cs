using MySql.Data.MySqlClient;
using System;

namespace MedicalRecordApp.Services
{
    public static class DatabaseService
    {
        private static string _connectionString = "host=localhost;user=root;password=root;database=medrecord_db";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        // You can set the connection string from configuration
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}