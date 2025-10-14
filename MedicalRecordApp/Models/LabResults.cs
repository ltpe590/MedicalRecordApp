using System;

namespace MedicalRecordApp.Models
{
    public class LabResult
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public string TestName { get; set; }
        public string Result { get; set; }
        public string NormalRange { get; set; }
        public string Units { get; set; }
        public bool IsAbnormal { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Visit Visit { get; set; }
    }
}

