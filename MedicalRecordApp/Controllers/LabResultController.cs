using MedicalRecordApp.Models;
using MedicalRecordApp.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MedicalRecordApp.Controllers
{
    public class LabResultController
    {
        private readonly string _connectionString;

        public LabResultController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<LabResult> GetLabResultsByVisitId(int visitId)
        {
            List<LabResult> labResults = new List<LabResult>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT * FROM lab_results WHERE visit_id = @visitId ORDER BY created_at DESC", conn);
                    cmd.Parameters.AddWithValue("@visitId", visitId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
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

        public List<LabResult> GetLabResultsByPatientId(int patientId)
        {
            List<LabResult> labResults = new List<LabResult>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT lr.* FROM lab_results lr " +
                        "INNER JOIN visits v ON lr.visit_id = v.id " +
                        "WHERE v.patient_id = @patientId " +
                        "ORDER BY lr.created_at DESC", conn);
                    cmd.Parameters.AddWithValue("@patientId", patientId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
                Console.WriteLine($"Error getting lab results by patient ID: {ex.Message}");
            }

            return labResults;
        }

        public LabResult GetLabResultById(int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM lab_results WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new LabResult
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting lab result by ID: {ex.Message}");
            }

            return null;
        }

        public bool SaveLabResult(LabResult labResult)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (labResult.Id == 0) // New lab result
                    {
                        cmd = new MySqlCommand(
                            "INSERT INTO lab_results (visit_id, test_name, result, normal_range, units, is_abnormal) " +
                            "VALUES (@visitId, @testName, @result, @normalRange, @units, @isAbnormal)", conn);
                    }
                    else // Update existing lab result
                    {
                        cmd = new MySqlCommand(
                            "UPDATE lab_results SET visit_id = @visitId, test_name = @testName, result = @result, " +
                            "normal_range = @normalRange, units = @units, is_abnormal = @isAbnormal " +
                            "WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@id", labResult.Id);
                    }

                    cmd.Parameters.AddWithValue("@visitId", labResult.VisitId);
                    cmd.Parameters.AddWithValue("@testName", labResult.TestName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@result", labResult.Result ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@normalRange", labResult.NormalRange ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@units", labResult.Units ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@isAbnormal", labResult.IsAbnormal);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving lab result: {ex.Message}");
                return false;
            }
        }

        public bool DeleteLabResult(int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM lab_results WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting lab result: {ex.Message}");
                return false;
            }
        }

        public bool DeleteLabResultsByVisitId(int visitId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM lab_results WHERE visit_id = @visitId", conn);
                    cmd.Parameters.AddWithValue("@visitId", visitId);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting lab results by visit ID: {ex.Message}");
                return false;
            }
        }

        public List<string> GetCommonTests(int limit = 10)
        {
            List<string> commonTests = new List<string>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT test_name, COUNT(*) as usage_count " +
                        "FROM lab_results " +
                        "WHERE test_name IS NOT NULL AND test_name != '' " +
                        "GROUP BY test_name " +
                        "ORDER BY usage_count DESC " +
                        "LIMIT @limit", conn);
                    cmd.Parameters.AddWithValue("@limit", limit);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            commonTests.Add(reader["test_name"]?.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting common tests: {ex.Message}");
            }

            return commonTests;
        }

        public List<LabResult> GetAbnormalResultsByPatientId(int patientId)
        {
            List<LabResult> abnormalResults = new List<LabResult>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT lr.* FROM lab_results lr " +
                        "INNER JOIN visits v ON lr.visit_id = v.id " +
                        "WHERE v.patient_id = @patientId AND lr.is_abnormal = true " +
                        "ORDER BY lr.created_at DESC", conn);
                    cmd.Parameters.AddWithValue("@patientId", patientId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
                            abnormalResults.Add(labResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting abnormal lab results: {ex.Message}");
            }

            return abnormalResults;
        }

        public int GetLabResultCountByVisitId(int visitId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM lab_results WHERE visit_id = @visitId", conn);
                    cmd.Parameters.AddWithValue("@visitId", visitId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting lab result count: {ex.Message}");
                return 0;
            }
        }

        public int GetAbnormalResultCountByPatientId(int patientId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM lab_results lr " +
                        "INNER JOIN visits v ON lr.visit_id = v.id " +
                        "WHERE v.patient_id = @patientId AND lr.is_abnormal = true", conn);
                    cmd.Parameters.AddWithValue("@patientId", patientId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting abnormal result count: {ex.Message}");
                return 0;
            }
        }
    }
}