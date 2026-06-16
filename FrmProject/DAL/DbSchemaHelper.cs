using System.Collections.Concurrent;
using System.Data;
using Microsoft.Data.SqlClient;

namespace FrmProject.DAL
{
    public static class DbSchemaHelper
    {
        private static readonly ConcurrentDictionary<string, bool> _columnCache = new();

        /// <summary>Clears the schema cache. Call when DB schema changes at runtime (rare).</summary>
        public static void ClearCache() => _columnCache.Clear();

        public static void ValidateRequiredSchema(SqlConnection conn)
        {
            ValidateLoginSecuritySchema(conn);
            ValidateAppSettingsSchema(conn);
            ValidateReturnRequestSchema(conn);
            ValidateBorrowWorkflowSchema(conn);
        }

        public static void ValidateLoginSecuritySchema(SqlConnection conn)
        {
            RequireColumns(conn, "Users", "FailedLoginCount", "LockoutUntil", "IsActive", "IsLocked");
        }

        public static void ValidateAppSettingsSchema(SqlConnection conn)
        {
            RequireTable(conn, "AppSettings");
            RequireColumns(conn, "AppSettings", "SettingKey", "SettingValue");
        }

        public static void ValidateReturnRequestSchema(SqlConnection conn)
        {
            RequireTable(conn, "ReturnRequests");
            RequireColumns(conn, "ReturnRequests", "ReturnRequestID", "TicketID", "RequestedAt", "Note", "CreatedAt");

            RequireTable(conn, "ReturnRequestDetails");
            RequireColumns(conn, "ReturnRequestDetails", "ReturnRequestDetailID", "ReturnRequestID", "DeviceID",
                "InstanceID", "BorrowQuantity", "ReturnQuantity", "Note");
        }

        public static void ValidateBorrowWorkflowSchema(SqlConnection conn)
        {
            RequireColumns(conn, "BorrowDetails", "ReturnedQuantity", "InstanceID");
            RequireColumns(conn, "DeviceInstances", "InstanceID", "DeviceID", "AssetCode", "Status", "Condition");
            RequireColumns(conn, "BorrowTickets", "Status", "Note", "ReturnNote");

            using var triggerCmd = new SqlCommand(
                "SELECT CASE WHEN OBJECT_ID(N'dbo.TR_BorrowDetails_RequireInstance', N'TR') IS NULL THEN 0 ELSE 1 END",
                conn);
            if (Convert.ToInt32(triggerCmd.ExecuteScalar()) != 1)
            {
                throw new InvalidOperationException(
                    "Database chưa có trigger dbo.TR_BorrowDetails_RequireInstance. Vui lòng chạy migration trước khi mở ứng dụng.");
            }
        }

        private static void RequireTable(SqlConnection conn, string tableName)
        {
            using var cmd = new SqlCommand(
                "SELECT CASE WHEN OBJECT_ID(@tableName, N'U') IS NULL THEN 0 ELSE 1 END",
                conn);
            cmd.Parameters.Add("@tableName", SqlDbType.NVarChar, 256).Value = $"dbo.{tableName}";

            if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                throw new InvalidOperationException($"Database thiếu bảng dbo.{tableName}. Vui lòng chạy migration trước khi mở ứng dụng.");
        }

        private static void RequireColumns(SqlConnection conn, string tableName, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (!HasColumn(conn, tableName, columnName))
                    throw new InvalidOperationException(
                        $"Database thiếu cột dbo.{tableName}.{columnName}. Vui lòng chạy migration trước khi mở ứng dụng.");
            }
        }

        public static bool HasColumn(SqlConnection conn, string tableName, string columnName, SqlTransaction? tran = null)
        {
            SqlConnectionStringBuilder csb = new(conn.ConnectionString);
            string key = $"{csb.DataSource}|{csb.InitialCatalog}|{tableName}.{columnName}";

            return _columnCache.GetOrAdd(key, _ =>
            {
                using var cmd = new SqlCommand(
                    "SELECT CASE WHEN COL_LENGTH(@tableName, @columnName) IS NULL THEN 0 ELSE 1 END", conn, tran);
                cmd.Parameters.AddWithValue("@tableName", tableName);
                cmd.Parameters.AddWithValue("@columnName", columnName);
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            });
        }

        /// <summary>Returns the raw columns needed for GROUP BY when using status CASE expression.</summary>
        public static string GetUserGroupByColumns(SqlConnection conn, string tableAlias = "u")
        {
            var cols = new List<string>();
            if (HasColumn(conn, "Users", "IsActive"))
                cols.Add($"{tableAlias}.IsActive");
            if (HasColumn(conn, "Users", "IsLocked"))
                cols.Add($"{tableAlias}.IsLocked");
            return string.Join(", ", cols);
        }

        public static string GetBorrowTicketDisplayExpression(SqlConnection conn, string tableAlias = "bt", SqlTransaction? tran = null)
        {
            return HasColumn(conn, "BorrowTickets", "TicketCode", tran)
                ? $"ISNULL({tableAlias}.TicketCode, 'PM' + CONVERT(VARCHAR, {tableAlias}.TicketID))"
                : $"'PM' + CONVERT(VARCHAR, {tableAlias}.TicketID)";
        }

        public static string GetBorrowTicketDisplayExpressionForCast(SqlConnection conn, string tableAlias = "bt", SqlTransaction? tran = null)
        {
            return HasColumn(conn, "BorrowTickets", "TicketCode", tran)
                ? $"ISNULL({tableAlias}.TicketCode, CAST({tableAlias}.TicketID AS VARCHAR))"
                : $"'PM' + CAST({tableAlias}.TicketID AS VARCHAR)";
        }

        public static string GetReturnDateColumnExpression(SqlConnection conn, string tableAlias = "bt", SqlTransaction? tran = null)
        {
            if (HasColumn(conn, "BorrowTickets", "ReturnDate", tran))
                return $"{tableAlias}.ReturnDate";

            if (HasColumn(conn, "BorrowTickets", "ActualReturnDate", tran))
                return $"{tableAlias}.ActualReturnDate";

            return "NULL";
        }

        public static string BuildLoginWhereClause(SqlConnection conn, string tableAlias = "u")
        {
            List<string> conditions = new List<string> { $"{tableAlias}.Username = @username" };
            conditions.AddRange(BuildEnabledUserConditions(conn, tableAlias));
            return string.Join(" AND ", conditions);
        }

        public static string BuildEnabledUserWhereClause(SqlConnection conn, string tableAlias = "u")
        {
            List<string> conditions = BuildEnabledUserConditions(conn, tableAlias);
            return conditions.Count == 0 ? "1 = 1" : string.Join(" AND ", conditions);
        }

        public static string GetUserStatusCaseExpression(SqlConnection conn, string tableAlias = "u")
        {
            bool hasIsActive = HasColumn(conn, "Users", "IsActive");
            bool hasIsLocked = HasColumn(conn, "Users", "IsLocked");

            if (hasIsActive && hasIsLocked)
            {
                return $@"CASE
                            WHEN ISNULL({tableAlias}.IsActive, 1) = 0 THEN N'Ngừng hoạt động'
                            WHEN ISNULL({tableAlias}.IsLocked, 0) = 1 THEN N'Bị khóa'
                            ELSE N'Hoạt động'
                         END";
            }

            if (hasIsActive)
                return $"CASE WHEN ISNULL({tableAlias}.IsActive, 1) = 1 THEN N'Hoạt động' ELSE N'Ngừng hoạt động' END";

            if (hasIsLocked)
                return $"CASE WHEN ISNULL({tableAlias}.IsLocked, 0) = 0 THEN N'Hoạt động' ELSE N'Bị khóa' END";

            return "N'Hoạt động'";
        }

        public static string BuildUserStatusFilterClause(SqlConnection conn, string selectedStatus, string tableAlias = "u")
        {
            bool hasIsActive = HasColumn(conn, "Users", "IsActive");
            bool hasIsLocked = HasColumn(conn, "Users", "IsLocked");

            if (string.IsNullOrWhiteSpace(selectedStatus) || selectedStatus.Contains("Tất cả"))
                return string.Empty;

            bool wantsActive = selectedStatus.Contains("Hoạt động");
            bool wantsInactive = selectedStatus.Contains("Ngừng");
            bool wantsLocked = selectedStatus.Contains("khóa", StringComparison.OrdinalIgnoreCase);

            if (wantsActive)
            {
                List<string> parts = new List<string>();
                if (hasIsActive)
                    parts.Add($"ISNULL({tableAlias}.IsActive, 1) = 1");
                if (hasIsLocked)
                    parts.Add($"ISNULL({tableAlias}.IsLocked, 0) = 0");
                return parts.Count == 0 ? string.Empty : string.Join(" AND ", parts);
            }

            if (wantsInactive && hasIsActive)
                return $"ISNULL({tableAlias}.IsActive, 1) = 0";

            if (wantsLocked && hasIsLocked)
                return $"ISNULL({tableAlias}.IsLocked, 0) = 1";

            return string.Empty;
        }

        public static string BuildActiveBorrowStatusCondition(string tableAlias = "")
        {
            string statusColumn = string.IsNullOrWhiteSpace(tableAlias)
                ? "Status"
                : $"{tableAlias}.Status";

            return $"{statusColumn} IN ('{BorrowTicketStatus.Borrowing}', '{BorrowTicketStatus.ReturnPending}')";
        }

        private static List<string> BuildEnabledUserConditions(SqlConnection conn, string tableAlias)
        {
            List<string> conditions = new List<string>();

            if (HasColumn(conn, "Users", "IsActive"))
                conditions.Add($"ISNULL({tableAlias}.IsActive, 1) = 1");

            if (HasColumn(conn, "Users", "IsLocked"))
                conditions.Add($"ISNULL({tableAlias}.IsLocked, 0) = 0");

            return conditions;
        }
    }
}

