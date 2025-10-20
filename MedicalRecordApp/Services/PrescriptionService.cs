using MedicalRecordApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MedicalRecordApp.Controllers
{
    public class PrescriptionService
    {
        private readonly string _connectionString = "YOUR_MYSQL_CONNECTION_STRING_HERE"; // Replace!
        private readonly string _tableName = "prescriptions"; // Table name for easier reference

        public List<Prescription> GetPrescriptionsByVisitId(int visitId)
        {
            List<Prescription> prescriptions = new List<Prescription>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string cmdText = $"SELECT Id, MedicationName, Dosage, Frequency, Instructions FROM {_tableName} WHERE VisitId = @visitId";
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@visitId", visitId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prescriptions.Add(new Prescription
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            MedicationName = reader["MedicationName"]?.ToString(),
                            Dosage = reader["Dosage"]?.ToString(),
                            Frequency = reader["Frequency"]?.ToString(),
                            Instructions = reader["Instructions"]?.ToString()
                        });
                    }
                }
            }

            return prescriptions;
        }
    }
}
