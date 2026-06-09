using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    internal sealed class TicketListStats
    {
        public int Total { get; init; }
        public int Pending { get; init; }
        public int Active { get; init; }
        public int Overdue { get; init; }
    }

    internal sealed class TicketDetailView
    {
        public string TicketDisplay { get; init; } = string.Empty;
        public string BorrowerName { get; init; } = string.Empty;
        public string RoomName { get; init; } = string.Empty;
        public string Purpose { get; init; } = string.Empty;
        public DateTime BorrowDate { get; init; }
        public DateTime ExpectedReturnDate { get; init; }
        public string ApprovedByName { get; init; } = string.Empty;
        public DateTime? ReturnDate { get; init; }
        public string StatusText { get; init; } = string.Empty;
        public int OverdueDays { get; init; }
        public DataTable Items { get; init; } = new();
    }

    internal static class TicketListRepository
    {
        public static DataTable SearchTickets(DateTime from, DateTime to, string keyword, string statusFilter, int currentUserId, AppRole appRole)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string returnDateColumn = DbSchemaHelper.GetReturnDateColumnExpression(conn, "bt");
            string quantityExpr = HasReturnHistoryTables(conn)
                ? @"COALESCE(
                        (SELECT SUM(bd2.Quantity) FROM BorrowDetails bd2 WHERE bd2.TicketID = bt.TicketID),
                        (SELECT SUM(rrd.ReturnQuantity)
                         FROM ReturnRequests rr
                         JOIN ReturnRequestDetails rrd ON rrd.ReturnRequestID = rr.ReturnRequestID
                         WHERE rr.TicketID = bt.TicketID),
                        0)"
                : "ISNULL((SELECT SUM(bd2.Quantity) FROM BorrowDetails bd2 WHERE bd2.TicketID = bt.TicketID), 0)";
            string ticketDisplayExpr = DbSchemaHelper.GetBorrowTicketDisplayExpressionForCast(conn, "bt");

            string query = $@"
                SELECT 
                    bt.TicketID AS [ID phiếu],
                    {ticketDisplayExpr} AS [Số phiếu],
                    CONVERT(VARCHAR, bt.BorrowDate, 103) AS [Ngày lập],
                    u.FullName AS [Người mượn],
                    ISNULL(roomInfo.RoomName, '') AS [Phòng],
                    {quantityExpr} AS [Số lượng],
                    CONVERT(VARCHAR, bt.BorrowDate, 103) AS [Ngày mượn],
                    CONVERT(VARCHAR, bt.ExpectedReturnDate, 103) AS [Hạn trả],
                    CASE WHEN {returnDateColumn} IS NOT NULL 
                         THEN CONVERT(VARCHAR, {returnDateColumn}, 103) 
                         ELSE '' END AS [Ngày trả],
                    CASE 
                        WHEN bt.Status = @returnedStatus OR {returnDateColumn} IS NOT NULL THEN N'Đã trả'
                        WHEN bt.Status = @rejectedStatus THEN N'Từ chối'
                        WHEN bt.Status = @returnPendingStatus THEN N'Chờ duyệt trả'
                        WHEN bt.Status = @borrowingStatus AND bt.ExpectedReturnDate < GETDATE() THEN N'Quá hạn'
                        WHEN bt.Status = @borrowingStatus THEN N'Đang mượn'
                        WHEN bt.Status = @pendingStatus THEN N'Chờ duyệt'
                        ELSE bt.Status
                    END AS [Trạng thái],
                    bt.Note AS [Ghi chú]
                FROM BorrowTickets bt
                JOIN Users u ON u.UserID = bt.UserID
                OUTER APPLY (
                    SELECT TOP 1 ISNULL(r.RoomName, '') AS RoomName
                    FROM BorrowDetails bd
                    JOIN Devices d ON d.DeviceID = bd.DeviceID
                    LEFT JOIN Rooms r ON r.RoomID = d.RoomID
                    WHERE bd.TicketID = bt.TicketID
                    ORDER BY r.RoomName
                ) roomInfo
                WHERE bt.BorrowDate BETWEEN @from AND @to";

            if (appRole == AppRole.User)
                query += " AND bt.UserID = @currentUserId";

            if (!string.IsNullOrEmpty(keyword))
                query += appRole == AppRole.User
                    ? " AND (CAST(bt.TicketID AS VARCHAR) LIKE @kw OR ISNULL(bt.TicketCode, '') LIKE @kw)"
                    : " AND (CAST(bt.TicketID AS VARCHAR) LIKE @kw OR ISNULL(bt.TicketCode, '') LIKE @kw OR u.FullName LIKE @kw)";

            query += BuildStatusFilter(statusFilter);
            query += " ORDER BY bt.BorrowDate DESC";

            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            AddStatusParameters(da.SelectCommand);
            da.SelectCommand.Parameters.AddWithValue("@from", from);
            da.SelectCommand.Parameters.AddWithValue("@to", to);
            if (appRole == AppRole.User)
                da.SelectCommand.Parameters.AddWithValue("@currentUserId", currentUserId);
            if (!string.IsNullOrEmpty(keyword))
                da.SelectCommand.Parameters.AddWithValue("@kw", $"%{keyword}%");

            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static TicketListStats GetStats(int currentUserId, AppRole appRole)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            return new TicketListStats
            {
                Total = Count(conn, "1 = 1", currentUserId, appRole),
                Pending = Count(conn, "Status = @pendingStatus", currentUserId, appRole),
                Active = Count(conn, DbSchemaHelper.BuildActiveBorrowStatusCondition(), currentUserId, appRole),
                Overdue = Count(conn, DbSchemaHelper.BuildActiveBorrowStatusCondition() + " AND ExpectedReturnDate < GETDATE()", currentUserId, appRole)
            };
        }

        public static TicketDetailView? GetTicketDetail(int ticketId, int currentUserId, AppRole appRole)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            string ticketDisplayExpr = DbSchemaHelper.GetBorrowTicketDisplayExpressionForCast(conn, "bt");
            string purposeExpr = DbSchemaHelper.HasColumn(conn, "BorrowTickets", "Purpose")
                ? "ISNULL(bt.Purpose, '')"
                : "CAST('' AS NVARCHAR(255))";
            string returnedByJoin = DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnedBy")
                ? "LEFT JOIN Users approver ON approver.UserID = bt.ReturnedBy"
                : "LEFT JOIN Users approver ON 1 = 0";

            string returnDateColumn = DbSchemaHelper.GetReturnDateColumnExpression(conn, "bt");

            string query = $@"
                SELECT {ticketDisplayExpr} AS TicketDisplay,
                       u.FullName AS BorrowerName,
                       ISNULL(roomInfo.RoomName, '') AS RoomName,
                       {purposeExpr} AS Purpose,
                       bt.BorrowDate,
                       bt.ExpectedReturnDate,
                       ISNULL(approver.FullName, '') AS ApprovedByName,
                       {returnDateColumn} AS ReturnDate,
                       CASE
                           WHEN bt.Status = @returnedStatus OR {returnDateColumn} IS NOT NULL THEN N'Đã trả'
                           WHEN bt.Status = @rejectedStatus THEN N'Từ chối'
                           WHEN bt.Status = @returnPendingStatus THEN N'Chờ duyệt trả'
                           WHEN bt.Status = @borrowingStatus AND bt.ExpectedReturnDate < GETDATE() THEN N'Quá hạn'
                           WHEN bt.Status = @borrowingStatus THEN N'Đang mượn'
                           WHEN bt.Status = @pendingStatus THEN N'Chờ duyệt'
                           ELSE bt.Status
                       END AS StatusText,
                       CASE
                           WHEN bt.ExpectedReturnDate < GETDATE()
                            AND bt.Status IN (@borrowingStatus, @returnPendingStatus)
                           THEN DATEDIFF(DAY, bt.ExpectedReturnDate, GETDATE())
                           ELSE 0
                       END AS OverdueDays
                FROM BorrowTickets bt
                JOIN Users u ON u.UserID = bt.UserID
                {returnedByJoin}
                OUTER APPLY (
                    SELECT TOP 1 ISNULL(r.RoomName, '') AS RoomName
                    FROM BorrowDetails bd
                    JOIN Devices d ON d.DeviceID = bd.DeviceID
                    LEFT JOIN Rooms r ON r.RoomID = d.RoomID
                    WHERE bd.TicketID = bt.TicketID
                    ORDER BY r.RoomName
                ) roomInfo
                WHERE bt.TicketID = @ticketId";

            if (appRole == AppRole.User)
                query += " AND bt.UserID = @currentUserId";

            TicketDetailView? detail = null;
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                AddStatusParameters(cmd);
                cmd.Parameters.AddWithValue("@ticketId", ticketId);
                if (appRole == AppRole.User)
                    cmd.Parameters.AddWithValue("@currentUserId", currentUserId);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    detail = new TicketDetailView
                    {
                        TicketDisplay = reader.GetString(0),
                        BorrowerName = reader.GetString(1),
                        RoomName = reader.GetString(2),
                        Purpose = reader.GetString(3),
                        BorrowDate = reader.GetDateTime(4),
                        ExpectedReturnDate = reader.GetDateTime(5),
                        ApprovedByName = reader.GetString(6),
                        ReturnDate = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                        StatusText = reader.GetString(8),
                        OverdueDays = reader.GetInt32(9)
                    };
                }
            }

            if (detail == null)
                return null;

            DataTable items = LoadBorrowDetailItems(conn, ticketId);
            if (items.Rows.Count == 0 && detail.StatusText == "Đã trả")
                items = LoadReturnedRequestItems(conn, ticketId);

            return new TicketDetailView
            {
                TicketDisplay = detail.TicketDisplay,
                BorrowerName = detail.BorrowerName,
                RoomName = detail.RoomName,
                Purpose = detail.Purpose,
                BorrowDate = detail.BorrowDate,
                ExpectedReturnDate = detail.ExpectedReturnDate,
                ApprovedByName = detail.ApprovedByName,
                ReturnDate = detail.ReturnDate,
                StatusText = detail.StatusText,
                OverdueDays = detail.OverdueDays,
                Items = items
            };
        }

        private static DataTable LoadBorrowDetailItems(SqlConnection conn, int ticketId)
        {
            DataTable items = new DataTable();
            string returnConditionSelect = HasReturnHistoryTables(conn)
                ? @"ISNULL(returnInfo.ReturnCondition, '') AS [Tình trạng khi trả]"
                : @"CAST('' AS NVARCHAR(255)) AS [Tình trạng khi trả]";
            string returnConditionApply = HasReturnHistoryTables(conn)
                ? @"OUTER APPLY (
                        SELECT TOP 1 ISNULL(rd.Note, '') AS ReturnCondition
                        FROM ReturnRequests rr
                        JOIN ReturnRequestDetails rd ON rd.ReturnRequestID = rr.ReturnRequestID
                        WHERE rr.TicketID = bd.TicketID
                          AND rd.DeviceID = bd.DeviceID
                          AND ISNULL(rd.InstanceID, 0) = ISNULL(bd.InstanceID, 0)
                        ORDER BY rd.ReturnRequestDetailID DESC
                    ) returnInfo"
                : string.Empty;

            using SqlDataAdapter da = new SqlDataAdapter(
                $@"SELECT ISNULL(di.AssetCode, ISNULL(d.DeviceCode, '')) AS [Mã TB],
                         d.DeviceName AS [Tên thiết bị],
                         bd.Quantity AS [OriginalQuantity],
                         bd.Quantity AS [Số lượng],
                         ISNULL(NULLIF(di.Condition, ''), N'Tốt') AS [Tình trạng khi mượn],
                         {returnConditionSelect}
                  FROM BorrowDetails bd
                  JOIN Devices d ON d.DeviceID = bd.DeviceID
                  LEFT JOIN DeviceInstances di ON di.InstanceID = bd.InstanceID
                  {returnConditionApply}
                  WHERE bd.TicketID = @ticketId
                  ORDER BY d.DeviceName, di.AssetCode", conn);
            da.SelectCommand.Parameters.AddWithValue("@ticketId", ticketId);
            da.Fill(items);
            return items;
        }

        private static DataTable LoadReturnedRequestItems(SqlConnection conn, int ticketId)
        {
            DataTable items = new DataTable();
            if (!HasReturnHistoryTables(conn))
                return items;

            using SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT ISNULL(di.AssetCode, ISNULL(d.DeviceCode, '')) AS [Mã TB],
                         d.DeviceName AS [Tên thiết bị],
                         rd.ReturnQuantity AS [Số lượng],
                         ISNULL(NULLIF(di.Condition, ''), N'Tốt') AS [Tình trạng khi mượn],
                         ISNULL(NULLIF(rd.Note, ''), N'Tốt') AS [Tình trạng khi trả]
                  FROM ReturnRequests rr
                  JOIN ReturnRequestDetails rd ON rd.ReturnRequestID = rr.ReturnRequestID
                  JOIN Devices d ON d.DeviceID = rd.DeviceID
                  LEFT JOIN DeviceInstances di ON di.InstanceID = rd.InstanceID
                  WHERE rr.TicketID = @ticketId
                  ORDER BY d.DeviceName, di.AssetCode", conn);
            da.SelectCommand.Parameters.AddWithValue("@ticketId", ticketId);
            da.Fill(items);
            return items;
        }

        private static bool HasReturnHistoryTables(SqlConnection conn)
        {
            using SqlCommand cmd = new SqlCommand(
                @"SELECT CASE
                    WHEN OBJECT_ID('dbo.ReturnRequests', 'U') IS NOT NULL
                     AND OBJECT_ID('dbo.ReturnRequestDetails', 'U') IS NOT NULL
                    THEN 1 ELSE 0 END", conn);
            return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
        }

        private static int Count(SqlConnection conn, string condition, int currentUserId, AppRole appRole)
        {
            string sql = "SELECT COUNT(*) FROM BorrowTickets WHERE " + condition;
            if (appRole == AppRole.User)
                sql += " AND UserID = @currentUserId";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@pendingStatus", BorrowTicketStatus.Pending);
            if (appRole == AppRole.User)
                cmd.Parameters.AddWithValue("@currentUserId", currentUserId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static string BuildStatusFilter(string statusFilter) =>
            statusFilter switch
            {
                "Đang mượn" => " AND bt.Status = @borrowingStatus AND bt.ExpectedReturnDate >= GETDATE()",
                "Quá hạn" => " AND bt.Status = @borrowingStatus AND bt.ExpectedReturnDate < GETDATE()",
                "Đã trả" => " AND bt.Status = @returnedStatus",
                "Chờ duyệt" => " AND bt.Status = @pendingStatus",
                "Chờ duyệt trả" => " AND bt.Status = @returnPendingStatus",
                "Từ chối" => " AND bt.Status = @rejectedStatus",
                _ => string.Empty
            };

        private static void AddStatusParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@returnedStatus", BorrowTicketStatus.Returned);
            cmd.Parameters.AddWithValue("@rejectedStatus", BorrowTicketStatus.Rejected);
            cmd.Parameters.AddWithValue("@returnPendingStatus", BorrowTicketStatus.ReturnPending);
            cmd.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
            cmd.Parameters.AddWithValue("@pendingStatus", BorrowTicketStatus.Pending);
        }
    }
}

