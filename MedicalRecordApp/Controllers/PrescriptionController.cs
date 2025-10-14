using MedicalRecordApp.Models;
using MedicalRecordApp.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MedicalRecordApp.Controllers
{
    public class PrescriptionController
    {
        public List<Prescription> GetPrescriptionsByVisitId(int visitId)
        {
            List<Prescription> prescriptions = new List<Prescription>();

            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT * FROM prescriptions WHERE visit_id = @visitId ORDER BY created_at DESC", conn);
                    cmd.Parameters.AddWithValue("@visitId", visitId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Prescription prescription = new Prescription
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
                            };
                            prescriptions.Add(prescription);
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

        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            List<Prescription> prescriptions = new List<Prescription>();

            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT p.* FROM prescriptions p " +
                        "INNER JOIN visits v ON p.visit_id = v.id " +
                        "WHERE v.patient_id = @patientId " +
                        "ORDER BY p.created_at DESC", conn);
                    cmd.Parameters.AddWithValue("@patientId", patientId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Prescription prescription = new Prescription
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
                            };
                            prescriptions.Add(prescription);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting prescriptions by patient ID: {ex.Message}");
            }

            return prescriptions;
        }

        public Prescription GetPrescriptionById(int id)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM prescriptions WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Prescription
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
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting prescription by ID: {ex.Message}");
            }

            return null;
        }

        public bool SavePrescription(Prescription prescription)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (prescription.Id == 0) // New prescription
                    {
                        cmd = new MySqlCommand(
                            "INSERT INTO prescriptions (visit_id, drug_name, dosage, frequency, duration, quantity, instructions) " +
                            "VALUES (@visitId, @drugName, @dosage, @frequency, @duration, @quantity, @instructions)", conn);
                    }
                    else // Update existing prescription
                    {
                        cmd = new MySqlCommand(
                            "UPDATE prescriptions SET visit_id = @visitId, drug_name = @drugName, dosage = @dosage, " +
                            "frequency = @frequency, duration = @duration, quantity = @quantity, instructions = @instructions " +
                            "WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@id", prescription.Id);
                    }

                    cmd.Parameters.AddWithValue("@visitId", prescription.VisitId);
                    cmd.Parameters.AddWithValue("@drugName", prescription.DrugName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dosage", prescription.Dosage ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@frequency", prescription.Frequency ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@duration", prescription.Duration ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@quantity", prescription.Quantity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@instructions", prescription.Instructions ?? (object)DBNull.Value);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
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
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM prescriptions WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting prescription: {ex.Message}");
                return false;
            }
        }

        public bool DeletePrescriptionsByVisitId(int visitId)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM prescriptions WHERE visit_id = @visitId", conn);
                    cmd.Parameters.AddWithValue("@visitId", visitId);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting prescriptions by visit ID: {ex.Message}");
                return false;
            }
        }

        public List<string> GetCommonDrugs(int limit = 10)
        {
            List<string> commonDrugs = new List<string>();

            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT drug_name, COUNT(*) as usage_count " +
                        "FROM prescriptions " +
                        "WHERE drug_name IS NOT NULL AND drug_name != '' " +
                        "GROUP BY drug_name " +
                        "ORDER BY usage_count DESC " +
                        "LIMIT @limit", conn);
                    cmd.Parameters.AddWithValue("@limit", limit);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            commonDrugs.Add(reader["drug_name"]?.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting common drugs: {ex.Message}");
            }

            return commonDrugs;
        }

        public int GetPrescriptionCountByVisitId(int visitId)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM prescriptions WHERE visit_id = @visitId", conn);
                    cmd.Parameters.AddWithValue("@visitId", visitId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting prescription count: {ex.Message}");
                return 0;
            }
        }
    }
}