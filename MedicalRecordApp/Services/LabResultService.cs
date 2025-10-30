using MedicalRecordApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MedicalRecordApp.Services
{
    public class LabResultService
    {
        private readonly string _connectionString;

        public LabResultService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<LabResult> GetLabResultsByVisitId(int visitId)
        {
            var labResults = new List<LabResult>();

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmdText = @"SELECT id, visit_id, test_name, result, normal_range, units, is_abnormal, created_at 
                                  FROM lab_results WHERE visit_id = @visitId ORDER BY created_at DESC";
                    var cmd = new MySqlCommand(cmdText, conn);
                    cmd.Parameters.AddWithValue("@visitId", visitId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labResults.Add(new LabResult
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                VisitId = Convert.ToInt32(reader["visit_id"]),
                                TestName = reader["test_name"]?.ToString(),
                                Result = reader["result"]?.ToString(),
                                NormalRange = reader["normal_range"]?.ToString(),
                                Units = reader["units"]?.ToString(),
                                IsAbnormal = Convert.ToBoolean(reader["is_abnormal"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                            });
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

        public bool SaveLabResult(LabResult labResult)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (labResult.Id == 0)
                    {
                        cmd = new MySqlCommand(
                            @"INSERT INTO lab_results (visit_id, test_name, result, normal_range, units, is_abnormal) 
                            VALUES (@visitId, @testName, @result, @normalRange, @units, @isAbnormal)",
                            conn);
                    }
                    else
                    {
                        cmd = new MySqlCommand(
                            @"UPDATE lab_results SET visit_id = @visitId, test_name = @testName, 
                            result = @result, normal_range = @normalRange, units = @units, is_abnormal = @isAbnormal 
                            WHERE id = @id",
                            conn);
                        cmd.Parameters.AddWithValue("@id", labResult.Id);
                    }

                    cmd.Parameters.AddWithValue("@visitId", labResult.VisitId);
                    cmd.Parameters.AddWithValue("@testName", labResult.TestName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@result", labResult.Result ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@normalRange", labResult.NormalRange ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@units", labResult.Units ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@isAbnormal", labResult.IsAbnormal);

                    return cmd.ExecuteNonQuery() > 0;
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
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("DELETE FROM lab_results WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting lab result: {ex.Message}");
                return false;
            }
        }
    }
}