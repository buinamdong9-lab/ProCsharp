using Microsoft.Data.SqlClient;

namespace FrmProject.DAL
{
    internal static class AuthRepository
    {
        private const int MaxAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(5);

        public static LoginUserRecord? GetLoginUser(string username)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            EnsureLoginSecurityColumns(conn);

            string failedCountExpr = DbSchemaHelper.HasColumn(conn, "Users", "FailedLoginCount")
                ? "ISNULL(u.FailedLoginCount, 0)"
                : "0";
            string lockoutExpr = DbSchemaHelper.HasColumn(conn, "Users", "LockoutUntil")
                ? "u.LockoutUntil"
                : "CAST(NULL AS DATETIME)";
            string isActiveExpr = DbSchemaHelper.HasColumn(conn, "Users", "IsActive")
                ? "ISNULL(u.IsActive, 1)"
                : "1";
            string isLockedExpr = DbSchemaHelper.HasColumn(conn, "Users", "IsLocked")
                ? "ISNULL(u.IsLocked, 0)"
                : "0";

            string query = $@"
                SELECT u.UserID,
                       u.FullName,
                       ISNULL(r.RoleName, 'User'),
                       ISNULL(u.PasswordHash, ''),
                       {failedCountExpr} AS FailedLoginCount,
                       {lockoutExpr} AS LockoutUntil,
                       {isActiveExpr} AS IsActive,
                       {isLockedExpr} AS IsLocked
                FROM Users u
                LEFT JOIN Roles r ON u.RoleID = r.RoleID
                WHERE u.Username = @username";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@username", username);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            return new LoginUserRecord
            {
                UserID = reader.GetInt32(0),
                FullName = reader.IsDBNull(1) ? username : reader.GetString(1),
                RoleName = reader.IsDBNull(2) ? "User" : reader.GetString(2),
                PasswordHash = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                FailedLoginCount = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                LockoutUntil = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                IsActive = !reader.IsDBNull(6) && Convert.ToInt32(reader.GetValue(6)) == 1,
                IsLocked = !reader.IsDBNull(7) && Convert.ToInt32(reader.GetValue(7)) == 1
            };
        }

        public static void ResetFailedLoginState(int userId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            EnsureLoginSecurityColumns(conn);
            using var cmd = new SqlCommand(
                "UPDATE Users SET FailedLoginCount = 0, LockoutUntil = NULL WHERE UserID = @userId", conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.ExecuteNonQuery();
        }

        public static LoginAttemptResult RegisterFailedLogin(int userId, int failedCount, DateTime? lockoutUntil)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            EnsureLoginSecurityColumns(conn);

            DateTime now = DateTime.Now;
            if (lockoutUntil.HasValue && lockoutUntil.Value <= now)
                failedCount = 0;

            int nextFailedCount = failedCount + 1;
            DateTime? nextLockout = null;
            if (nextFailedCount >= MaxAttempts)
            {
                nextLockout = now.Add(LockoutDuration);
                nextFailedCount = 0;
            }

            using var cmd = new SqlCommand(
                "UPDATE Users SET FailedLoginCount = @failedCount, LockoutUntil = @lockoutUntil WHERE UserID = @userId", conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@failedCount", nextFailedCount);
            cmd.Parameters.AddWithValue("@lockoutUntil", nextLockout ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();

            return new LoginAttemptResult
            {
                AttemptsLeft = nextLockout.HasValue ? 0 : Math.Max(0, MaxAttempts - nextFailedCount),
                LockoutUntil = nextLockout
            };
        }

        private static void EnsureLoginSecurityColumns(SqlConnection conn)
        {
            using var cmd = new SqlCommand(@"
                IF COL_LENGTH('dbo.Users', 'FailedLoginCount') IS NULL
                BEGIN
                    ALTER TABLE dbo.Users
                    ADD FailedLoginCount INT NOT NULL
                        CONSTRAINT DF_Users_FailedLoginCount DEFAULT (0);
                END;

                IF COL_LENGTH('dbo.Users', 'LockoutUntil') IS NULL
                BEGIN
                    ALTER TABLE dbo.Users
                    ADD LockoutUntil DATETIME NULL;
                END;", conn);
            cmd.ExecuteNonQuery();
            DbSchemaHelper.ClearCache();
        }
    }
}

