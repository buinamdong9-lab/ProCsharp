using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    internal static class SettingsRepository
    {
        public static void EnsureAppSettingsTable()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlCommand cmd = new SqlCommand(@"
                IF OBJECT_ID('dbo.AppSettings', 'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.AppSettings (
                        SettingKey   NVARCHAR(100) NOT NULL PRIMARY KEY,
                        SettingValue NVARCHAR(500) NULL
                    );
                END", conn);
            cmd.ExecuteNonQuery();
        }

        public static string GetValue(string key, string defaultValue = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            return GetValue(conn, key, defaultValue);
        }

        public static int GetIntValue(string key, int defaultValue)
        {
            string value = GetValue(key, defaultValue.ToString());
            return int.TryParse(value, out int parsed) ? parsed : defaultValue;
        }

        public static bool GetYesNoValue(string key, bool defaultValue)
        {
            string value = GetValue(key, defaultValue ? "Có" : "Không");
            if (value.Equals("Có", StringComparison.OrdinalIgnoreCase))
                return true;
            if (value.Equals("Không", StringComparison.OrdinalIgnoreCase))
                return false;

            return defaultValue;
        }

        public static void SaveValues(IReadOnlyDictionary<string, string> settings)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            foreach (var setting in settings)
                SaveValue(conn, setting.Key, setting.Value);
        }

        public static DataTable GetDeviceCategories()
        {
            const string query = @"
                SELECT dc.CategoryID AS [Mã loại], 
                       dc.CategoryName AS [Tên loại],
                       COUNT(d.DeviceID) AS [Số thiết bị],
                       '' AS [Thao tác]
                FROM DeviceCategories dc
                LEFT JOIN Devices d ON d.CategoryID = dc.CategoryID
                GROUP BY dc.CategoryID, dc.CategoryName
                ORDER BY dc.CategoryName";

            using SqlConnection conn = DbHelper.GetConnection();
            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            return Fill(da);
        }

        public static DataTable GetRoles()
        {
            const string query = @"
                SELECT r.RoleID AS [Mã vai trò],
                       r.RoleName AS [Tên vai trò],
                       COUNT(u.UserID) AS [Số người dùng]
                FROM Roles r
                LEFT JOIN Users u ON u.RoleID = r.RoleID
                GROUP BY r.RoleID, r.RoleName
                ORDER BY r.RoleName";

            using SqlConnection conn = DbHelper.GetConnection();
            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            return Fill(da);
        }

        public static void AddDeviceCategory(string categoryName)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlCommand cmd = new SqlCommand("INSERT INTO DeviceCategories (CategoryName) VALUES (@name)", conn);
            cmd.Parameters.AddWithValue("@name", categoryName);
            cmd.ExecuteNonQuery();
        }

        private static void SaveValue(SqlConnection conn, string key, string value)
        {
            using SqlCommand cmd = new SqlCommand(@"
                MERGE dbo.AppSettings AS t
                USING (SELECT @key AS SettingKey) AS s
                ON t.SettingKey = s.SettingKey
                WHEN MATCHED THEN UPDATE SET SettingValue = @val
                WHEN NOT MATCHED THEN INSERT (SettingKey, SettingValue) VALUES (@key, @val);", conn);
            cmd.Parameters.AddWithValue("@key", key);
            cmd.Parameters.AddWithValue("@val", value ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        private static string GetValue(SqlConnection conn, string key, string defaultValue)
        {
            using SqlCommand cmd = new SqlCommand("SELECT SettingValue FROM dbo.AppSettings WHERE SettingKey = @key", conn);
            cmd.Parameters.AddWithValue("@key", key);
            object? result = cmd.ExecuteScalar();
            return result == null || result == DBNull.Value ? defaultValue : result.ToString()!;
        }

        private static DataTable Fill(SqlDataAdapter da)
        {
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}

