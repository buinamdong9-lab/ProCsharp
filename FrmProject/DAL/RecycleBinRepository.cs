using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    internal static class RecycleBinRepository
    {
        public static DataTable Load(string itemType, string keyword)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            DataTable result = CreateResultTable();
            if (itemType == "Tất cả" || itemType == "Thiết bị")
                AppendDeletedDevices(conn, result, keyword);
            if (itemType == "Tất cả" || itemType == "Mã cá thể")
                AppendDeletedInstances(conn, result, keyword);
            if (itemType == "Tất cả" || itemType == "Phòng học")
                AppendDeletedRooms(conn, result, keyword);
            if (itemType == "Tất cả" || itemType == "Người dùng")
                AppendDeletedUsers(conn, result, keyword);

            return result;
        }

        public static void Restore(string itemType, int id)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                switch (itemType)
                {
                    case "Thiết bị":
                        RestoreDevice(conn, tran, id);
                        break;
                    case "Mã cá thể":
                        RestoreInstance(conn, tran, id);
                        break;
                    case "Phòng học":
                        Execute(conn, tran, "UPDATE Rooms SET Status = @status WHERE RoomID = @id",
                            ("@status", RoomStatus.Active), ("@id", id));
                        break;
                    case "Người dùng":
                        RestoreUser(conn, tran, id);
                        break;
                    default:
                        throw new InvalidOperationException("Loại dữ liệu không hợp lệ.");
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static void DeleteForever(string itemType, int id)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                switch (itemType)
                {
                    case "Thiết bị":
                        DeleteDeviceForever(conn, tran, id);
                        break;
                    case "Mã cá thể":
                        DeleteInstanceForever(conn, tran, id);
                        break;
                    case "Phòng học":
                        DeleteRoomForever(conn, tran, id);
                        break;
                    case "Người dùng":
                        DeleteUserForever(conn, tran, id);
                        break;
                    default:
                        throw new InvalidOperationException("Loại dữ liệu không hợp lệ.");
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private static DataTable CreateResultTable()
        {
            DataTable table = new();
            table.Columns.Add("Loại", typeof(string));
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Mã", typeof(string));
            table.Columns.Add("Tên", typeof(string));
            table.Columns.Add("Trạng thái", typeof(string));
            table.Columns.Add("Ghi chú", typeof(string));
            return table;
        }

        private static void AppendDeletedDevices(SqlConnection conn, DataTable result, string keyword)
        {
            using SqlCommand cmd = new(
                @"SELECT DeviceID, ISNULL(DeviceCode, ''), DeviceName, Status, ISNULL(Note, '')
                  FROM Devices
                  WHERE Status = @status
                    AND (@kw = '' OR DeviceName LIKE @like OR ISNULL(DeviceCode, '') LIKE @like OR ISNULL(Note, '') LIKE @like)
                  ORDER BY DeviceName", conn);
            cmd.Parameters.AddWithValue("@status", DeviceStatus.Retired);
            AddKeywordParameters(cmd, keyword);
            AppendRows(result, "Thiết bị", cmd);
        }

        private static void AppendDeletedInstances(SqlConnection conn, DataTable result, string keyword)
        {
            using SqlCommand cmd = new(
                @"SELECT di.InstanceID, ISNULL(di.AssetCode, ''), d.DeviceName, di.Status, ISNULL(di.Note, '')
                  FROM DeviceInstances di
                  JOIN Devices d ON d.DeviceID = di.DeviceID
                  WHERE di.Status = @status
                    AND (@kw = '' OR di.AssetCode LIKE @like OR d.DeviceName LIKE @like OR ISNULL(di.Note, '') LIKE @like)
                  ORDER BY d.DeviceName, di.AssetCode", conn);
            cmd.Parameters.AddWithValue("@status", DeviceStatus.Retired);
            AddKeywordParameters(cmd, keyword);
            AppendRows(result, "Mã cá thể", cmd);
        }

        private static void AppendDeletedRooms(SqlConnection conn, DataTable result, string keyword)
        {
            using SqlCommand cmd = new(
                @"SELECT RoomID, ISNULL(RoomCode, ''), RoomName, Status, ISNULL(Note, '')
                  FROM Rooms
                  WHERE Status = @status
                    AND (@kw = '' OR RoomCode LIKE @like OR RoomName LIKE @like OR ISNULL(Note, '') LIKE @like)
                  ORDER BY RoomName", conn);
            cmd.Parameters.AddWithValue("@status", RoomStatus.Retired);
            AddKeywordParameters(cmd, keyword);
            AppendRows(result, "Phòng học", cmd);
        }

        private static void AppendDeletedUsers(SqlConnection conn, DataTable result, string keyword)
        {
            if (!DbSchemaHelper.HasColumn(conn, "Users", "IsActive"))
                return;

            using SqlCommand cmd = new(
                @"SELECT UserID, ISNULL(UserCode, ''), FullName, N'Ngừng hoạt động', ISNULL(Email, '')
                  FROM Users
                  WHERE ISNULL(IsActive, 1) = 0
                    AND (@kw = '' OR FullName LIKE @like OR ISNULL(UserCode, '') LIKE @like OR ISNULL(Email, '') LIKE @like)
                  ORDER BY FullName", conn);
            AddKeywordParameters(cmd, keyword);
            AppendRows(result, "Người dùng", cmd);
        }

        private static void AppendRows(DataTable result, string type, SqlCommand cmd)
        {
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Rows.Add(
                    type,
                    reader.GetInt32(0),
                    reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    reader.IsDBNull(4) ? string.Empty : reader.GetString(4));
            }
        }

        private static void AddKeywordParameters(SqlCommand cmd, string keyword)
        {
            string value = keyword.Trim();
            cmd.Parameters.AddWithValue("@kw", value);
            cmd.Parameters.AddWithValue("@like", "%" + value + "%");
        }

        private static void RestoreDevice(SqlConnection conn, SqlTransaction tran, int id)
        {
            Execute(conn, tran, "UPDATE Devices SET Status = @status WHERE DeviceID = @id",
                ("@status", DeviceCondition.Good), ("@id", id));
            Execute(conn, tran,
                @"UPDATE DeviceInstances
                  SET Status = @availableStatus
                  WHERE DeviceID = @id
                    AND Status = @retiredStatus
                    AND ISNULL(Note, '') LIKE N'%Xóa mềm theo thiết bị%'",
                ("@availableStatus", DeviceStatus.Available),
                ("@retiredStatus", DeviceStatus.Retired),
                ("@id", id));
            RecalculateDeviceQuantities(conn, tran, id);
        }

        private static void RestoreInstance(SqlConnection conn, SqlTransaction tran, int id)
        {
            int deviceId = GetInt(conn, tran, "SELECT DeviceID FROM DeviceInstances WHERE InstanceID = @id", id);
            string deviceStatus = GetString(conn, tran, "SELECT ISNULL(Status, '') FROM Devices WHERE DeviceID = @id", deviceId);
            if (deviceStatus == DeviceStatus.Retired)
                throw new InvalidOperationException("Thiết bị cha đang ở thùng rác. Hãy khôi phục thiết bị trước.");

            Execute(conn, tran, "UPDATE DeviceInstances SET Status = @status WHERE InstanceID = @id",
                ("@status", DeviceStatus.Available), ("@id", id));
            RecalculateDeviceQuantities(conn, tran, deviceId);
        }

        private static void RestoreUser(SqlConnection conn, SqlTransaction tran, int id)
        {
            List<(string, object)> parameters = new() { ("@id", id) };
            string sql = "UPDATE Users SET IsActive = 1";
            if (DbSchemaHelper.HasColumn(conn, "Users", "IsLocked", tran))
                sql += ", IsLocked = 0";
            sql += " WHERE UserID = @id";
            Execute(conn, tran, sql, parameters.ToArray());
        }

        private static void DeleteDeviceForever(SqlConnection conn, SqlTransaction tran, int id)
        {
            EnsureNoRows(conn, tran, "SELECT COUNT(*) FROM BorrowDetails WHERE DeviceID = @id", id,
                "Không thể xóa vĩnh viễn thiết bị vì còn lịch sử phiếu mượn.");
            Execute(conn, tran, "DELETE FROM DeviceInstances WHERE DeviceID = @id", ("@id", id));
            Execute(conn, tran, "DELETE FROM Devices WHERE DeviceID = @id AND Status = @status", ("@id", id), ("@status", DeviceStatus.Retired));
        }

        private static void DeleteInstanceForever(SqlConnection conn, SqlTransaction tran, int id)
        {
            EnsureNoRows(conn, tran, "SELECT COUNT(*) FROM BorrowDetails WHERE InstanceID = @id", id,
                "Không thể xóa vĩnh viễn mã cá thể vì còn lịch sử phiếu mượn.");
            int deviceId = GetInt(conn, tran, "SELECT DeviceID FROM DeviceInstances WHERE InstanceID = @id", id);
            Execute(conn, tran, "DELETE FROM DeviceInstances WHERE InstanceID = @id AND Status = @status", ("@id", id), ("@status", DeviceStatus.Retired));
            RecalculateDeviceQuantities(conn, tran, deviceId);
        }

        private static void DeleteRoomForever(SqlConnection conn, SqlTransaction tran, int id)
        {
            EnsureNoRows(conn, tran, "SELECT COUNT(*) FROM Devices WHERE RoomID = @id", id,
                "Không thể xóa vĩnh viễn phòng vì vẫn còn thiết bị gắn với phòng này.");
            Execute(conn, tran, "DELETE FROM Rooms WHERE RoomID = @id AND Status = @status", ("@id", id), ("@status", RoomStatus.Retired));
        }

        private static void DeleteUserForever(SqlConnection conn, SqlTransaction tran, int id)
        {
            List<string> referenceColumns = new() { "UserID" };
            if (DbSchemaHelper.HasColumn(conn, "BorrowTickets", "CreatedBy", tran))
                referenceColumns.Add("CreatedBy");
            if (DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnedBy", tran))
                referenceColumns.Add("ReturnedBy");

            string referenceWhere = string.Join(" OR ", referenceColumns.Select(column => $"{column} = @id"));
            EnsureNoRows(conn, tran, $"SELECT COUNT(*) FROM BorrowTickets WHERE {referenceWhere}", id,
                "Không thể xóa vĩnh viễn người dùng vì còn lịch sử phiếu mượn/trả.");
            Execute(conn, tran, "DELETE FROM Users WHERE UserID = @id AND ISNULL(IsActive, 1) = 0", ("@id", id));
        }

        private static void RecalculateDeviceQuantities(SqlConnection conn, SqlTransaction tran, int deviceId)
        {
            Execute(conn, tran,
                @"UPDATE Devices
                  SET TotalQuantity = (SELECT COUNT(*) FROM DeviceInstances WHERE DeviceID = @id AND Status <> @retiredStatus),
                      AvailableQuantity = (
                          SELECT COUNT(*)
                          FROM DeviceInstances
                          WHERE DeviceID = @id
                            AND Status = @availableStatus
                            AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
                      )
                  WHERE DeviceID = @id",
                ("@id", deviceId),
                ("@retiredStatus", DeviceStatus.Retired),
                ("@availableStatus", DeviceStatus.Available),
                ("@goodCondition", DeviceCondition.Good));
        }

        private static void EnsureNoRows(SqlConnection conn, SqlTransaction tran, string sql, int id, string message)
        {
            using SqlCommand cmd = new(sql, conn, tran);
            cmd.Parameters.AddWithValue("@id", id);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                throw new InvalidOperationException(message);
        }

        private static int GetInt(SqlConnection conn, SqlTransaction tran, string sql, int id)
        {
            using SqlCommand cmd = new(sql, conn, tran);
            cmd.Parameters.AddWithValue("@id", id);
            object? value = cmd.ExecuteScalar();
            if (value == null || value == DBNull.Value)
                throw new InvalidOperationException("Không tìm thấy dữ liệu cần xử lý.");
            return Convert.ToInt32(value);
        }

        private static string GetString(SqlConnection conn, SqlTransaction tran, string sql, int id)
        {
            using SqlCommand cmd = new(sql, conn, tran);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteScalar()?.ToString() ?? string.Empty;
        }

        private static void Execute(SqlConnection conn, SqlTransaction tran, string sql, params (string Name, object Value)[] parameters)
        {
            using SqlCommand cmd = new(sql, conn, tran);
            foreach (var parameter in parameters)
                cmd.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }
    }
}

