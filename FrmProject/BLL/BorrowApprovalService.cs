using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.BLL
{
    /// <summary>
    /// Shared service for approving/rejecting PENDING borrow tickets.
    /// </summary>
    internal static class BorrowApprovalService
    {
        public static void ApproveBorrow(int ticketId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();

            try
            {
                DbSchemaHelper.EnsureBorrowTicketStatusConstraint(conn, tran);

                string currentStatus = ReturnApprovalService.GetTicketStatus(conn, tran, ticketId);
                if (!string.Equals(currentStatus, BorrowTicketStatus.Pending, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Phiếu này không còn ở trạng thái chờ duyệt.");

                List<(int DeviceID, int InstanceID, string DeviceName, int AvailableQuantity, int RequestedQuantity, string DeviceStatus, string InstanceStatus, string InstanceCondition)> items =
                    LoadBorrowItems(conn, tran, ticketId);

                if (items.Count == 0)
                    throw new InvalidOperationException("Phiếu này chưa có chi tiết thiết bị để phê duyệt.");

                foreach (var item in items)
                {
                    if (string.Equals(item.DeviceStatus, DeviceStatus.Maintenance, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException(
                            $"Thiết bị '{item.DeviceName}' đang ở trạng thái bảo trì, không thể xuất kho.");
                    }

                    if (string.Equals(item.DeviceStatus, DeviceStatus.Retired, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException(
                            $"Thiết bị '{item.DeviceName}' đã ngừng sử dụng, không thể xuất kho.");
                    }

                    if (item.AvailableQuantity < item.RequestedQuantity)
                    {
                        throw new InvalidOperationException(
                            $"Thiết bị '{item.DeviceName}' chỉ còn {item.AvailableQuantity}, không đủ {item.RequestedQuantity} để phê duyệt.");
                    }

                    if (item.InstanceID > 0)
                    {
                        if (!string.Equals(item.InstanceStatus, DeviceStatus.Available, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new InvalidOperationException(
                                $"Cá thể của thiết bị '{item.DeviceName}' không còn ở trạng thái có sẵn, không thể phê duyệt mượn.");
                        }

                        if (!string.Equals(NormalizeBorrowCondition(item.InstanceCondition), DeviceCondition.Good, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new InvalidOperationException(
                                $"Cá thể của thiết bị '{item.DeviceName}' có tình trạng '{item.InstanceCondition}', chỉ thiết bị tình trạng Tốt mới được mượn.");
                        }
                    }
                }

                using (var cmd = new SqlCommand(
                    "UPDATE BorrowTickets SET Status = @status WHERE TicketID = @id", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@status", BorrowTicketStatus.Borrowing);
                    cmd.Parameters.AddWithValue("@id", ticketId);
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SqlCommand(
                    @"UPDATE Devices
                      SET AvailableQuantity = AvailableQuantity - @qty
                      WHERE DeviceID = @deviceId
                        AND AvailableQuantity >= @qty", conn, tran))
                {
                    cmd.Parameters.Add("@qty", SqlDbType.Int);
                    cmd.Parameters.Add("@deviceId", SqlDbType.Int);

                    foreach (var item in items)
                    {
                        cmd.Parameters["@qty"].Value = item.RequestedQuantity;
                        cmd.Parameters["@deviceId"].Value = item.DeviceID;
                        int affected = cmd.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            throw new InvalidOperationException(
                                $"Thiết bị '{item.DeviceName}' không còn đủ tồn kho để phê duyệt.");
                        }
                    }
                }

                // Update instances
                using (var cmd = new SqlCommand(
                    @"UPDATE DeviceInstances
                      SET Status = @status
                      WHERE InstanceID = @instanceID
                        AND Status = @availableStatus
                        AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition", conn, tran))
                {
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = DeviceStatus.Borrowed;
                    cmd.Parameters.Add("@availableStatus", SqlDbType.NVarChar, 50).Value = DeviceStatus.Available;
                    cmd.Parameters.Add("@goodCondition", SqlDbType.NVarChar, 50).Value = DeviceCondition.Good;
                    cmd.Parameters.Add("@instanceID", SqlDbType.Int);

                    foreach (var item in items.Where(x => x.InstanceID > 0))
                    {
                        cmd.Parameters["@instanceID"].Value = item.InstanceID;
                        int affected = cmd.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            throw new InvalidOperationException(
                                $"Cá thể của thiết bị '{item.DeviceName}' không còn khả dụng để phê duyệt.");
                        }
                    }
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static void RejectBorrow(int ticketId, string reason)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            DbSchemaHelper.EnsureBorrowTicketStatusConstraint(conn);
            DbSchemaHelper.EnsureBorrowTicketNoteCapacity(conn);

            string currentStatus = ReturnApprovalService.GetTicketStatus(conn, null, ticketId);
            if (!string.Equals(currentStatus, BorrowTicketStatus.Pending, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Phiếu này không còn ở trạng thái chờ duyệt.");

            using var cmd = new SqlCommand(
                "UPDATE BorrowTickets SET Status = @status, Note = ISNULL(Note, '') + ' | Từ chối: ' + @reason WHERE TicketID = @id", conn);
            cmd.Parameters.AddWithValue("@id", ticketId);
            cmd.Parameters.AddWithValue("@status", BorrowTicketStatus.Rejected);
            cmd.Parameters.AddWithValue("@reason", reason);
            cmd.ExecuteNonQuery();
        }

        private static List<(int DeviceID, int InstanceID, string DeviceName, int AvailableQuantity, int RequestedQuantity, string DeviceStatus, string InstanceStatus, string InstanceCondition)> LoadBorrowItems(
            SqlConnection conn,
            SqlTransaction tran,
            int ticketId)
        {
            using var cmd = new SqlCommand(
                @"SELECT d.DeviceID,
                         ISNULL(bd.InstanceID, 0),
                         d.DeviceName,
                         d.AvailableQuantity,
                         bd.Quantity,
                         ISNULL(d.Status, ''),
                         ISNULL(di.Status, ''),
                         ISNULL(di.Condition, '')
                  FROM BorrowDetails bd
                  JOIN Devices d WITH (UPDLOCK, HOLDLOCK) ON d.DeviceID = bd.DeviceID
                  LEFT JOIN DeviceInstances di WITH (UPDLOCK, HOLDLOCK) ON di.InstanceID = bd.InstanceID
                  WHERE bd.TicketID = @id", conn, tran);
            cmd.Parameters.AddWithValue("@id", ticketId);

            using var reader = cmd.ExecuteReader();
            List<(int DeviceID, int InstanceID, string DeviceName, int AvailableQuantity, int RequestedQuantity, string DeviceStatus, string InstanceStatus, string InstanceCondition)> items = new();
            while (reader.Read())
            {
                items.Add((
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    reader.IsDBNull(7) ? string.Empty : reader.GetString(7)));
            }

            return items;
        }

        private static string NormalizeBorrowCondition(string condition)
        {
            return string.IsNullOrWhiteSpace(condition)
                ? DeviceCondition.Good
                : condition.Trim();
        }
    }
}

