using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    internal static class ReturnTicketRepository
    {
        public static List<LookupItem> SearchBorrowingTickets(int currentUserId, AppRole appRole, string keyword = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string ticketDisplayExpr = DbSchemaHelper.GetBorrowTicketDisplayExpression(conn, "bt");
            bool hasTicketCode = DbSchemaHelper.HasColumn(conn, "BorrowTickets", "TicketCode");
            string searchCondition = string.Empty;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                searchCondition = hasTicketCode
                    ? " AND (CONVERT(VARCHAR, bt.TicketID) LIKE @kw OR bt.TicketCode LIKE @kw OR u.FullName LIKE @kw)"
                    : " AND (CONVERT(VARCHAR, bt.TicketID) LIKE @kw OR u.FullName LIKE @kw)";
            }

            using var cmd = new SqlCommand(
                $@"SELECT bt.TicketID, {ticketDisplayExpr} + ' - ' + u.FullName + ' (' + 
                   CONVERT(VARCHAR, bt.BorrowDate, 103) + ')'
                   FROM BorrowTickets bt
                   JOIN Users u ON u.UserID = bt.UserID
                   WHERE {DbSchemaHelper.BuildActiveBorrowStatusCondition("bt")}
                     AND (@isUser = 0 OR bt.UserID = @userId)
                     {searchCondition}
                   ORDER BY bt.BorrowDate DESC", conn);
            cmd.Parameters.AddWithValue("@isUser", appRole == AppRole.User ? 1 : 0);
            cmd.Parameters.AddWithValue("@userId", currentUserId);
            if (!string.IsNullOrWhiteSpace(keyword))
                cmd.Parameters.AddWithValue("@kw", $"%{keyword}%");

            List<LookupItem> tickets = new();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                tickets.Add(new LookupItem(reader.GetInt32(0), reader.GetString(1)));

            return tickets;
        }

        public static ReturnTicketDetails? GetTicketDetails(int ticketId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string ticketDisplayExpr = DbSchemaHelper.GetBorrowTicketDisplayExpressionForCast(conn, "bt");
            string returnNoteExpr = DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnNote")
                ? "bt.ReturnNote"
                : "CAST(NULL AS NVARCHAR(MAX))";

            ReturnTicketDetails? details = null;
            using (var cmd = new SqlCommand(
                $@"SELECT {ticketDisplayExpr}, u.FullName, bt.BorrowDate, bt.ExpectedReturnDate, bt.Status, {returnNoteExpr}
                   FROM BorrowTickets bt
                   JOIN Users u ON u.UserID = bt.UserID
                   WHERE bt.TicketID = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", ticketId);
                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    details = new ReturnTicketDetails
                    {
                        TicketDisplay = reader.GetString(0),
                        BorrowerName = reader.GetString(1),
                        BorrowDate = reader.GetDateTime(2),
                        ExpectedReturnDate = reader.GetDateTime(3),
                        Status = reader.GetString(4),
                        ReturnNote = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                    };
                }
            }

            if (details == null)
                return null;

            string query = @"
                SELECT bd.DeviceID,
                       bd.InstanceID,
                       di.AssetCode AS [Mã tài sản],
                       d.DeviceName AS [Tên thiết bị],
                       (bd.Quantity - bd.ReturnedQuantity) AS [SL mượn],
                       (bd.Quantity - bd.ReturnedQuantity) AS [SL trả],
                       ISNULL(NULLIF(di.Condition, ''), N'Tốt') AS [Tình trạng khi mượn],
                       N'Tốt' AS [Tình trạng khi trả],
                       ISNULL(bd.Note, '') AS [Ghi chú]
                FROM BorrowDetails bd
                JOIN Devices d ON d.DeviceID = bd.DeviceID
                LEFT JOIN DeviceInstances di ON di.InstanceID = bd.InstanceID
                WHERE bd.TicketID = @id AND bd.Quantity > bd.ReturnedQuantity";

            using var da = new SqlDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@id", ticketId);
            DataTable dt = new();
            da.Fill(dt);
            details.Items = dt;
            return details;
        }

        public static void ApplyPendingReturnQuantities(int ticketId, DataTable dt)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            if (!ReturnRequestRepository.TryLoadRequest(conn, null, ticketId, out _, out List<ReturnRequestItem> pendingItems))
                return;

            foreach (DataRow row in dt.Rows)
            {
                int deviceId = Convert.ToInt32(row["DeviceID"]);
                int instanceId = row["InstanceID"] == DBNull.Value ? 0 : Convert.ToInt32(row["InstanceID"]);
                ReturnRequestItem? pendingItem = pendingItems.FirstOrDefault(item =>
                    item.DeviceID == deviceId && (item.InstanceID == 0 || item.InstanceID == instanceId));

                if (pendingItem != null)
                {
                    row["SL trả"] = pendingItem.ReturnQty;
                    if (!string.IsNullOrWhiteSpace(pendingItem.Note))
                    {
                        if (dt.Columns.Contains("Tình trạng khi trả"))
                            row["Tình trạng khi trả"] = pendingItem.Note;
                        row["Ghi chú"] = pendingItem.Note;
                    }
                }
            }
        }

        public static bool TryLoadPendingReturnRequest(int ticketId, out DateTime requestedAt, out List<ReturnRequestItem> pendingItems)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            return ReturnRequestRepository.TryLoadRequest(conn, null, ticketId, out requestedAt, out pendingItems);
        }

        public static void SubmitReturnRequest(
            int ticketId,
            int currentUserId,
            AppRole appRole,
            DateTime requestedAt,
            List<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)> returnItems,
            string requestNote)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();

            try
            {
                DbSchemaHelper.EnsureBorrowTicketStatusConstraint(conn, tran);
                DbSchemaHelper.EnsureBorrowTicketNoteCapacity(conn, tran);

                if (appRole == AppRole.User)
                    ReturnApprovalService.VerifyTicketOwnership(conn, tran, ticketId, currentUserId);

                ReturnRequestRepository.SaveRequest(conn, tran, ticketId, requestedAt, returnItems, requestNote);

                bool hasReturnNote = DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnNote", tran);
                string payload = ReturnRequestHelper.BuildPayload(requestedAt, returnItems);
                using var cmd = new SqlCommand(BuildReturnRequestSql(hasReturnNote, appRole), conn, tran);
                cmd.Parameters.AddWithValue("@id", ticketId);
                cmd.Parameters.AddWithValue("@note", requestNote);
                cmd.Parameters.AddWithValue("@returnPendingStatus", BorrowTicketStatus.ReturnPending);
                cmd.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
                if (hasReturnNote)
                    cmd.Parameters.AddWithValue("@returnPayload", payload);
                if (appRole == AppRole.User)
                    cmd.Parameters.AddWithValue("@userId", currentUserId);

                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    throw new InvalidOperationException("Phiếu không còn ở trạng thái đang mượn hoặc bạn không có quyền thao tác.");

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private static string BuildReturnRequestSql(bool hasReturnNote, AppRole appRole)
        {
            List<string> updates = new() { "Status = @returnPendingStatus" };

            if (hasReturnNote)
                updates.Add("ReturnNote = @returnPayload");

            updates.Add("Note = CASE WHEN ISNULL(Note, '') = '' THEN @note ELSE ISNULL(Note, '') + ' | ' + @note END");

            string whereClause = "TicketID = @id AND Status = @borrowingStatus";
            if (appRole == AppRole.User)
                whereClause += " AND UserID = @userId";

            return "UPDATE BorrowTickets SET " + string.Join(", ", updates) + " WHERE " + whereClause;
        }
    }
}

