using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MedicalRecordApp.Services
{
    public class SmartSuggestionService
    {
        // ✅ Actual connection string for your testing database
        private readonly string _connectionString = "Server=localhost;Database=medrecrd_db;User ID=root;Password=root;SslMode=none;";

        // ✅ Main function required by ServicesRepository
        public SmartSuggestion GetSmartSuggestion(int patientId, string query)
        {
            List<string> suggestions = new List<string>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    // 🔍 Example query for demonstration — searches a table 'smart_suggestions'
                    // You can later create this table or replace this with your own logic
                    string cmdText = @"
                        SELECT DISTINCT term
                        FROM smart_suggestions
                        WHERE term LIKE CONCAT('%', @query, '%')
                        ORDER BY usage_count DESC
                        LIMIT 10;
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                    {
                        cmd.Parameters.AddWithValue("@query", query);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                suggestions.Add(reader["term"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting smart suggestions: {ex.Message}");
            }

            // ✅ Return a SmartSuggestion object (matches your ServicesRepository call)
            return new SmartSuggestion
            {
                PatientId = patientId,
                Query = query,
                Suggestions = suggestions
            };
        }
    }

    // ✅ Simple data model for smart suggestions
    public class SmartSuggestion
    {
        public int PatientId { get; set; }
        public string Query { get; set; }
        public List<string> Suggestions { get; set; }
    }
}
