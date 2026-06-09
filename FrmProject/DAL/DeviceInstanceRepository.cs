using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    internal static class DeviceInstanceRepository
    {
        public static DataTable GetByDevice(int deviceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            using SqlCommand cmd = new SqlCommand(
                "SELECT InstanceID, AssetCode AS [Mã tài sản], Status AS [Trạng thái], Condition AS [Tình trạng] FROM dbo.DeviceInstances WHERE DeviceID = @id ORDER BY InstanceID", conn);
            cmd.Parameters.AddWithValue("@id", deviceId);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Lấy DeviceCode (mã gốc) của loại thiết bị.
        /// </summary>
        public static string GetDeviceCode(int deviceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlCommand cmd = new SqlCommand("SELECT DeviceCode FROM dbo.Devices WHERE DeviceID = @id", conn);
            cmd.Parameters.AddWithValue("@id", deviceId);
            return cmd.ExecuteScalar()?.ToString() ?? deviceId.ToString();
        }

        /// <summary>
        /// Tự động sinh mã cá thể tiếp theo dạng {DeviceCode}-{STT:D3}.
        /// Ví dụ: TH001 → TH001-001, TH001-002, ...
        /// </summary>
        public static string GetNextAssetCode(int deviceId)
        {
            string deviceCode = GetDeviceCode(deviceId);

            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            // Lấy tất cả AssetCode hiện tại của thiết bị này để tính số thứ tự tiếp theo
            using SqlCommand cmd = new SqlCommand(
                "SELECT AssetCode FROM dbo.DeviceInstances WHERE DeviceID = @id", conn);
            cmd.Parameters.AddWithValue("@id", deviceId);

            var existingCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    existingCodes.Add(reader.GetString(0));
            }

            // Tìm số thứ tự nhỏ nhất chưa dùng
            int seq = 1;
            string candidate;
            do
            {
                candidate = $@"{deviceCode}\{seq:D3}";
                seq++;
            } while (existingCodes.Contains(candidate));

            return candidate;
        }

        public static bool AssetCodeExists(string assetCode)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.DeviceInstances WHERE AssetCode = @code", conn);
            cmd.Parameters.AddWithValue("@code", assetCode);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public static void Insert(int deviceId, string assetCode, string status, string condition)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                using SqlCommand insertCmd = new SqlCommand(
                    "INSERT INTO dbo.DeviceInstances (DeviceID, AssetCode, Status, Condition) VALUES (@devId, @code, @status, @cond)", conn, tran);
                insertCmd.Parameters.AddWithValue("@devId", deviceId);
                insertCmd.Parameters.AddWithValue("@code", assetCode);
                insertCmd.Parameters.AddWithValue("@status", status);
                insertCmd.Parameters.AddWithValue("@cond", condition);
                insertCmd.ExecuteNonQuery();

                UpdateDeviceQuantities(conn, tran, deviceId);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static void Delete(int deviceId, int instanceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                using (SqlCommand statusCmd = new SqlCommand(
                    "SELECT Status FROM dbo.DeviceInstances WHERE InstanceID = @id AND DeviceID = @deviceId", conn, tran))
                {
                    statusCmd.Parameters.AddWithValue("@id", instanceId);
                    statusCmd.Parameters.AddWithValue("@deviceId", deviceId);
                    string currentStatus = statusCmd.ExecuteScalar()?.ToString() ?? string.Empty;
                    if (string.Equals(currentStatus, DeviceStatus.Borrowed, StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("Mã cá thể đang được mượn. Hãy xử lý trả thiết bị trước khi xóa mềm.");
                }

                using SqlCommand cmd = new SqlCommand(@"
                    UPDATE dbo.DeviceInstances
                    SET Status = @retiredStatus,
                        Note = LTRIM(RTRIM(CONCAT(ISNULL(NULLIF(Note, ''), ''), CASE WHEN ISNULL(NULLIF(Note, ''), '') = '' THEN '' ELSE ' | ' END, @note)))
                    WHERE InstanceID = @id
                      AND DeviceID = @deviceId", conn, tran);
                cmd.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
                cmd.Parameters.AddWithValue("@note", $"Xóa mềm ngày {DateTime.Now:dd/MM/yyyy HH:mm}");
                cmd.Parameters.AddWithValue("@id", instanceId);
                cmd.Parameters.AddWithValue("@deviceId", deviceId);
                cmd.ExecuteNonQuery();

                UpdateDeviceQuantities(conn, tran, deviceId);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static void UpdateStatusAndCondition(int deviceId, int instanceId, string status, string condition)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                using SqlCommand cmd = new SqlCommand(
                    @"UPDATE dbo.DeviceInstances
                      SET Status = @status,
                          Condition = @condition
                      WHERE InstanceID = @instanceId
                        AND DeviceID = @deviceId", conn, tran);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@condition", condition);
                cmd.Parameters.AddWithValue("@instanceId", instanceId);
                cmd.Parameters.AddWithValue("@deviceId", deviceId);

                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    throw new InvalidOperationException("Không tìm thấy mã cá thể cần cập nhật.");

                UpdateDeviceQuantities(conn, tran, deviceId);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static void UpdateDeviceQuantities(SqlConnection conn, SqlTransaction tran, int deviceId)
        {
            using SqlCommand cmd = new SqlCommand(@"
                UPDATE dbo.Devices 
                SET TotalQuantity = (SELECT COUNT(*) FROM dbo.DeviceInstances WHERE DeviceID = @id AND Status <> @retiredStatus),
                    AvailableQuantity = (
                        SELECT COUNT(*)
                        FROM dbo.DeviceInstances
                        WHERE DeviceID = @id
                          AND Status = @availableStatus
                          AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
                    )
                WHERE DeviceID = @id", conn, tran);
            cmd.Parameters.AddWithValue("@id", deviceId);
            cmd.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
            cmd.Parameters.AddWithValue("@availableStatus", DeviceStatus.Available);
            cmd.Parameters.AddWithValue("@goodCondition", DeviceCondition.Good);
            cmd.ExecuteNonQuery();
        }
    }
}

