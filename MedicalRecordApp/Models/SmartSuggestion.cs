using System;

namespace MedicalRecordApp.Models
{
    public class SmartSuggestion
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Value { get; set; }
        public int UsageCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

