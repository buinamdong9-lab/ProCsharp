using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class UserRepository : IUserRepository
    {
        public List<UserDisplayModel> GetAllUsers()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query("sp_GetAllUsers", commandType: CommandType.StoredProcedure);
            return MapRowsToUsers(rows);
        }

        public UserIdentity? FindUserIdentity(string userCode, string username)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.QueryFirstOrDefault<UserIdentity>(
                "sp_FindUserIdentity",
                new { code = userCode, username },
                commandType: CommandType.StoredProcedure);
        }

        public void DeleteUser(int userId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute("sp_DeleteUser", new { id = userId }, commandType: CommandType.StoredProcedure);
        }

        public string GenerateUserCode()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            object? result = conn.ExecuteScalar("sp_GenerateUserCode", commandType: CommandType.StoredProcedure);
            int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
            return $"ND{nextId:D3}";
        }

        public void SaveUser(UserEditModel user, bool isAdding)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            int roleId = GetRoleId(conn, user.RoleName);

            conn.Execute(
                "sp_SaveUser",
                new
                {
                    userId = user.UserId,
                    code = user.UserCode,
                    name = user.FullName,
                    email = user.Email,
                    dept = user.Department,
                    phone = user.Phone,
                    roleID = roleId,
                    username = user.Username,
                    password = string.IsNullOrWhiteSpace(user.PasswordHash) ? null : user.PasswordHash,
                    active = user.IsActive,
                    locked = user.IsLocked,
                    isAdding
                },
                commandType: CommandType.StoredProcedure);
        }

        public void ResetPassword(int userId, string passwordHash)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_ResetPassword",
                new { id = userId, hash = passwordHash },
                commandType: CommandType.StoredProcedure);
        }

        public List<UserDisplayModel> SearchUsers(string keyword, string role, string status)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_SearchUsers",
                new
                {
                    keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword,
                    role = string.IsNullOrWhiteSpace(role) ? null : role,
                    status = string.IsNullOrWhiteSpace(status) ? null : status
                },
                commandType: CommandType.StoredProcedure);
            return MapRowsToUsers(rows);
        }

        public List<UserDisplayModel> GetUsersPaged(
            int pageNumber, int pageSize,
            string keyword = "", string role = "", string status = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_GetUsersPaged",
                new
                {
                    pageNumber,
                    pageSize,
                    keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword,
                    role = string.IsNullOrWhiteSpace(role) ? null : role,
                    status = string.IsNullOrWhiteSpace(status) ? null : status
                },
                commandType: CommandType.StoredProcedure);
            return MapRowsToUsers(rows);
        }

        public int GetTotalUsersCount(
            string keyword = "", string role = "", string status = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.ExecuteScalar<int>(
                "sp_GetTotalUsersCount",
                new
                {
                    keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword,
                    role = string.IsNullOrWhiteSpace(role) ? null : role,
                    status = string.IsNullOrWhiteSpace(status) ? null : status
                },
                commandType: CommandType.StoredProcedure);
        }

        // ═══════ Private helpers ═══════

        private static int GetRoleId(SqlConnection conn, string roleName)
        {
            object? result = conn.ExecuteScalar(
                "sp_GetRoleIdByName",
                new { name = roleName },
                commandType: CommandType.StoredProcedure);
            return result != null ? Convert.ToInt32(result) : 1;
        }

        private static List<UserDisplayModel> MapRowsToUsers(IEnumerable<dynamic> rows)
        {
            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new UserDisplayModel
                {
                    UserCode = dict["Mã ND"]?.ToString() ?? (dict.TryGetValue("Mã số", out var ms) ? ms?.ToString() ?? "" : ""),
                    FullName = dict["Họ tên"]?.ToString() ?? "",
                    Email = dict["Email"]?.ToString() ?? "",
                    Department = dict["Khoa/Bộ Môn"]?.ToString() ?? "",
                    Role = dict["Vai trò"]?.ToString() ?? "",
                    Phone = dict["Số điện thoại"]?.ToString() ?? "",
                    BorrowingCount = Convert.ToInt32(dict["Đang mượn"]),
                    Status = dict["Trạng thái"]?.ToString() ?? ""
                };
            }).ToList();
        }
    }
}
