using MedicalRecordApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MedicalRecordApp.Controllers
{
    public class VisitService
    {
        private readonly string _connectionString = "YOUR_MYSQL_CONNECTION_STRING_HERE"; // Replace!
        private readonly string _tableName = "visits"; // Table name for easier reference

        public List<Visit> GetVisitsByPatientId(int patientId)
        {
            List<Visit> visits = new List<Visit>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string cmdText = $"SELECT Id, PatientId, AppointmentDateTime, Notes FROM {_tableName} WHERE PatientId = @patientId";
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@patientId", patientId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        visits.Add(new Visit
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            AppointmentDateTime = reader["AppointmentDateTime"]?.ToString(),
                            Notes = reader["Notes"]?.ToString()
                        });
                    }
                }
            }

            return visits;
        }
    }
}
