using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public void ValidateSchema() => _settingsRepository.EnsureAppSettingsTable();
        public void EnsureAppSettingsTable() => ValidateSchema();

        public string GetValue(string key, string defaultValue = "") =>
            _settingsRepository.GetValue(key, defaultValue);

        public int GetIntValue(string key, int defaultValue) =>
            _settingsRepository.GetIntValue(key, defaultValue);

        public bool GetYesNoValue(string key, bool defaultValue) =>
            _settingsRepository.GetYesNoValue(key, defaultValue);

        public void SaveValues(IReadOnlyDictionary<string, string> settings) =>
            _settingsRepository.SaveValues(settings);

        public DataTable GetDeviceCategories() =>
            _settingsRepository.GetDeviceCategories();

        public DataTable GetRoles() =>
            _settingsRepository.GetRoles();

        public void AddDeviceCategory(string categoryName) =>
            _settingsRepository.AddDeviceCategory(categoryName);
    }
}
