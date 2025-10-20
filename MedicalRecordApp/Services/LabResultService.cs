using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; // Use this for MySQL.  Entity Framework Core is an alternative.
using System.Linq;

namespace MedicalRecordApp.Controllers
{
    public class LabResultController
    {
        private readonly string _connectionString = "YOUR_MYSQL_CONNECTION_STRING_HERE"; // Replace with your actual connection string

        public List<LabResult> GetLabResultsByVisitId(int visitId)
        {
            List<LabResult> labResults = new List<LabResult>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string cmdText = "SELECT * FROM lab_results WHERE visit_id = @visitId ORDER BY created_at DESC";
                    SqlCommand cmd = new SqlCommand(cmdText, conn);
                    cmd.Parameters.AddWithValue("@visitId", visitId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LabResult labResult = new LabResult
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                VisitId = Convert.ToInt32(reader["visit_id"]),
                                TestName = reader["test_name"]?.ToString(),
                                Result = reader["result"]?.ToString(),
                                NormalRange = reader["normal_range"]?.ToString(),
                                Units = reader["units"]?.ToString(),
                                IsAbnormal = Convert.ToBoolean(reader["is_abnormal"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                            };
                            labResults.Add(labResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting lab results by visit ID: {ex.Message}");
            }

            return labResults;
        }
    }

    // LabResult Model (Important!)
    public class LabResult
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public string TestName { get; set; }
        public string Result { get; set; }
        public string NormalRange { get; set; }
        public string Units { get; set; }
        public bool IsAbnormal { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
