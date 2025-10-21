using MedicalRecordApp.Models;
using MedicalRecordApp.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient; // Add this


namespace MedicalRecordApp.Controllers
{
    public class PatientController
    {
        public List<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();

            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM patients ORDER BY name", conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Patient patient = new Patient
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                Gender = reader["gender"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                                Phone = reader["phone"].ToString(),
                                Address = reader["address"].ToString(),
                                Note = reader["note"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            };

                            patients.Add(patient);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In a real application, you would log this error
                Console.WriteLine($"Error getting patients: {ex.Message}");
            }

            return patients;
        }

        public Patient GetPatientById(int id)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM patients WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Patient
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                Gender = reader["gender"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                                Phone = reader["phone"].ToString(),
                                Address = reader["address"].ToString(),
                                Note = reader["note"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting patient by ID: {ex.Message}");
            }

            return null;
        }

        public bool SavePatient(Patient patient)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (patient.Id == 0) // New patient
                    {
                        cmd = new MySqlCommand(
                            "INSERT INTO patients (name, gender, date_of_birth, phone, address, note) " +
                            "VALUES (@name, @gender, @dob, @phone, @address, @note)", conn);
                    }
                    else // Update existing patient
                    {
                        cmd = new MySqlCommand(
                            "UPDATE patients SET name = @name, gender = @gender, date_of_birth = @dob, " +
                            "phone = @phone, address = @address, note = @note WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@id", patient.Id);
                    }

                    cmd.Parameters.AddWithValue("@name", patient.Name);
                    cmd.Parameters.AddWithValue("@gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@dob", patient.DateOfBirth);
                    cmd.Parameters.AddWithValue("@phone", patient.Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@address", patient.Address ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@note", patient.Note ?? (object)DBNull.Value);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving patient: {ex.Message}");
                return false;
            }
        }

        public bool DeletePatient(int id)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM patients WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting patient: {ex.Message}");
                return false;
            }
        }
    }
}
