using MedicalRecordApp.Models;
using MedicalRecordApp.Services;
using System;
using System.Collections.Generic;

namespace MedicalRecordApp.Controllers
{
    public class SmartSuggestionController
    {
        private readonly SmartSuggestionService _suggestionService;

        public SmartSuggestionController()
        {
            // Initialize with your connection string
            _suggestionService = new SmartSuggestionService("host=localhost;user=root;password=root;database=medrecord_db");
        }

        public List<SmartSuggestion> GetAllSuggestions()
        {
            return _suggestionService.GetAllSuggestions();
        }

        public List<SmartSuggestion> GetSuggestionsByCategory(string category)
        {
            return _suggestionService.GetSuggestionsByCategory(category);
        }

        public List<string> GetCategories()
        {
            return _suggestionService.GetCategories();
        }

        public SmartSuggestion GetSuggestionById(int id)
        {
            return _suggestionService.GetSuggestionById(id);
        }

        public SmartSuggestion GetSuggestionByValue(string category, string value)
        {
            return _suggestionService.GetSuggestionByValue(category, value);
        }

        public bool SaveSuggestion(SmartSuggestion suggestion)
        {
            return _suggestionService.SaveSuggestion(suggestion);
        }

        public bool IncrementUsageCount(string category, string value)
        {
            return _suggestionService.IncrementUsageCount(category, value);
        }

        public bool DeleteSuggestion(int id)
        {
            return _suggestionService.DeleteSuggestion(id);
        }

        public bool DeleteSuggestionsByCategory(string category)
        {
            return _suggestionService.DeleteSuggestionsByCategory(category);
        }

        public List<string> GetTopSuggestions(string category, int limit = 5)
        {
            return _suggestionService.GetTopSuggestions(category, limit);
        }

        public void InitializeDefaultSuggestions()
        {
            _suggestionService.InitializeDefaultSuggestions();
        }

        public int GetSuggestionCountByCategory(string category)
        {
            return _suggestionService.GetSuggestionCountByCategory(category);
        }
    }
}