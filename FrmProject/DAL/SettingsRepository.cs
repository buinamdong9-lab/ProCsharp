using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Dapper;

namespace FrmProject.DAL
{
    public class SettingsRepository : ISettingsRepository
    {
        public void EnsureAppSettingsTable()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            DbSchemaHelper.ValidateAppSettingsSchema(conn);
        }

        public string GetValue(string key, string defaultValue = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            return GetValue(conn, key, defaultValue);
        }

        public int GetIntValue(string key, int defaultValue)
        {
            string value = GetValue(key, defaultValue.ToString());
            return int.TryParse(value, out int parsed) ? parsed : defaultValue;
        }

        public bool GetYesNoValue(string key, bool defaultValue)
        {
            string value = GetValue(key, defaultValue ? "Có" : "Không");
            if (value.Equals("Có", System.StringComparison.OrdinalIgnoreCase))
                return true;
            if (value.Equals("Không", System.StringComparison.OrdinalIgnoreCase))
                return false;

            return defaultValue;
        }

        public void SaveValues(IReadOnlyDictionary<string, string> settings)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            foreach (var setting in settings)
                SaveValue(conn, setting.Key, setting.Value);
        }

        public DataTable GetDeviceCategories()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            DataTable dt = new DataTable();
            dt.Load(conn.ExecuteReader("sp_GetDeviceCategories", commandType: CommandType.StoredProcedure));
            return dt;
        }

        public DataTable GetRoles()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            DataTable dt = new DataTable();
            dt.Load(conn.ExecuteReader("sp_GetRoles", commandType: CommandType.StoredProcedure));
            return dt;
        }

        public void AddDeviceCategory(string categoryName)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_AddDeviceCategory",
                new { name = categoryName },
                commandType: CommandType.StoredProcedure);
        }

        private static void SaveValue(SqlConnection conn, string key, string value)
        {
            conn.Execute(
                "sp_SaveSettingValue",
                new { key, val = value },
                commandType: CommandType.StoredProcedure);
        }

        private static string GetValue(SqlConnection conn, string key, string defaultValue)
        {
            return conn.ExecuteScalar<string>(
                "sp_GetSettingValue",
                new { key, defaultValue },
                commandType: CommandType.StoredProcedure) ?? defaultValue;
        }
    }
}
