using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace FrmProject.DAL
{
    public class AuthRepository : IAuthRepository
    {
        private const int MaxAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(5);

        public LoginUserRecord? GetLoginUser(string username)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            DbSchemaHelper.ValidateLoginSecuritySchema(conn);

            return conn.QueryFirstOrDefault<LoginUserRecord>(
                "sp_GetLoginUser",
                new { username },
                commandType: CommandType.StoredProcedure);
        }

        public void ResetFailedLoginState(int userId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            DbSchemaHelper.ValidateLoginSecuritySchema(conn);

            conn.Execute("sp_ResetFailedLoginState", new { userId }, commandType: CommandType.StoredProcedure);
        }

        public LoginAttemptResult RegisterFailedLogin(int userId, int failedCount, DateTime? lockoutUntil)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            DbSchemaHelper.ValidateLoginSecuritySchema(conn);

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

            conn.Execute(
                "sp_RegisterFailedLogin",
                new { userId, failedCount = nextFailedCount, lockoutUntil = nextLockout },
                commandType: CommandType.StoredProcedure);

            return new LoginAttemptResult
            {
                AttemptsLeft = nextLockout.HasValue ? 0 : Math.Max(0, MaxAttempts - nextFailedCount),
                LockoutUntil = nextLockout
            };
        }

    }
}

