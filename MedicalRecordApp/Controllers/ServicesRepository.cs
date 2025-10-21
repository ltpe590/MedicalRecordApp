using MedicalRecordApp.Models;
using MedicalRecordApp.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MedicalRecordApp.Controllers
{
    public class ServicesRepository
    {
        private readonly PatientService _patientService;
        private readonly PrescriptionService _prescriptionService;
        private readonly VisitService _visitService;
        private readonly SmartSuggestionService _smartSuggestionService; // Added SmartSuggestionService

        public ServicesRepository(PatientService patientService, PrescriptionService prescriptionService, VisitService visitService, SmartSuggestionService smartSuggestionService)
        {
            _patientService = patientService;
            _prescriptionService = prescriptionService;
            _visitService = visitService;
            _smartSuggestionService = smartSuggestionService;
        }

        public MySqlConnection GetConnection()
        {
            // Implement your database connection logic here.
            // This is a placeholder; replace with your actual connection code.
            return new MySqlConnection("YOUR_MYSQL_CONNECTION_STRING_HERE");
        }

        public Patient GetPatient(int patientId)
        {
            return _patientService.GetPatient(patientId);
        }

        public List<Prescription> GetPrescriptionsByVisitId(int visitId)
        {
            return _prescriptionService.GetPrescriptionsByVisitId(visitId);
        }

        public List<Visit> GetVisitsByPatientId(int patientId)
        {
            return _visitService.GetVisitsByPatientId(patientId);
        }

        public SmartSuggestion GetSmartSuggestion(int patientId, string query)
        {
            return _smartSuggestionService.GetSmartSuggestion(patientId, query);
        }
    }
}
