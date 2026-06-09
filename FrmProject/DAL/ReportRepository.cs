using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    internal static class ReportRepository
    {
        public static DataTable GetMonthlyStats(DateTime from, DateTime to)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            ReturnRequestRepository.EnsureSchema(conn);

            const string query = @"
                SELECT 
                    FORMAT(bt.BorrowDate, 'MM/yyyy') AS [Tháng],
                    COUNT(DISTINCT bt.TicketID) AS [Số phiếu mượn],
                    SUM(tQty.Qty) AS [Tổng SL mượn],
                    SUM(CASE WHEN bt.Status = @returnedStatus THEN tQty.Qty ELSE 0 END) AS [Đã trả],
                    SUM(CASE WHEN bt.Status = @borrowingStatus AND bt.ExpectedReturnDate < GETDATE() 
                        THEN tQty.Qty ELSE 0 END) AS [Quá hạn]
                FROM BorrowTickets bt
                CROSS APPLY (
                    SELECT COALESCE(
                        (SELECT SUM(bd2.Quantity) FROM BorrowDetails bd2 WHERE bd2.TicketID = bt.TicketID),
                        (SELECT SUM(rrd.ReturnQuantity)
                         FROM ReturnRequests rr
                         JOIN ReturnRequestDetails rrd ON rrd.ReturnRequestID = rr.ReturnRequestID
                         WHERE rr.TicketID = bt.TicketID),
                        0
                    ) AS Qty
                ) tQty
                WHERE bt.BorrowDate BETWEEN @from AND @to
                GROUP BY FORMAT(bt.BorrowDate, 'MM/yyyy'), YEAR(bt.BorrowDate), MONTH(bt.BorrowDate)
                ORDER BY YEAR(bt.BorrowDate), MONTH(bt.BorrowDate)";

            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            AddDateRangeParameters(da.SelectCommand, from, to);
            da.SelectCommand.Parameters.AddWithValue("@returnedStatus", BorrowTicketStatus.Returned);
            da.SelectCommand.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
            return Fill(da);
        }

        public static DataTable GetTopDevices(DateTime from, DateTime to)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            const string query = @"
                SELECT TOP 10
                    ROW_NUMBER() OVER (ORDER BY SUM(bd.Quantity) DESC) AS [Hạng],
                    d.DeviceName AS [Thiết bị],
                    SUM(bd.Quantity) AS [Lượt mượn],
                    ISNULL(CAST(ROUND(SUM(bd.Quantity) * 100.0 / 
                        NULLIF((SELECT ISNULL(SUM(bd2.Quantity), 0)
                                FROM BorrowDetails bd2
                                JOIN BorrowTickets bt2 ON bt2.TicketID = bd2.TicketID
                                WHERE bt2.BorrowDate BETWEEN @from AND @to), 0), 1) AS VARCHAR) + '%', '0%') AS [Tỉ lệ]
                FROM BorrowDetails bd
                JOIN BorrowTickets bt ON bt.TicketID = bd.TicketID
                JOIN Devices d ON d.DeviceID = bd.DeviceID
                WHERE bt.BorrowDate BETWEEN @from AND @to
                GROUP BY d.DeviceName
                ORDER BY [Lượt mượn] DESC";

            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            AddDateRangeParameters(da.SelectCommand, from, to);
            return Fill(da);
        }

        public static DataTable GetOverdueTickets(DateTime from, DateTime to)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            ReturnRequestRepository.EnsureSchema(conn);

            string ticketDisplayExpr = DbSchemaHelper.GetBorrowTicketDisplayExpressionForCast(conn, "bt");
            string statusCondition = DbSchemaHelper.BuildActiveBorrowStatusCondition("bt");

            string query = @"
                SELECT 
                    " + ticketDisplayExpr + @" AS [Số phiếu],
                    u.FullName AS [Người mượn],
                    ISNULL(roomInfo.RoomName, '') AS [Phòng],
                    CONVERT(VARCHAR, bt.BorrowDate, 103) AS [Ngày mượn],
                    CONVERT(VARCHAR, bt.ExpectedReturnDate, 103) AS [Hạn trả],
                    DATEDIFF(DAY, bt.ExpectedReturnDate, GETDATE()) AS [Số ngày quá hạn],
                    N'Cần xử lý' AS [Tình trạng]
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
                WHERE (" + statusCondition + @")
                  AND bt.BorrowDate BETWEEN @from AND @to
                  AND bt.ExpectedReturnDate < GETDATE()
                ORDER BY bt.ExpectedReturnDate";

            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            AddDateRangeParameters(da.SelectCommand, from, to);
            return Fill(da);
        }

        private static void AddDateRangeParameters(SqlCommand cmd, DateTime from, DateTime to)
        {
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);
        }

        private static DataTable Fill(SqlDataAdapter da)
        {
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}

