using FrmProject.Models;
using FrmProject.GUI;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    public class DeviceRepository
    {
        public static DataTable GetAllDevices()
        {
            const string query = @"
                SELECT 
                    d.DeviceID,
                    d.DeviceCode,
                    d.DeviceName        AS [Tên thiết bị],
                    d.DeviceCode        AS [Mã thiết bị],
                    dc.CategoryName     AS [Loại thiết bị],
                    ISNULL(r.RoomName, '') AS [Vị trí],
                    d.Status            AS [Tình trạng],
                    d.TotalQuantity     AS [Số lượng],
                    d.AvailableQuantity AS AvailableQuantity,
                    ISNULL(d.Note, '')  AS [Ghi chú]
                FROM dbo.Devices d
                JOIN dbo.DeviceCategories dc ON dc.CategoryID = d.CategoryID
                LEFT JOIN dbo.Rooms r ON r.RoomID = d.RoomID
                ORDER BY d.DeviceID";

            using SqlConnection conn = DbHelper.GetConnection();
            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static int GetTotalDevicesCount(string keyword = "", string categoryName = "", string status = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string query = @"
                SELECT COUNT(*)
                FROM dbo.Devices d
                JOIN dbo.DeviceCategories dc ON dc.CategoryID = d.CategoryID
                LEFT JOIN dbo.Rooms r ON r.RoomID = d.RoomID
                WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(keyword)) query += " AND (d.DeviceName LIKE @kw OR d.Note LIKE @kw)";
            if (!string.IsNullOrWhiteSpace(categoryName)) query += " AND dc.CategoryName = @cat";
            if (!string.IsNullOrWhiteSpace(status)) query += " AND d.Status = @stt";

            using SqlCommand cmd = new SqlCommand(query, conn);
            if (!string.IsNullOrWhiteSpace(keyword)) cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
            if (!string.IsNullOrWhiteSpace(categoryName)) cmd.Parameters.AddWithValue("@cat", categoryName);
            if (!string.IsNullOrWhiteSpace(status)) cmd.Parameters.AddWithValue("@stt", status);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static DataTable GetDevicesPaged(int pageNumber, int pageSize, string keyword = "", string categoryName = "", string status = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            string query = @"
                SELECT 
                    d.DeviceID,
                    d.DeviceCode,
                    d.DeviceName        AS [Tên thiết bị],
                    d.DeviceCode        AS [Mã thiết bị],
                    dc.CategoryName     AS [Loại thiết bị],
                    ISNULL(r.RoomName, '') AS [Vị trí],
                    d.Status            AS [Tình trạng],
                    d.TotalQuantity     AS [Số lượng],
                    d.AvailableQuantity AS AvailableQuantity,
                    ISNULL(d.Note, '')  AS [Ghi chú]
                FROM dbo.Devices d
                JOIN dbo.DeviceCategories dc ON dc.CategoryID = d.CategoryID
                LEFT JOIN dbo.Rooms r ON r.RoomID = d.RoomID
                WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(keyword)) query += " AND (d.DeviceName LIKE @kw OR d.Note LIKE @kw)";
            if (!string.IsNullOrWhiteSpace(categoryName)) query += " AND dc.CategoryName = @cat";
            if (!string.IsNullOrWhiteSpace(status)) query += " AND d.Status = @stt";
            query += @"
                ORDER BY d.DeviceID
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
            cmd.Parameters.AddWithValue("@limit", pageSize);
            if (!string.IsNullOrWhiteSpace(keyword)) cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
            if (!string.IsNullOrWhiteSpace(categoryName)) cmd.Parameters.AddWithValue("@cat", categoryName);
            if (!string.IsNullOrWhiteSpace(status)) cmd.Parameters.AddWithValue("@stt", status);

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetCategories()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            using SqlCommand cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM dbo.DeviceCategories ORDER BY CategoryName", conn);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static void SaveDevice(
            int deviceId,
            string deviceCode,
            string deviceName,
            string categoryName,
            string roomNameOrCode,
            int totalQuantity,
            int selectedTotalQuantity,
            int selectedAvailableQuantity,
            string status,
            string note)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            int categoryID = GetOrCreateCategoryId(conn, categoryName);
            int? roomID = ResolveRoomId(conn, roomNameOrCode);

            if (!string.IsNullOrWhiteSpace(roomNameOrCode) && roomID == null)
                throw new InvalidOperationException("Vị trí/phòng không tồn tại. Hãy nhập đúng mã hoặc tên phòng.");

            if (DeviceCodeExists(conn, deviceCode, deviceId))
                throw new InvalidOperationException("Mã thiết bị đã tồn tại. Vui lòng nhập mã khác.");

            if (deviceId < 0)
            {
                using SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO dbo.Devices 
                        (DeviceCode, DeviceName, CategoryID, RoomID, TotalQuantity, AvailableQuantity, Status, Note)
                      VALUES (@code, @name, @catID, @roomID, @qty, @qty, @status, @note)", conn);
                AddDeviceParameters(cmd, deviceCode, deviceName, categoryID, roomID, totalQuantity, totalQuantity, status, note);
                cmd.ExecuteNonQuery();
                return;
            }

            int borrowedQuantity = selectedTotalQuantity - selectedAvailableQuantity;
            if (totalQuantity < borrowedQuantity)
                throw new InvalidOperationException($"Không thể giảm tổng số lượng xuống {totalQuantity} vì hiện có {borrowedQuantity} thiết bị đang được mượn.");

            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE dbo.Devices SET
                    DeviceCode = @code, DeviceName = @name, CategoryID = @catID, RoomID = @roomID,
                    TotalQuantity = @totalQty, AvailableQuantity = @availableQty,
                    Status = @status, Note = @note
                  WHERE DeviceID = @id", conn))
            {
                AddDeviceParameters(cmd, deviceCode, deviceName, categoryID, roomID, totalQuantity, totalQuantity - borrowedQuantity, status, note);
                cmd.Parameters.AddWithValue("@id", deviceId);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteDevice(int deviceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE dbo.Devices
                    SET Status = @retiredStatus,
                        AvailableQuantity = 0,
                        Note = LTRIM(RTRIM(CONCAT(ISNULL(NULLIF(Note, ''), ''), CASE WHEN ISNULL(NULLIF(Note, ''), '') = '' THEN '' ELSE ' | ' END, @note)))
                    WHERE DeviceID = @id", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
                    cmd.Parameters.AddWithValue("@note", $"Xóa mềm ngày {DateTime.Now:dd/MM/yyyy HH:mm}");
                    cmd.Parameters.AddWithValue("@id", deviceId);
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE dbo.DeviceInstances
                    SET Status = @retiredStatus,
                        Note = LTRIM(RTRIM(CONCAT(ISNULL(NULLIF(Note, ''), ''), CASE WHEN ISNULL(NULLIF(Note, ''), '') = '' THEN '' ELSE ' | ' END, @note)))
                    WHERE DeviceID = @id
                      AND Status = @availableStatus", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
                    cmd.Parameters.AddWithValue("@availableStatus", DeviceStatus.Available);
                    cmd.Parameters.AddWithValue("@note", $"Xóa mềm theo thiết bị ngày {DateTime.Now:dd/MM/yyyy HH:mm}");
                    cmd.Parameters.AddWithValue("@id", deviceId);
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static string GenerateDeviceCode()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string query = @"
                SELECT MAX(CAST(SUBSTRING(DeviceCode, 3, LEN(DeviceCode) - 2) AS INT)) 
                FROM dbo.Devices 
                WHERE DeviceCode LIKE 'TB[0-9]%'";
            using SqlCommand cmd = new SqlCommand(query, conn);
            object? result = cmd.ExecuteScalar();
            int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
            return $"TB{nextId:D2}";
        }

        public static List<DeviceDisplayModel> GetAvailableDevices()
        {
            List<DeviceDisplayModel> devices = new List<DeviceDisplayModel>();

            string query = @"
                SELECT 
                    d.DeviceID,
                    d.DeviceName,
                    dc.CategoryName,
                    ISNULL(r.RoomName, N'') AS RoomName,
                    d.Status,
                    d.AvailableQuantity,
                    ISNULL(d.Note, N'') AS Note
                FROM dbo.Devices d
                INNER JOIN dbo.DeviceCategories dc 
                    ON d.CategoryID = dc.CategoryID
                LEFT JOIN dbo.Rooms r 
                    ON d.RoomID = r.RoomID
                WHERE d.AvailableQuantity > 0
                  AND d.Status NOT IN (@maintenanceStatus, @retiredStatus)
                  AND ISNULL(r.Status, N'Hoạt động') <> @retiredRoomStatus
                ORDER BY d.DeviceID;";

            using SqlConnection conn = DbHelper.GetConnection();
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@maintenanceStatus", DeviceStatus.Maintenance);
            cmd.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
            cmd.Parameters.AddWithValue("@retiredRoomStatus", RoomStatus.Retired);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DeviceDisplayModel device = new DeviceDisplayModel
                {
                    DeviceID = Convert.ToInt32(reader["DeviceID"]),
                    DeviceName = reader["DeviceName"]?.ToString() ?? string.Empty,
                    CategoryName = reader["CategoryName"]?.ToString() ?? string.Empty,
                    RoomName = reader["RoomName"]?.ToString() ?? string.Empty,
                    Status = reader["Status"]?.ToString() ?? string.Empty,
                    AvailableQuantity = Convert.ToInt32(reader["AvailableQuantity"]),
                    Note = reader["Note"]?.ToString() ?? string.Empty
                };

                devices.Add(device);
            }

            return devices;
        }

        public static DataTable GetDevicesByRoom(string roomCode)
        {
            DataTable dt = new DataTable();
            string query = @"
        SELECT 
            d.DeviceCode        AS [Mã TB],
            d.DeviceName        AS [Tên thiết bị],
            d.TotalQuantity     AS [SL],
            d.Status            AS [Tình trạng]
        FROM Devices d
        JOIN Rooms r ON d.RoomID = r.RoomID
        WHERE r.RoomCode = @RoomCode
          AND d.Status <> @retiredStatus";

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@RoomCode", roomCode);
                da.SelectCommand.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
                da.Fill(dt);
            }
            return dt;
        }

        private static int GetOrCreateCategoryId(SqlConnection conn, string categoryName)
        {
            using SqlCommand cmd = new SqlCommand("SELECT CategoryID FROM dbo.DeviceCategories WHERE CategoryName = @name", conn);
            cmd.Parameters.AddWithValue("@name", categoryName);
            object? result = cmd.ExecuteScalar();
            if (result != null)
                return Convert.ToInt32(result);

            using SqlCommand insertCmd = new SqlCommand("INSERT INTO dbo.DeviceCategories (CategoryName) VALUES (@name); SELECT SCOPE_IDENTITY();", conn);
            insertCmd.Parameters.AddWithValue("@name", categoryName);
            return Convert.ToInt32(insertCmd.ExecuteScalar());
        }

        private static int? ResolveRoomId(SqlConnection conn, string roomNameOrCode)
        {
            if (string.IsNullOrWhiteSpace(roomNameOrCode))
                return null;

            using SqlCommand cmd = new SqlCommand("SELECT TOP 1 RoomID FROM dbo.Rooms WHERE RoomName = @roomName OR RoomCode = @roomName", conn);
            cmd.Parameters.AddWithValue("@roomName", roomNameOrCode);
            object? result = cmd.ExecuteScalar();
            return result == null ? null : Convert.ToInt32(result);
        }

        private static bool DeviceCodeExists(SqlConnection conn, string deviceCode, int deviceId)
        {
            using SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM dbo.Devices WHERE DeviceCode = @code AND DeviceID <> @id", conn);
            cmd.Parameters.AddWithValue("@code", deviceCode);
            cmd.Parameters.AddWithValue("@id", deviceId < 0 ? 0 : deviceId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private static void AddDeviceParameters(SqlCommand cmd, string deviceCode, string deviceName, int categoryId, int? roomId, int totalQuantity, int availableQuantity, string status, string note)
        {
            cmd.Parameters.AddWithValue("@code", deviceCode);
            cmd.Parameters.AddWithValue("@name", deviceName);
            cmd.Parameters.AddWithValue("@catID", categoryId);
            cmd.Parameters.AddWithValue("@roomID", (object?)roomId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@totalQty", totalQuantity);
            cmd.Parameters.AddWithValue("@availableQty", availableQuantity);
            cmd.Parameters.AddWithValue("@qty", totalQuantity);
            cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(status) ? "Tốt" : status);
            cmd.Parameters.AddWithValue("@note", note);
        }
    }
}

