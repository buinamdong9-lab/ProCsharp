using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class SettingsService(ISettingsRepository settingsRepository) : ISettingsService
    {
        public void ValidateSchema() => settingsRepository.EnsureAppSettingsTable();
        public void EnsureAppSettingsTable() => ValidateSchema();

        public string GetValue(string key, string defaultValue = "") =>
            settingsRepository.GetValue(key, defaultValue);

        public int GetIntValue(string key, int defaultValue) =>
            settingsRepository.GetIntValue(key, defaultValue);

        public bool GetYesNoValue(string key, bool defaultValue) =>
            settingsRepository.GetYesNoValue(key, defaultValue);

        public void SaveValues(IReadOnlyDictionary<string, string> settings) =>
            settingsRepository.SaveValues(settings);

        public List<DeviceCategoryStatsModel> GetDeviceCategories() =>
            settingsRepository.GetDeviceCategories();

        public List<RoleStatsModel> GetRoles() =>
            settingsRepository.GetRoles();

        public void AddDeviceCategory(string categoryName) =>
            settingsRepository.AddDeviceCategory(categoryName);
    }
}
