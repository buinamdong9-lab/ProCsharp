using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.BLL
{
    /// <summary>
    /// Shared service for approving/rejecting RETURN_PENDING tickets.
    /// Used by both UcDanhsachphieu and UcTrathietbi.
    /// </summary>
    internal static class ReturnApprovalService
    {
        /// <summary>
        /// Approves a RETURN_PENDING ticket: restores inventory and marks RETURNED.
        /// </summary>
        public static void ApproveReturn(int ticketId, int approvedByUserId, List<(int DeviceID, int InstanceID)>? selectedItems = null)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();

            try
            {
                DbSchemaHelper.EnsureBorrowTicketStatusConstraint(conn, tran);
                DbSchemaHelper.EnsureBorrowTicketNoteCapacity(conn, tran);

                string currentStatus = GetTicketStatus(conn, tran, ticketId);
                if (!string.Equals(currentStatus, BorrowTicketStatus.ReturnPending, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Phiếu này không còn ở trạng thái chờ duyệt trả.");

                if (!ReturnRequestRepository.TryLoadRequest(conn, tran, ticketId, out DateTime requestedAt, out List<ReturnRequestItem> items))
                    throw new InvalidOperationException("Không đọc được dữ liệu yêu cầu trả của phiếu này.");

                // If selectedItems is specified, we filter to process only those.
                List<ReturnRequestItem> itemsToApprove = items;
                if (selectedItems != null && selectedItems.Count > 0)
                {
                    itemsToApprove = items.Where(x => selectedItems.Any(s => s.DeviceID == x.DeviceID && (x.InstanceID == 0 || s.InstanceID == x.InstanceID))).ToList();
                    if (itemsToApprove.Count == 0)
                        throw new InvalidOperationException("Không có thiết bị được chọn nào khớp với yêu cầu trả.");
                }

                foreach (var item in itemsToApprove.Where(x => x.ReturnQty > 0))
                    ValidateReturnQuantity(conn, tran, ticketId, item);

                // 1. Process Device Instances (InstanceID > 0)
                var instanceItems = itemsToApprove.Where(x => x.ReturnQty > 0 && x.InstanceID > 0).ToList();
                if (instanceItems.Count > 0)
                {
                    string instanceUpdateSql = DbSchemaHelper.HasColumn(conn, "DeviceInstances", "Condition", tran)
                        ? @"UPDATE DeviceInstances
                            SET Status = @status,
                                Condition = @condition
                            WHERE InstanceID = @instanceID"
                        : @"UPDATE DeviceInstances
                            SET Status = @status
                            WHERE InstanceID = @instanceID";

                    using (var cmd = new SqlCommand(instanceUpdateSql, conn, tran))
                    {
                        cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50);
                        if (DbSchemaHelper.HasColumn(conn, "DeviceInstances", "Condition", tran))
                            cmd.Parameters.Add("@condition", SqlDbType.NVarChar, 255);
                        cmd.Parameters.Add("@instanceID", SqlDbType.Int);

                        foreach (var item in instanceItems)
                        {
                            bool available = IsReturnedAsAvailable(item.Note);
                            bool deviceRetired = IsDeviceRetired(conn, tran, item.DeviceID);
                            cmd.Parameters["@status"].Value = deviceRetired
                                ? DeviceStatus.Retired
                                : available ? DeviceStatus.Available : DeviceStatus.Maintenance;
                            if (cmd.Parameters.Contains("@condition"))
                                cmd.Parameters["@condition"].Value = NormalizeReturnCondition(item.Note);
                            cmd.Parameters["@instanceID"].Value = item.InstanceID;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Sync Devices.TotalQuantity and Devices.AvailableQuantity for devices with instances
                    var instanceDeviceIds = instanceItems
                        .Select(x => x.DeviceID)
                        .Distinct()
                        .ToList();

                    foreach (var deviceId in instanceDeviceIds)
                    {
                        DeviceInstanceRepository.UpdateDeviceQuantities(conn, tran, deviceId);
                    }
                }

                // 2. Process Bulk Devices (InstanceID <= 0)
                var bulkItems = itemsToApprove.Where(x => x.ReturnQty > 0 && x.InstanceID <= 0).ToList();
                if (bulkItems.Count > 0)
                {
                    using var updateGoodCmd = new SqlCommand(
                        @"UPDATE Devices
                          SET AvailableQuantity = AvailableQuantity + @qty
                          WHERE DeviceID = @deviceID
                            AND Status <> @retiredStatus", conn, tran);
                    updateGoodCmd.Parameters.Add("@qty", SqlDbType.Int);
                    updateGoodCmd.Parameters.Add("@deviceID", SqlDbType.Int);
                    updateGoodCmd.Parameters.Add("@retiredStatus", SqlDbType.NVarChar, 50).Value = DeviceStatus.Retired;

                    using var updateDamagedCmd = new SqlCommand(
                        @"UPDATE Devices
                          SET TotalQuantity = TotalQuantity - @qty
                          WHERE DeviceID = @deviceID
                            AND Status <> @retiredStatus", conn, tran);
                    updateDamagedCmd.Parameters.Add("@qty", SqlDbType.Int);
                    updateDamagedCmd.Parameters.Add("@deviceID", SqlDbType.Int);
                    updateDamagedCmd.Parameters.Add("@retiredStatus", SqlDbType.NVarChar, 50).Value = DeviceStatus.Retired;

                    foreach (var item in bulkItems)
                    {
                        bool available = IsReturnedAsAvailable(item.Note);
                        if (available)
                        {
                            updateGoodCmd.Parameters["@qty"].Value = item.ReturnQty;
                            updateGoodCmd.Parameters["@deviceID"].Value = item.DeviceID;
                            updateGoodCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            updateDamagedCmd.Parameters["@qty"].Value = item.ReturnQty;
                            updateDamagedCmd.Parameters["@deviceID"].Value = item.DeviceID;
                            updateDamagedCmd.ExecuteNonQuery();
                        }
                    }
                }

                ApplyReturnedQuantities(conn, tran, ticketId, itemsToApprove);

                // Now handle request database changes
                bool requestFullyApproved = true;
                if (selectedItems != null && selectedItems.Count > 0)
                {
                    // Delete approved items from ReturnRequestDetails
                    using (var deleteReqDetailCmd = new SqlCommand(
                        @"DELETE rrd
                          FROM ReturnRequestDetails rrd
                          JOIN ReturnRequests rr ON rr.ReturnRequestID = rrd.ReturnRequestID
                          WHERE rr.TicketID = @ticketId
                            AND rrd.DeviceID = @deviceId
                            AND ISNULL(rrd.InstanceID, 0) = @instanceId", conn, tran))
                    {
                        deleteReqDetailCmd.Parameters.Add("@ticketId", SqlDbType.Int).Value = ticketId;
                        deleteReqDetailCmd.Parameters.Add("@deviceId", SqlDbType.Int);
                        deleteReqDetailCmd.Parameters.Add("@instanceId", SqlDbType.Int);

                        foreach (var item in itemsToApprove)
                        {
                            deleteReqDetailCmd.Parameters["@deviceId"].Value = item.DeviceID;
                            deleteReqDetailCmd.Parameters["@instanceId"].Value = item.InstanceID;
                            deleteReqDetailCmd.ExecuteNonQuery();
                        }
                    }

                    // Check if any items left in request
                    using (var checkRemainingCmd = new SqlCommand(
                        @"SELECT COUNT(*)
                          FROM ReturnRequestDetails rrd
                          JOIN ReturnRequests rr ON rr.ReturnRequestID = rrd.ReturnRequestID
                          WHERE rr.TicketID = @ticketId", conn, tran))
                    {
                        checkRemainingCmd.Parameters.AddWithValue("@ticketId", ticketId);
                        int remainingReqItems = Convert.ToInt32(checkRemainingCmd.ExecuteScalar());
                        requestFullyApproved = (remainingReqItems == 0);
                    }
                }

                int remainingQuantity = GetRemainingBorrowQuantity(conn, tran, ticketId);
                bool fullyReturned = remainingQuantity == 0;

                if (requestFullyApproved)
                {
                    string nextStatus = fullyReturned ? BorrowTicketStatus.Returned : BorrowTicketStatus.Borrowing;
                    string approvalNote = fullyReturned
                        ? "Admin đã duyệt yêu cầu trả thiết bị."
                        : $"Admin đã duyệt trả một phần. Còn {remainingQuantity} thiết bị chưa trả.";

                    string approveSql = BuildApproveReturnSql(conn, tran);
                    using (var cmd = new SqlCommand(approveSql, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@id", ticketId);
                        cmd.Parameters.AddWithValue("@status", nextStatus);
                        cmd.Parameters.AddWithValue("@fullyReturned", fullyReturned ? 1 : 0);
                        cmd.Parameters.AddWithValue("@returnDate", requestedAt == DateTime.MinValue ? DateTime.Now : requestedAt);
                        cmd.Parameters.AddWithValue("@returnedBy", approvedByUserId <= 0 ? DBNull.Value : approvedByUserId);
                        cmd.Parameters.AddWithValue("@note", approvalNote);
                        cmd.ExecuteNonQuery();
                    }

                    ReturnRequestRepository.ClearRequest(conn, tran, ticketId);
                }
                else
                {
                    // Part of the request is still pending
                    // The ticket remains in RETURN_PENDING status.
                    // We update the ReturnNote payload in BorrowTickets to contain only the remaining items.
                    if (DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnNote", tran))
                    {
                        // Load the remaining request items
                        List<ReturnRequestItem> remainingReqItems = new();
                        using (var loadRemainingCmd = new SqlCommand(
                            @"SELECT d.DeviceID, ISNULL(d.InstanceID, 0), d.BorrowQuantity, d.ReturnQuantity, ISNULL(d.Note, '')
                              FROM ReturnRequests r
                              JOIN ReturnRequestDetails d ON d.ReturnRequestID = r.ReturnRequestID
                              WHERE r.TicketID = @ticketId", conn, tran))
                        {
                            loadRemainingCmd.Parameters.AddWithValue("@ticketId", ticketId);
                            using (var reader = loadRemainingCmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    remainingReqItems.Add(new ReturnRequestItem
                                    {
                                        DeviceID = reader.GetInt32(0),
                                        InstanceID = reader.GetInt32(1),
                                        BorrowQty = reader.GetInt32(2),
                                        ReturnQty = reader.GetInt32(3),
                                        Note = reader.GetString(4)
                                    });
                                }
                            }
                        }

                        var tupleList = remainingReqItems.Select(x => (x.DeviceID, x.InstanceID, x.BorrowQty, x.ReturnQty, x.Note)).ToList();
                        string payload = ReturnRequestHelper.BuildPayload(requestedAt, tupleList);

                        using (var updatePayloadCmd = new SqlCommand(
                            "UPDATE BorrowTickets SET ReturnNote = @payload, Note = ISNULL(Note, '') + @partialApprovalNote WHERE TicketID = @id", conn, tran))
                        {
                            string partialApprovalNote = $" | Admin duyệt trả một phần ({itemsToApprove.Count} dòng). Còn lại đang tranh chấp.";
                            updatePayloadCmd.Parameters.AddWithValue("@payload", payload);
                            updatePayloadCmd.Parameters.AddWithValue("@partialApprovalNote", partialApprovalNote);
                            updatePayloadCmd.Parameters.AddWithValue("@id", ticketId);
                            updatePayloadCmd.ExecuteNonQuery();
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

        /// <summary>
        /// Rejects a RETURN_PENDING ticket: reverts to BORROWING and clears ReturnNote.
        /// </summary>
        public static void RejectReturn(int ticketId, string reason)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();

            try
            {
                DbSchemaHelper.EnsureBorrowTicketStatusConstraint(conn, tran);
                DbSchemaHelper.EnsureBorrowTicketNoteCapacity(conn, tran);

                using var cmd = new SqlCommand(BuildRejectReturnSql(conn, tran), conn, tran);
                cmd.Parameters.AddWithValue("@id", ticketId);
                cmd.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
                cmd.Parameters.AddWithValue("@returnPendingStatus", BorrowTicketStatus.ReturnPending);
                cmd.Parameters.AddWithValue("@note", "Yêu cầu trả bị từ chối: " + reason.Trim());

                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    throw new InvalidOperationException("Phiếu này không còn ở trạng thái chờ duyệt trả.");

                ReturnRequestRepository.ClearRequest(conn, tran, ticketId);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Verifies that a ticket belongs to a specific user. Throws if not.
        /// </summary>
        public static void VerifyTicketOwnership(SqlConnection conn, SqlTransaction tran, int ticketId, int userId)
        {
            using var cmd = new SqlCommand("SELECT UserID FROM BorrowTickets WHERE TicketID = @id", conn, tran);
            cmd.Parameters.AddWithValue("@id", ticketId);
            var result = cmd.ExecuteScalar();
            if (result == null)
                throw new InvalidOperationException("Phiếu mượn không tồn tại.");
            int ownerUserId = Convert.ToInt32(result);
            if (ownerUserId != userId)
                throw new InvalidOperationException("Bạn không có quyền thao tác trên phiếu này.");
        }

        /// <summary>
        /// Loads pending return tickets for admin review.
        /// </summary>
        public static DataTable GetPendingReturnTickets()
        {
            DataTable dt = new DataTable();
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string ticketDisplayExpr = DbSchemaHelper.GetBorrowTicketDisplayExpressionForCast(conn, "bt");

            string query = $@"
                SELECT bt.TicketID,
                       {ticketDisplayExpr} AS [Số phiếu],
                       u.FullName AS [Người mượn],
                       CONVERT(VARCHAR, bt.BorrowDate, 103) AS [Ngày mượn],
                       CONVERT(VARCHAR, bt.ExpectedReturnDate, 103) AS [Hạn trả],
                       ISNULL(bt.Note, '') AS [Ghi chú]
                FROM BorrowTickets bt
                JOIN Users u ON u.UserID = bt.UserID
                WHERE bt.Status = @status
                ORDER BY bt.BorrowDate DESC";

            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@status", BorrowTicketStatus.ReturnPending);
            da.Fill(dt);
            return dt;
        }

        public static string GetTicketStatus(SqlConnection conn, SqlTransaction? tran, int ticketId)
        {
            using var cmd = new SqlCommand("SELECT Status FROM BorrowTickets WHERE TicketID = @id", conn, tran);
            cmd.Parameters.AddWithValue("@id", ticketId);
            return cmd.ExecuteScalar()?.ToString() ?? string.Empty;
        }

        private static bool IsDeviceRetired(SqlConnection conn, SqlTransaction tran, int deviceId)
        {
            using var cmd = new SqlCommand(
                "SELECT CASE WHEN Status = @retiredStatus THEN 1 ELSE 0 END FROM Devices WHERE DeviceID = @deviceId",
                conn,
                tran);
            cmd.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
            cmd.Parameters.AddWithValue("@deviceId", deviceId);
            return Convert.ToInt32(cmd.ExecuteScalar() ?? 0) == 1;
        }

        private static string BuildApproveReturnSql(SqlConnection conn, SqlTransaction tran)
        {
            List<string> updates = new() { "Status = @status" };

            string? returnDateColumn = null;
            if (DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnDate", tran))
                returnDateColumn = "ReturnDate";
            else if (DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ActualReturnDate", tran))
                returnDateColumn = "ActualReturnDate";

            if (!string.IsNullOrWhiteSpace(returnDateColumn))
                updates.Add($"{returnDateColumn} = CASE WHEN @fullyReturned = 1 THEN @returnDate ELSE {returnDateColumn} END");

            if (DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnedBy", tran))
                updates.Add("ReturnedBy = @returnedBy");

            if (DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnNote", tran))
                updates.Add("ReturnNote = NULL");

            updates.Add("Note = CASE WHEN ISNULL(Note, '') = '' THEN @note ELSE ISNULL(Note, '') + ' | ' + @note END");

            return "UPDATE BorrowTickets SET " + string.Join(", ", updates) + " WHERE TicketID = @id";
        }

        private static string BuildRejectReturnSql(SqlConnection conn, SqlTransaction tran)
        {
            List<string> updates = new()
            {
                "Status = @borrowingStatus",
                "Note = CASE WHEN ISNULL(Note, '') = '' THEN @note ELSE ISNULL(Note, '') + ' | ' + @note END"
            };

            if (DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnNote", tran))
                updates.Insert(1, "ReturnNote = NULL");

            return "UPDATE BorrowTickets SET " + string.Join(", ", updates) + " WHERE TicketID = @id AND Status = @returnPendingStatus";
        }

        private static bool IsReturnedAsAvailable(string? returnCondition)
        {
            if (string.IsNullOrWhiteSpace(returnCondition))
                return true;

            string normalized = returnCondition.Trim();
            return normalized.Equals("Tốt", StringComparison.OrdinalIgnoreCase) ||
                   normalized.StartsWith("Tốt -", StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeReturnCondition(string? returnCondition)
        {
            return string.IsNullOrWhiteSpace(returnCondition)
                ? "Tốt"
                : returnCondition.Trim();
        }

        private static void ValidateReturnQuantity(SqlConnection conn, SqlTransaction tran, int ticketId, ReturnRequestItem item)
        {
            using var cmd = new SqlCommand(
                @"SELECT Quantity, ReturnedQuantity
                  FROM BorrowDetails WITH (UPDLOCK, HOLDLOCK)
                  WHERE TicketID = @ticketId
                    AND DeviceID = @deviceId
                    AND ISNULL(InstanceID, 0) = @instanceId", conn, tran);
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            cmd.Parameters.AddWithValue("@deviceId", item.DeviceID);
            cmd.Parameters.AddWithValue("@instanceId", item.InstanceID);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                throw new InvalidOperationException("Chi tiết trả không còn tồn tại trong phiếu mượn.");

            int quantity = reader.GetInt32(0);
            int returnedQuantity = reader.GetInt32(1);
            int currentBorrowQuantity = quantity - returnedQuantity;
            reader.Close();

            if (item.ReturnQty <= 0 || item.ReturnQty > currentBorrowQuantity)
                throw new InvalidOperationException("Số lượng trả không hợp lệ so với số lượng còn đang mượn.");
        }

        private static void ApplyReturnedQuantities(SqlConnection conn, SqlTransaction tran, int ticketId, IEnumerable<ReturnRequestItem> items)
        {
            using var updateCmd = new SqlCommand(
                @"UPDATE BorrowDetails
                  SET ReturnedQuantity = ReturnedQuantity + @returnQty,
                      Note = CASE
                          WHEN ISNULL(@note, '') = '' THEN Note
                          WHEN ISNULL(Note, '') = '' THEN @note
                          ELSE ISNULL(Note, '') + ' | ' + @note
                      END
                  WHERE TicketID = @ticketId
                    AND DeviceID = @deviceId
                    AND ISNULL(InstanceID, 0) = @instanceId
                    AND (Quantity - ReturnedQuantity) >= @returnQty", conn, tran);
            updateCmd.Parameters.Add("@ticketId", SqlDbType.Int).Value = ticketId;
            updateCmd.Parameters.Add("@deviceId", SqlDbType.Int);
            updateCmd.Parameters.Add("@instanceId", SqlDbType.Int);
            updateCmd.Parameters.Add("@returnQty", SqlDbType.Int);
            updateCmd.Parameters.Add("@note", SqlDbType.NVarChar, 255);

            foreach (var item in items.Where(x => x.ReturnQty > 0))
            {
                updateCmd.Parameters["@deviceId"].Value = item.DeviceID;
                updateCmd.Parameters["@instanceId"].Value = item.InstanceID;
                updateCmd.Parameters["@returnQty"].Value = item.ReturnQty;
                updateCmd.Parameters["@note"].Value = string.IsNullOrWhiteSpace(item.Note)
                    ? DBNull.Value
                    : item.Note.Trim();

                int affected = updateCmd.ExecuteNonQuery();
                if (affected == 0)
                {
                    throw new InvalidOperationException("Không thể cập nhật số lượng còn mượn của phiếu.");
                }
            }
        }

        private static int GetRemainingBorrowQuantity(SqlConnection conn, SqlTransaction tran, int ticketId)
        {
            using var cmd = new SqlCommand(
                "SELECT ISNULL(SUM(Quantity - ReturnedQuantity), 0) FROM BorrowDetails WHERE TicketID = @ticketId", conn, tran);
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}


