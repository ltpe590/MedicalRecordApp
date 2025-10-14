using System;
using System.Collections.Generic;

namespace MedicalRecordApp.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime? ScheduledFor { get; set; }
        public string Status { get; set; }
        public string ChiefComplaint { get; set; }
        public string Duration { get; set; }
        public string HistoryOfPresentIllness { get; set; }
        public string PastMedicalHistory { get; set; }
        public string FamilyHistory { get; set; }
        public string SocialHistory { get; set; }
        public string ReviewOfSystems { get; set; }
        public string PhysicalExamination { get; set; }
        public string Temperature { get; set; }
        public string BloodPressure { get; set; }
        public string HeartRate { get; set; }
        public string RespiratoryRate { get; set; }
        public string OxygenSaturation { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string BMI { get; set; }
        public string Assessment { get; set; }
        public string Plan { get; set; }
        public string FollowUp { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public Patient Patient { get; set; }
        public List<Prescription> Prescriptions { get; set; }
        public List<LabResult> LabResults { get; set; }
    }
}

