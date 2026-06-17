using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using FrmProject.Models;

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

        public List<DeviceCategoryStatsModel> GetDeviceCategories()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query("sp_GetDeviceCategories", commandType: CommandType.StoredProcedure);
            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new DeviceCategoryStatsModel
                {
                    CategoryID = Convert.ToInt32(dict["Mã loại"]),
                    CategoryName = dict["Tên loại"]?.ToString() ?? "",
                    DeviceCount = Convert.ToInt32(dict["Số thiết bị"]),
                    Action = dict["Thao tác"]?.ToString() ?? ""
                };
            }).ToList();
        }

        public List<RoleStatsModel> GetRoles()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query("sp_GetRoles", commandType: CommandType.StoredProcedure);
            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new RoleStatsModel
                {
                    RoleID = Convert.ToInt32(dict["Mã vai trò"]),
                    RoleName = dict["Tên vai trò"]?.ToString() ?? "",
                    UserCount = Convert.ToInt32(dict["Số người dùng"])
                };
            }).ToList();
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
