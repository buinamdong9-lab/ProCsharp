using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    /// <summary>
    /// Renamed from UserDao → UserRepository for naming consistency.
    /// All methods are now static, matching other Repository classes.
    /// </summary>
    internal static class UserRepository
    {
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            string statusExpr = DbSchemaHelper.GetUserStatusCaseExpression(conn, "u");
            string groupByCols = DbSchemaHelper.GetUserGroupByColumns(conn, "u");
            string query = $@"
                SELECT 
                    ISNULL(u.UserCode, '')      AS [{Col.MaND}],
                    u.FullName                  AS [{Col.HoTen}],
                    ISNULL(u.UserCode, '')      AS [{Col.MaSo}],
                    ISNULL(u.Email, '')         AS [{Col.Email}],
                    ISNULL(u.Department, '')    AS [{Col.KhoaBoMon}],
                    ISNULL(r.RoleName, '')      AS [{Col.VaiTro}],
                    ISNULL(u.Phone, '')         AS [{Col.SoDienThoai}],
                    COUNT(bt.TicketID)          AS [{Col.DangMuon}],
                    {statusExpr}               AS [{Col.TrangThai}]
                FROM Users u
                LEFT JOIN Roles r ON u.RoleID = r.RoleID
                LEFT JOIN BorrowTickets bt ON u.UserID = bt.UserID 
                    AND {DbSchemaHelper.BuildActiveBorrowStatusCondition("bt")}
                GROUP BY u.UserID, u.UserCode, u.FullName, 
                         u.Email, u.Department, r.RoleName, 
                         u.Phone{(string.IsNullOrEmpty(groupByCols) ? "" : ", " + groupByCols)}
                ORDER BY u.FullName";

            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.Fill(dt);
            return dt;
        }

        public static UserIdentity? FindUserIdentity(string userCode, string username)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(
                @"SELECT TOP 1 UserID, Username
                  FROM Users
                  WHERE UserCode = @code OR Username = @username
                  ORDER BY CASE WHEN UserCode = @code THEN 0 ELSE 1 END", conn);
            cmd.Parameters.AddWithValue("@code", userCode);
            cmd.Parameters.AddWithValue("@username", username);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            return new UserIdentity
            {
                UserId = reader.GetInt32(0),
                Username = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
            };
        }

        public static void DeleteUser(int userId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(
                @"UPDATE Users
                  SET IsActive = 0,
                      IsLocked = 1
                  WHERE UserID = @id", conn);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        public static string GenerateUserCode()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string query = @"
                SELECT MAX(CAST(SUBSTRING(UserCode, 3, LEN(UserCode) - 2) AS INT)) 
                FROM Users 
                WHERE UserCode LIKE 'ND[0-9]%'";
            using SqlCommand cmd = new SqlCommand(query, conn);
            object? result = cmd.ExecuteScalar();
            int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
            return $"ND{nextId:D3}";
        }

        public static void SaveUser(UserEditModel user, bool isAdding)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            int roleId = GetRoleId(conn, user.RoleName);

            if (isAdding || user.UserId < 0)
            {
                using var cmd = new SqlCommand(BuildInsertUserSql(conn), conn);
                FillCommonUserParameters(cmd, user, roleId);
                if (DbSchemaHelper.HasColumn(conn, "Users", "IsActive"))
                    cmd.Parameters.AddWithValue("@active", user.IsActive);
                if (DbSchemaHelper.HasColumn(conn, "Users", "IsLocked"))
                    cmd.Parameters.AddWithValue("@locked", user.IsLocked);
                if (DbSchemaHelper.HasColumn(conn, "Users", "CreatedAt"))
                    cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@password", user.PasswordHash ?? string.Empty);
                cmd.ExecuteNonQuery();
                return;
            }

            using (var cmd = new SqlCommand(BuildUpdateUserSql(conn), conn))
            {
                FillCommonUserParameters(cmd, user, roleId);
                if (DbSchemaHelper.HasColumn(conn, "Users", "IsActive"))
                    cmd.Parameters.AddWithValue("@active", user.IsActive);
                if (DbSchemaHelper.HasColumn(conn, "Users", "IsLocked"))
                    cmd.Parameters.AddWithValue("@locked", user.IsLocked);
                cmd.Parameters.AddWithValue("@id", user.UserId);
                cmd.ExecuteNonQuery();
            }

            // Fix: reuse the same connection instead of opening a new one
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                ResetPassword(conn, user.UserId, user.PasswordHash);
        }

        public static void ResetPassword(int userId, string passwordHash)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            ResetPassword(conn, userId, passwordHash);
        }

        public static DataTable SearchUsers(string keyword, string role, string status)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            string statusExpr = DbSchemaHelper.GetUserStatusCaseExpression(conn, "u");
            string query = $@"
                SELECT 
                    ISNULL(u.UserCode, '') AS [{Col.MaND}],
                    u.FullName AS [{Col.HoTen}],
                    ISNULL(u.UserCode, '') AS [{Col.MaSo}],
                    ISNULL(u.Email, '') AS [{Col.Email}],
                    ISNULL(u.Department, '') AS [{Col.KhoaBoMon}],
                    ISNULL(r.RoleName, '') AS [{Col.VaiTro}],
                    ISNULL(u.Phone, '') AS [{Col.SoDienThoai}],
                    COUNT(bt.TicketID) AS [{Col.DangMuon}],
                    {statusExpr} AS [{Col.TrangThai}]
                FROM Users u
                LEFT JOIN Roles r ON u.RoleID = r.RoleID
                LEFT JOIN BorrowTickets bt ON u.UserID = bt.UserID AND " + DbSchemaHelper.BuildActiveBorrowStatusCondition("bt") + @"
                WHERE 1 = 1";

            if (!string.IsNullOrEmpty(keyword))
                query += " AND (u.FullName LIKE @kw OR ISNULL(u.UserCode, '') LIKE @kw OR ISNULL(u.Email, '') LIKE @kw OR ISNULL(u.Username, '') LIKE @kw)";

            if (!string.IsNullOrEmpty(role))
                query += " AND ISNULL(r.RoleName, '') LIKE @role";

            string statusFilterClause = DbSchemaHelper.BuildUserStatusFilterClause(conn, status, "u");
            if (!string.IsNullOrWhiteSpace(statusFilterClause))
                query += " AND " + statusFilterClause;

            query += $@" GROUP BY u.UserID, u.UserCode, u.FullName, 
                              u.Email, u.Department, r.RoleName, u.Phone, {statusExpr}
                         ORDER BY u.FullName";

            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            if (!string.IsNullOrEmpty(keyword))
                da.SelectCommand.Parameters.AddWithValue("@kw", $"%{keyword}%");
            if (!string.IsNullOrEmpty(role))
                da.SelectCommand.Parameters.AddWithValue("@role", $"%{role}%");

            DataTable dt = new();
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Lấy danh sách người dùng theo trang (server-side pagination).
        /// pageNumber bắt đầu từ 1. pageSize = số dòng mỗi trang.
        /// </summary>
        public static DataTable GetUsersPaged(
            int pageNumber, int pageSize,
            string keyword = "", string role = "", string status = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            string statusExpr = DbSchemaHelper.GetUserStatusCaseExpression(conn, "u");
            int offset = (pageNumber - 1) * pageSize;

            string where = BuildWhereClause(keyword, role,
                DbSchemaHelper.BuildUserStatusFilterClause(conn, status, "u"));

            string query = $@"
                SELECT 
                    ISNULL(u.UserCode, '')      AS [{Col.MaND}],
                    u.FullName                  AS [{Col.HoTen}],
                    ISNULL(u.UserCode, '')      AS [{Col.MaSo}],
                    ISNULL(u.Email, '')         AS [{Col.Email}],
                    ISNULL(u.Department, '')    AS [{Col.KhoaBoMon}],
                    ISNULL(r.RoleName, '')      AS [{Col.VaiTro}],
                    ISNULL(u.Phone, '')         AS [{Col.SoDienThoai}],
                    COUNT(bt.TicketID)          AS [{Col.DangMuon}],
                    {statusExpr}               AS [{Col.TrangThai}]
                FROM Users u
                LEFT JOIN Roles r ON u.RoleID = r.RoleID
                LEFT JOIN BorrowTickets bt ON u.UserID = bt.UserID
                    AND {DbSchemaHelper.BuildActiveBorrowStatusCondition("bt")}
                {where}
                GROUP BY u.UserID, u.UserCode, u.FullName,
                         u.Email, u.Department, r.RoleName, u.Phone, {statusExpr}
                ORDER BY u.FullName
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Offset", offset);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            AddFilterParameters(cmd, keyword, role);

            DataTable dt = new();
            using SqlDataAdapter da = new(cmd);
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Đếm tổng số người dùng thỏa điều kiện lọc — dùng để tính tổng số trang.
        /// </summary>
        public static int GetTotalUsersCount(
            string keyword = "", string role = "", string status = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            string statusClause = DbSchemaHelper.BuildUserStatusFilterClause(conn, status, "u");
            string where = BuildWhereClause(keyword, role, statusClause);

            string query = $@"
                SELECT COUNT(DISTINCT u.UserID)
                FROM Users u
                LEFT JOIN Roles r ON u.RoleID = r.RoleID
                {where}";

            using SqlCommand cmd = new(query, conn);
            AddFilterParameters(cmd, keyword, role);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // ═══════ Private helpers ═══════

        /// <summary>Xây dựng mệnh đề WHERE dùng chung cho GetUsersPaged và GetTotalUsersCount.</summary>
        private static string BuildWhereClause(string keyword, string role, string statusClause)
        {
            var clauses = new List<string> { "1=1" };
            if (!string.IsNullOrEmpty(keyword))
                clauses.Add("(u.FullName LIKE @kw OR ISNULL(u.UserCode,'') LIKE @kw OR ISNULL(u.Email,'') LIKE @kw OR ISNULL(u.Username,'') LIKE @kw)");
            if (!string.IsNullOrEmpty(role))
                clauses.Add("ISNULL(r.RoleName,'') LIKE @role");
            if (!string.IsNullOrWhiteSpace(statusClause))
                clauses.Add(statusClause);
            return "WHERE " + string.Join(" AND ", clauses);
        }

        /// <summary>Thêm tham số lọc keyword và role vào SqlCommand.</summary>
        private static void AddFilterParameters(SqlCommand cmd, string keyword, string role)
        {
            if (!string.IsNullOrEmpty(keyword))
                cmd.Parameters.AddWithValue("@kw", $"%{keyword}%");
            if (!string.IsNullOrEmpty(role))
                cmd.Parameters.AddWithValue("@role", $"%{role}%");
        }

        /// <summary>
        /// Internal overload that reuses an existing open connection (fixes double-connection issue in SaveUser).
        /// </summary>
        private static void ResetPassword(SqlConnection conn, int userId, string passwordHash)
        {
            using var cmd = new SqlCommand(
                "UPDATE Users SET PasswordHash = @hash WHERE UserID = @id", conn);
            cmd.Parameters.AddWithValue("@hash", passwordHash);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        private static int GetRoleId(SqlConnection conn, string roleName)
        {
            using var cmdRole = new SqlCommand(
                "SELECT TOP 1 RoleID FROM Roles WHERE RoleName = @name", conn);
            cmdRole.Parameters.AddWithValue("@name", roleName);
            object? result = cmdRole.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 1;
        }

        private static void FillCommonUserParameters(SqlCommand cmd, UserEditModel user, int roleId)
        {
            cmd.Parameters.AddWithValue("@code", user.UserCode);
            cmd.Parameters.AddWithValue("@name", user.FullName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@dept", user.Department);
            cmd.Parameters.AddWithValue("@phone", user.Phone);
            cmd.Parameters.AddWithValue("@roleID", roleId);
            cmd.Parameters.AddWithValue("@username", user.Username);
        }

        private static string BuildInsertUserSql(SqlConnection conn)
        {
            List<string> columns = new()
            {
                "UserCode", "FullName", "Email", "Department", "Phone", "RoleID", "Username", "PasswordHash"
            };

            List<string> values = new()
            {
                "@code", "@name", "@email", "@dept", "@phone", "@roleID", "@username", "@password"
            };

            if (DbSchemaHelper.HasColumn(conn, "Users", "IsActive"))
            {
                columns.Add("IsActive");
                values.Add("@active");
            }

            if (DbSchemaHelper.HasColumn(conn, "Users", "IsLocked"))
            {
                columns.Add("IsLocked");
                values.Add("@locked");
            }

            if (DbSchemaHelper.HasColumn(conn, "Users", "CreatedAt"))
            {
                columns.Add("CreatedAt");
                values.Add("@createdAt");
            }

            return $"INSERT INTO Users ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
        }

        private static string BuildUpdateUserSql(SqlConnection conn)
        {
            List<string> updates = new()
            {
                "UserCode = @code",
                "FullName = @name",
                "Email = @email",
                "Department = @dept",
                "Phone = @phone",
                "RoleID = @roleID",
                "Username = @username"
            };

            if (DbSchemaHelper.HasColumn(conn, "Users", "IsActive"))
                updates.Add("IsActive = @active");

            if (DbSchemaHelper.HasColumn(conn, "Users", "IsLocked"))
                updates.Add("IsLocked = @locked");

            return "UPDATE Users SET " + string.Join(", ", updates) + " WHERE UserID = @id";
        }
    }
}

