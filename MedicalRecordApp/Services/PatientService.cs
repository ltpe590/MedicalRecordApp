using MedicalRecordApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MedicalRecordApp.Controllers
{
    public class PatientService
    {
        // ✅ Your actual MySQL connection
        private readonly string _connectionString = "Server=localhost;Database=medrecrd_db;User ID=root;Password=root;SslMode=none;";

        private readonly string _tableName = "patients"; // Table name for easier reference

        // ✅ Get all patients
        public List<Patient> GetPatients()
        {
            List<Patient> patients = new List<Patient>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string cmdText = $"SELECT id, name, address FROM {_tableName} ORDER BY id DESC";

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"]?.ToString(),
                            Address = reader["address"]?.ToString()
                        });
                    }
                }
            }

            return patients;
        }

        // ✅ Get single patient by ID
        public Patient GetPatient(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string cmdText = $"SELECT id, name, address FROM {_tableName} WHERE id = @id LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Patient
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"]?.ToString(),
                                Address = reader["address"]?.ToString()
                            };
                        }
                    }
                }
            }

            return null; // Not found
        }

        // ✅ Add new patient
        public bool CreatePatient(Patient patient)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string cmdText = $"INSERT INTO {_tableName} (name, address) VALUES (@name, @address)";

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@name", patient.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@address", patient.Address ?? (object)DBNull.Value);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        // ✅ Update existing patient
        public bool UpdatePatient(Patient patient)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string cmdText = $"UPDATE {_tableName} SET name = @name, address = @address WHERE id = @id";

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@id", patient.Id);
                    cmd.Parameters.AddWithValue("@name", patient.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@address", patient.Address ?? (object)DBNull.Value);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        // ✅ Delete patient
        public bool DeletePatient(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string cmdText = $"DELETE FROM {_tableName} WHERE id = @id";

                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
    }
}
