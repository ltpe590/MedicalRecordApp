using System;

namespace MedicalRecordApp.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public string DrugName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string Quantity { get; set; }
        public string Instructions { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Visit Visit { get; set; }
    }
}

