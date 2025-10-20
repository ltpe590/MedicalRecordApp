using MedicalRecordApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MedicalRecordApp.Controllers
{
    public class PatientService
    {
        private readonly string _connectionString = "YOUR_MYSQL_CONNECTION_STRING_HERE"; // Replace!
        private readonly string _tableName = "patients"; // Table name for easier reference

        public List<Patient> GetPatients()
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string cmdText = $"SELECT Id, Name, Address FROM {_tableName}";  // Dynamic table name

                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"]?.ToString(),
                            Address = reader["Address"]?.ToString()
                        });
                    }
                }
            }

            return patients;
        }

        // Add other methods here (e.g., CreatePatient, UpdatePatient, DeletePatient)
    }
}
