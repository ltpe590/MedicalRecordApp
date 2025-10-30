using MedicalRecordApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MedicalRecordApp.Services
{
    public class SmartSuggestionService
    {
        private readonly string _connectionString;

        public SmartSuggestionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public List<SmartSuggestion> GetAllSuggestions()
        {
            List<SmartSuggestion> suggestions = new List<SmartSuggestion>();

            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT * FROM smart_suggestions ORDER BY usage_count DESC, category, value", conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SmartSuggestion suggestion = new SmartSuggestion
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Category = reader["category"]?.ToString(),
                                Value = reader["value"]?.ToString(),
                                UsageCount = Convert.ToInt32(reader["usage_count"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            };
                            suggestions.Add(suggestion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all suggestions: {ex.Message}");
            }

            return suggestions;
        }

        public List<SmartSuggestion> GetSuggestionsByCategory(string category)
        {
            List<SmartSuggestion> suggestions = new List<SmartSuggestion>();

            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT * FROM smart_suggestions WHERE category = @category ORDER BY usage_count DESC, value", conn);
                    cmd.Parameters.AddWithValue("@category", category);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SmartSuggestion suggestion = new SmartSuggestion
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Category = reader["category"]?.ToString(),
                                Value = reader["value"]?.ToString(),
                                UsageCount = Convert.ToInt32(reader["usage_count"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            };
                            suggestions.Add(suggestion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting suggestions by category: {ex.Message}");
            }

            return suggestions;
        }

        public List<string> GetCategories()
        {
            List<string> categories = new List<string>();

            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT DISTINCT category FROM smart_suggestions ORDER BY category", conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(reader["category"]?.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting categories: {ex.Message}");
            }

            return categories;
        }

        public SmartSuggestion GetSuggestionById(int id)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM smart_suggestions WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new SmartSuggestion
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Category = reader["category"]?.ToString(),
                                Value = reader["value"]?.ToString(),
                                UsageCount = Convert.ToInt32(reader["usage_count"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting suggestion by ID: {ex.Message}");
            }

            return null;
        }

        public SmartSuggestion GetSuggestionByValue(string category, string value)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT * FROM smart_suggestions WHERE category = @category AND value = @value", conn);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@value", value);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new SmartSuggestion
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Category = reader["category"]?.ToString(),
                                Value = reader["value"]?.ToString(),
                                UsageCount = Convert.ToInt32(reader["usage_count"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting suggestion by value: {ex.Message}");
            }

            return null;
        }

        public bool SaveSuggestion(SmartSuggestion suggestion)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (suggestion.Id == 0) // New suggestion
                    {
                        cmd = new MySqlCommand(
                            "INSERT INTO smart_suggestions (category, value, usage_count, created_at, updated_at) " +
                            "VALUES (@category, @value, @usageCount, @createdAt, @updatedAt)", conn);
                    }
                    else // Update existing suggestion
                    {
                        cmd = new MySqlCommand(
                            "UPDATE smart_suggestions SET category = @category, value = @value, " +
                            "usage_count = @usageCount, updated_at = @updatedAt WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@id", suggestion.Id);
                    }

                    cmd.Parameters.AddWithValue("@category", suggestion.Category ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@value", suggestion.Value ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@usageCount", suggestion.UsageCount);
                    cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving suggestion: {ex.Message}");
                return false;
            }
        }

        public bool IncrementUsageCount(string category, string value)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();

                    // First, check if the suggestion already exists
                    var existingSuggestion = GetSuggestionByValue(category, value);

                    if (existingSuggestion != null)
                    {
                        // Update existing suggestion
                        MySqlCommand cmd = new MySqlCommand(
                            "UPDATE smart_suggestions SET usage_count = usage_count + 1, updated_at = @updatedAt " +
                            "WHERE category = @category AND value = @value", conn);
                        cmd.Parameters.AddWithValue("@category", category);
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                    else
                    {
                        // Create new suggestion
                        SmartSuggestion newSuggestion = new SmartSuggestion
                        {
                            Category = category,
                            Value = value,
                            UsageCount = 1,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        return SaveSuggestion(newSuggestion);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error incrementing usage count: {ex.Message}");
                return false;
            }
        }

        public bool DeleteSuggestion(int id)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM smart_suggestions WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting suggestion: {ex.Message}");
                return false;
            }
        }

        public bool DeleteSuggestionsByCategory(string category)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM smart_suggestions WHERE category = @category", conn);
                    cmd.Parameters.AddWithValue("@category", category);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting suggestions by category: {ex.Message}");
                return false;
            }
        }

        public List<string> GetTopSuggestions(string category, int limit = 5)
        {
            List<string> topSuggestions = new List<string>();

            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT value FROM smart_suggestions " +
                        "WHERE category = @category " +
                        "ORDER BY usage_count DESC, value " +
                        "LIMIT @limit", conn);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@limit", limit);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topSuggestions.Add(reader["value"]?.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting top suggestions: {ex.Message}");
            }

            return topSuggestions;
        }

        public void InitializeDefaultSuggestions()
        {
            try
            {
                var defaultSuggestions = new List<SmartSuggestion>
                {
                    // Common chief complaints
                    new SmartSuggestion { Category = "ChiefComplaint", Value = "Headache", UsageCount = 1 },
                    new SmartSuggestion { Category = "ChiefComplaint", Value = "Fever", UsageCount = 1 },
                    new SmartSuggestion { Category = "ChiefComplaint", Value = "Cough", UsageCount = 1 },
                    new SmartSuggestion { Category = "ChiefComplaint", Value = "Abdominal pain", UsageCount = 1 },
                    new SmartSuggestion { Category = "ChiefComplaint", Value = "Chest pain", UsageCount = 1 },

                    // Common assessments
                    new SmartSuggestion { Category = "Assessment", Value = "Upper respiratory infection", UsageCount = 1 },
                    new SmartSuggestion { Category = "Assessment", Value = "Hypertension", UsageCount = 1 },
                    new SmartSuggestion { Category = "Assessment", Value = "Diabetes mellitus", UsageCount = 1 },
                    new SmartSuggestion { Category = "Assessment", Value = "Back pain", UsageCount = 1 },
                    new SmartSuggestion { Category = "Assessment", Value = "Anxiety", UsageCount = 1 },

                    // Common plans
                    new SmartSuggestion { Category = "Plan", Value = "Follow up in 2 weeks", UsageCount = 1 },
                    new SmartSuggestion { Category = "Plan", Value = "Lifestyle modifications", UsageCount = 1 },
                    new SmartSuggestion { Category = "Plan", Value = "Physical therapy", UsageCount = 1 },
                    new SmartSuggestion { Category = "Plan", Value = "Refer to specialist", UsageCount = 1 },
                    new SmartSuggestion { Category = "Plan", Value = "Routine blood work", UsageCount = 1 }
                };

                foreach (var suggestion in defaultSuggestions)
                {
                    // Only add if it doesn't already exist
                    var existing = GetSuggestionByValue(suggestion.Category, suggestion.Value);
                    if (existing == null)
                    {
                        SaveSuggestion(suggestion);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing default suggestions: {ex.Message}");
            }
        }

        public int GetSuggestionCountByCategory(string category)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM smart_suggestions WHERE category = @category", conn);
                    cmd.Parameters.AddWithValue("@category", category);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting suggestion count by category: {ex.Message}");
                return 0;
            }
        }

        // New method to get suggestions for specific medical fields
        public List<string> GetMedicalSuggestions(string fieldType)
        {
            return GetTopSuggestions(fieldType, 10);
        }

        // Method to populate suggestions from database when needed
        public void PopulateSuggestionsFromDatabase()
        {
            // This method can be called to ensure default suggestions exist
            InitializeDefaultSuggestions();
        }
    }
}