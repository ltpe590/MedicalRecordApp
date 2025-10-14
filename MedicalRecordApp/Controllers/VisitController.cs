using MedicalRecordApp.Models;
using MedicalRecordApp.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MedicalRecordApp.Controllers
{
    public class VisitController
    {
        public List<Visit> GetAllVisits()
        {
            List<Visit> visits = new List<Visit>();

            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT v.*, p.name as patient_name " +
                        "FROM visits v " +
                        "LEFT JOIN patients p ON v.patient_id = p.id " +
                        "ORDER BY v.visit_date DESC", conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Visit visit = new Visit
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                PatientId = Convert.ToInt32(reader["patient_id"]),
                                VisitDate = Convert.ToDateTime(reader["visit_date"]),
                                ScheduledFor = reader["scheduled_for"] == DBNull.Value ?
                                    null : (DateTime?)Convert.ToDateTime(reader["scheduled_for"]),
                                Status = reader["status"]?.ToString(),
                                ChiefComplaint = reader["chief_complaint"]?.ToString(),
                                Duration = reader["duration"]?.ToString(),
                                HistoryOfPresentIllness = reader["history_of_present_illness"]?.ToString(),
                                PastMedicalHistory = reader["past_medical_history"]?.ToString(),
                                FamilyHistory = reader["family_history"]?.ToString(),
                                SocialHistory = reader["social_history"]?.ToString(),
                                ReviewOfSystems = reader["review_of_systems"]?.ToString(),
                                PhysicalExamination = reader["physical_examination"]?.ToString(),
                                Temperature = reader["temperature"]?.ToString(),
                                BloodPressure = reader["blood_pressure"]?.ToString(),
                                HeartRate = reader["heart_rate"]?.ToString(),
                                RespiratoryRate = reader["respiratory_rate"]?.ToString(),
                                OxygenSaturation = reader["oxygen_saturation"]?.ToString(),
                                Weight = reader["weight"]?.ToString(),
                                Height = reader["height"]?.ToString(),
                                BMI = reader["bmi"]?.ToString(),
                                Assessment = reader["assessment"]?.ToString(),
                                Plan = reader["plan"]?.ToString(),
                                FollowUp = reader["follow_up"]?.ToString(),
                                Notes = reader["notes"]?.ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                Patient = new Patient { Name = reader["patient_name"]?.ToString() }
                            };
                            visits.Add(visit);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting visits: {ex.Message}");
            }

            return visits;
        }

        public List<Visit> GetVisitsByPatientId(int patientId)
        {
            List<Visit> visits = new List<Visit>();

            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT * FROM visits WHERE patient_id = @patientId ORDER BY visit_date DESC", conn);
                    cmd.Parameters.AddWithValue("@patientId", patientId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Visit visit = new Visit
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                PatientId = Convert.ToInt32(reader["patient_id"]),
                                VisitDate = Convert.ToDateTime(reader["visit_date"]),
                                ScheduledFor = reader["scheduled_for"] == DBNull.Value ?
                                    null : (DateTime?)Convert.ToDateTime(reader["scheduled_for"]),
                                Status = reader["status"]?.ToString(),
                                ChiefComplaint = reader["chief_complaint"]?.ToString(),
                                Duration = reader["duration"]?.ToString(),
                                HistoryOfPresentIllness = reader["history_of_present_illness"]?.ToString(),
                                PastMedicalHistory = reader["past_medical_history"]?.ToString(),
                                FamilyHistory = reader["family_history"]?.ToString(),
                                SocialHistory = reader["social_history"]?.ToString(),
                                ReviewOfSystems = reader["review_of_systems"]?.ToString(),
                                PhysicalExamination = reader["physical_examination"]?.ToString(),
                                Temperature = reader["temperature"]?.ToString(),
                                BloodPressure = reader["blood_pressure"]?.ToString(),
                                HeartRate = reader["heart_rate"]?.ToString(),
                                RespiratoryRate = reader["respiratory_rate"]?.ToString(),
                                OxygenSaturation = reader["oxygen_saturation"]?.ToString(),
                                Weight = reader["weight"]?.ToString(),
                                Height = reader["height"]?.ToString(),
                                BMI = reader["bmi"]?.ToString(),
                                Assessment = reader["assessment"]?.ToString(),
                                Plan = reader["plan"]?.ToString(),
                                FollowUp = reader["follow_up"]?.ToString(),
                                Notes = reader["notes"]?.ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            };
                            visits.Add(visit);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting visits by patient ID: {ex.Message}");
            }

            return visits;
        }

        public Visit GetVisitById(int id)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM visits WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Visit
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                PatientId = Convert.ToInt32(reader["patient_id"]),
                                VisitDate = Convert.ToDateTime(reader["visit_date"]),
                                ScheduledFor = reader["scheduled_for"] == DBNull.Value ?
                                    null : (DateTime?)Convert.ToDateTime(reader["scheduled_for"]),
                                Status = reader["status"]?.ToString(),
                                ChiefComplaint = reader["chief_complaint"]?.ToString(),
                                Duration = reader["duration"]?.ToString(),
                                HistoryOfPresentIllness = reader["history_of_present_illness"]?.ToString(),
                                PastMedicalHistory = reader["past_medical_history"]?.ToString(),
                                FamilyHistory = reader["family_history"]?.ToString(),
                                SocialHistory = reader["social_history"]?.ToString(),
                                ReviewOfSystems = reader["review_of_systems"]?.ToString(),
                                PhysicalExamination = reader["physical_examination"]?.ToString(),
                                Temperature = reader["temperature"]?.ToString(),
                                BloodPressure = reader["blood_pressure"]?.ToString(),
                                HeartRate = reader["heart_rate"]?.ToString(),
                                RespiratoryRate = reader["respiratory_rate"]?.ToString(),
                                OxygenSaturation = reader["oxygen_saturation"]?.ToString(),
                                Weight = reader["weight"]?.ToString(),
                                Height = reader["height"]?.ToString(),
                                BMI = reader["bmi"]?.ToString(),
                                Assessment = reader["assessment"]?.ToString(),
                                Plan = reader["plan"]?.ToString(),
                                FollowUp = reader["follow_up"]?.ToString(),
                                Notes = reader["notes"]?.ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting visit by ID: {ex.Message}");
            }

            return null;
        }

        public bool SaveVisit(Visit visit)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (visit.Id == 0) // New visit
                    {
                        cmd = new MySqlCommand(
                            "INSERT INTO visits (patient_id, visit_date, scheduled_for, status, chief_complaint, " +
                            "duration, history_of_present_illness, past_medical_history, family_history, " +
                            "social_history, review_of_systems, physical_examination, temperature, blood_pressure, " +
                            "heart_rate, respiratory_rate, oxygen_saturation, weight, height, bmi, assessment, " +
                            "plan, follow_up, notes) " +
                            "VALUES (@patientId, @visitDate, @scheduledFor, @status, @chiefComplaint, " +
                            "@duration, @historyOfPresentIllness, @pastMedicalHistory, @familyHistory, " +
                            "@socialHistory, @reviewOfSystems, @physicalExamination, @temperature, @bloodPressure, " +
                            "@heartRate, @respiratoryRate, @oxygenSaturation, @weight, @height, @bmi, " +
                            "@assessment, @plan, @followUp, @notes)", conn);
                    }
                    else // Update existing visit
                    {
                        cmd = new MySqlCommand(
                            "UPDATE visits SET patient_id = @patientId, visit_date = @visitDate, " +
                            "scheduled_for = @scheduledFor, status = @status, chief_complaint = @chiefComplaint, " +
                            "duration = @duration, history_of_present_illness = @historyOfPresentIllness, " +
                            "past_medical_history = @pastMedicalHistory, family_history = @familyHistory, " +
                            "social_history = @socialHistory, review_of_systems = @reviewOfSystems, " +
                            "physical_examination = @physicalExamination, temperature = @temperature, " +
                            "blood_pressure = @bloodPressure, heart_rate = @heartRate, " +
                            "respiratory_rate = @respiratoryRate, oxygen_saturation = @oxygenSaturation, " +
                            "weight = @weight, height = @height, bmi = @bmi, assessment = @assessment, " +
                            "plan = @plan, follow_up = @followUp, notes = @notes WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@id", visit.Id);
                    }

                    cmd.Parameters.AddWithValue("@patientId", visit.PatientId);
                    cmd.Parameters.AddWithValue("@visitDate", visit.VisitDate);
                    cmd.Parameters.AddWithValue("@scheduledFor", visit.ScheduledFor ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", visit.Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@chiefComplaint", visit.ChiefComplaint ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@duration", visit.Duration ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@historyOfPresentIllness", visit.HistoryOfPresentIllness ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pastMedicalHistory", visit.PastMedicalHistory ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@familyHistory", visit.FamilyHistory ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@socialHistory", visit.SocialHistory ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@reviewOfSystems", visit.ReviewOfSystems ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@physicalExamination", visit.PhysicalExamination ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@temperature", visit.Temperature ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@bloodPressure", visit.BloodPressure ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@heartRate", visit.HeartRate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@respiratoryRate", visit.RespiratoryRate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@oxygenSaturation", visit.OxygenSaturation ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@weight", visit.Weight ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@height", visit.Height ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@bmi", visit.BMI ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@assessment", visit.Assessment ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@plan", visit.Plan ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@followUp", visit.FollowUp ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@notes", visit.Notes ?? (object)DBNull.Value);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving visit: {ex.Message}");
                return false;
            }
        }

        public bool DeleteVisit(int id)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM visits WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting visit: {ex.Message}");
                return false;
            }
        }

        public int GetVisitCountByPatientId(int patientId)
        {
            try
            {
                using (MySqlConnection conn = DatabaseService.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM visits WHERE patient_id = @patientId", conn);
                    cmd.Parameters.AddWithValue("@patientId", patientId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting visit count: {ex.Message}");
                return 0;
            }
        }
    }
}