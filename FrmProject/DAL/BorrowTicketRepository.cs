using Microsoft.Data.SqlClient;

namespace FrmProject.DAL
{
    internal static class BorrowTicketRepository
    {
        public static List<LookupItem> GetEnabledUsers()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string enabledUserCondition = DbSchemaHelper.BuildEnabledUserWhereClause(conn, "u");
            using var cmd = new SqlCommand($"SELECT u.UserID, u.FullName FROM Users u WHERE {enabledUserCondition} ORDER BY u.FullName", conn);
            return ReadLookupItems(cmd);
        }

        public static List<LookupItem> GetRooms()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(
                @"SELECT RoomID, RoomName
                  FROM Rooms
                  WHERE ISNULL(Status, N'Hoạt động') <> @retiredStatus
                  ORDER BY RoomName", conn);
            cmd.Parameters.AddWithValue("@retiredStatus", RoomStatus.Retired);
            return ReadLookupItems(cmd);
        }

        public static List<LookupItem> GetBorrowableDevices()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(
                @"SELECT d.DeviceID, d.DeviceName + ' (' + ISNULL(d.DeviceCode, '') + ')' AS DisplayName
                  FROM Devices d
                  LEFT JOIN Rooms r ON r.RoomID = d.RoomID
                  WHERE d.Status NOT IN (@maintenanceStatus, @retiredStatus)
                    AND ISNULL(r.Status, N'Hoạt động') <> @retiredRoomStatus
                    AND EXISTS (
                        SELECT 1
                        FROM DeviceInstances di
                        WHERE di.DeviceID = d.DeviceID
                          AND di.Status = @availableStatus
                          AND ISNULL(NULLIF(di.Condition, ''), @goodCondition) = @goodCondition
                    )
                  ORDER BY d.DeviceName", conn);
            cmd.Parameters.AddWithValue("@maintenanceStatus", DeviceStatus.Maintenance);
            cmd.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
            cmd.Parameters.AddWithValue("@retiredRoomStatus", RoomStatus.Retired);
            cmd.Parameters.AddWithValue("@availableStatus", DeviceStatus.Available);
            cmd.Parameters.AddWithValue("@goodCondition", DeviceCondition.Good);
            return ReadLookupItems(cmd);
        }

        public static List<LookupItem> GetAvailableInstances(int deviceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(
                @"SELECT InstanceID, AssetCode
                  FROM DeviceInstances
                  WHERE DeviceID = @devId
                    AND Status = @availableStatus
                    AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
                  ORDER BY AssetCode", conn);
            cmd.Parameters.AddWithValue("@devId", deviceId);
            cmd.Parameters.AddWithValue("@availableStatus", DeviceStatus.Available);
            cmd.Parameters.AddWithValue("@goodCondition", DeviceCondition.Good);
            return ReadLookupItems(cmd);
        }

        public static int CreatePendingTicket(BorrowTicketDraft draft)
        {
            if (draft.Items.Count == 0)
                throw new InvalidOperationException("Phiếu mượn phải có ít nhất một cá thể thiết bị.");

            if (draft.Items.Any(item => item.InstanceId <= 0 || item.Quantity != 1))
                throw new InvalidOperationException("Mỗi dòng phiếu mượn phải gắn với một cá thể thiết bị hợp lệ và có số lượng bằng 1.");

            if (draft.Items.Select(item => item.InstanceId).Distinct().Count() != draft.Items.Count)
                throw new InvalidOperationException("Một cá thể thiết bị không được xuất hiện nhiều lần trong cùng phiếu.");

            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();

            try
            {
                DbSchemaHelper.EnsureBorrowTicketStatusConstraint(conn, tran);
                DbSchemaHelper.EnsureBorrowTicketNoteCapacity(conn, tran);

                bool hasTicketCode = DbSchemaHelper.HasColumn(conn, "BorrowTickets", "TicketCode", tran);
                bool hasPurpose = DbSchemaHelper.HasColumn(conn, "BorrowTickets", "Purpose", tran);
                bool hasCreatedBy = DbSchemaHelper.HasColumn(conn, "BorrowTickets", "CreatedBy", tran);
                bool hasBorrowDetailInstance = DbSchemaHelper.HasColumn(conn, "BorrowDetails", "InstanceID", tran);
                if (!hasBorrowDetailInstance)
                    throw new InvalidOperationException("Cơ sở dữ liệu chưa hỗ trợ quản lý cá thể thiết bị. Vui lòng chạy migration trước.");

                List<string> ticketColumns = new() { "UserID" };
                List<string> ticketValues = new() { "@userID" };

                if (hasCreatedBy)
                {
                    ticketColumns.Add("CreatedBy");
                    ticketValues.Add("@createdBy");
                }

                if (hasTicketCode)
                {
                    ticketColumns.Add("TicketCode");
                    ticketValues.Add("@ticketCode");
                }

                ticketColumns.Add("BorrowDate");
                ticketValues.Add("@borrowDate");
                ticketColumns.Add("ExpectedReturnDate");
                ticketValues.Add("@returnDate");

                if (hasPurpose)
                {
                    ticketColumns.Add("Purpose");
                    ticketValues.Add("@purpose");
                }

                ticketColumns.Add("Status");
                ticketValues.Add("@status");
                ticketColumns.Add("Note");
                ticketValues.Add("@note");

                string sqlTicket = $@"INSERT INTO BorrowTickets ({string.Join(", ", ticketColumns)})
                                      VALUES ({string.Join(", ", ticketValues)});
                                      SELECT SCOPE_IDENTITY();";

                int ticketId;
                using (var cmd = new SqlCommand(sqlTicket, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@userID", draft.BorrowerId);
                    if (hasCreatedBy)
                        cmd.Parameters.AddWithValue("@createdBy", draft.BorrowerId);
                    if (hasTicketCode)
                        cmd.Parameters.AddWithValue("@ticketCode", draft.TicketCode);
                    cmd.Parameters.AddWithValue("@borrowDate", draft.BorrowDate);
                    cmd.Parameters.AddWithValue("@returnDate", draft.ExpectedReturnDate);
                    if (hasPurpose)
                        cmd.Parameters.AddWithValue("@purpose", draft.Purpose);
                    cmd.Parameters.AddWithValue("@status", BorrowTicketStatus.Pending);
                    cmd.Parameters.AddWithValue("@note", draft.Note);
                    ticketId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                foreach (BorrowTicketDraftItem item in draft.Items)
                {
                    const string sqlDetail =
                        @"INSERT INTO BorrowDetails (TicketID, DeviceID, InstanceID, Quantity)
                          SELECT @ticketID, @deviceID, @instanceID, @qty
                          WHERE EXISTS (
                              SELECT 1
                              FROM DeviceInstances WITH (UPDLOCK, HOLDLOCK)
                              WHERE InstanceID = @instanceID
                                AND DeviceID = @deviceID
                                AND Status = @availableStatus
                                AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
                          )
                            AND NOT EXISTS (
                              SELECT 1
                              FROM BorrowDetails existing
                              JOIN BorrowTickets ticket ON ticket.TicketID = existing.TicketID
                              WHERE existing.InstanceID = @instanceID
                                AND ticket.Status IN (@pendingStatus, @borrowingStatus, @returnPendingStatus)
                            );";

                    using var cmd = new SqlCommand(sqlDetail, conn, tran);
                    cmd.Parameters.AddWithValue("@ticketID", ticketId);
                    cmd.Parameters.AddWithValue("@deviceID", item.DeviceId);
                    cmd.Parameters.AddWithValue("@instanceID", item.InstanceId);
                    cmd.Parameters.AddWithValue("@qty", item.Quantity);
                    cmd.Parameters.AddWithValue("@availableStatus", DeviceStatus.Available);
                    cmd.Parameters.AddWithValue("@goodCondition", DeviceCondition.Good);
                    cmd.Parameters.AddWithValue("@pendingStatus", BorrowTicketStatus.Pending);
                    cmd.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
                    cmd.Parameters.AddWithValue("@returnPendingStatus", BorrowTicketStatus.ReturnPending);

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        throw new InvalidOperationException(
                            "Một cá thể đã được chọn ở phiếu khác hoặc không còn ở trạng thái Có sẵn/Tốt.");
                    }
                }

                tran.Commit();
                return ticketId;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private static List<LookupItem> ReadLookupItems(SqlCommand cmd)
        {
            List<LookupItem> items = new();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                items.Add(new LookupItem(reader.GetInt32(0), reader.GetString(1)));

            return items;
        }
    }
}

