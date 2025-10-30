using MedicalRecordApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MedicalRecordApp.Services
{
    public class PrescriptionService
    {
        private readonly string _connectionString;

        public PrescriptionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Prescription> GetPrescriptionsByVisitId(int visitId)
        {
            var prescriptions = new List<Prescription>();

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmdText = @"SELECT id, visit_id, drug_name, dosage, frequency, duration, 
                                  quantity, instructions, created_at 
                                  FROM prescriptions WHERE visit_id = @visitId ORDER BY created_at DESC";
                    var cmd = new MySqlCommand(cmdText, conn);
                    cmd.Parameters.AddWithValue("@visitId", visitId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prescriptions.Add(new Prescription
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                VisitId = Convert.ToInt32(reader["visit_id"]),
                                DrugName = reader["drug_name"]?.ToString(),
                                Dosage = reader["dosage"]?.ToString(),
                                Frequency = reader["frequency"]?.ToString(),
                                Duration = reader["duration"]?.ToString(),
                                Quantity = reader["quantity"]?.ToString(),
                                Instructions = reader["instructions"]?.ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting prescriptions by visit ID: {ex.Message}");
            }

            return prescriptions;
        }

        public bool SavePrescription(Prescription prescription)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (prescription.Id == 0)
                    {
                        cmd = new MySqlCommand(
                            @"INSERT INTO prescriptions (visit_id, drug_name, dosage, frequency, duration, quantity, instructions) 
                            VALUES (@visitId, @drugName, @dosage, @frequency, @duration, @quantity, @instructions)",
                            conn);
                    }
                    else
                    {
                        cmd = new MySqlCommand(
                            @"UPDATE prescriptions SET visit_id = @visitId, drug_name = @drugName, 
                            dosage = @dosage, frequency = @frequency, duration = @duration, 
                            quantity = @quantity, instructions = @instructions 
                            WHERE id = @id",
                            conn);
                        cmd.Parameters.AddWithValue("@id", prescription.Id);
                    }

                    cmd.Parameters.AddWithValue("@visitId", prescription.VisitId);
                    cmd.Parameters.AddWithValue("@drugName", prescription.DrugName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dosage", prescription.Dosage ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@frequency", prescription.Frequency ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@duration", prescription.Duration ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@quantity", prescription.Quantity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@instructions", prescription.Instructions ?? (object)DBNull.Value);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving prescription: {ex.Message}");
                return false;
            }
        }

        public bool DeletePrescription(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("DELETE FROM prescriptions WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting prescription: {ex.Message}");
                return false;
            }
        }
    }
}