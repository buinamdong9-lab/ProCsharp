using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    internal sealed class DashboardSnapshot
    {
        public int TotalDevices { get; init; }
        public int BorrowedDevices { get; init; }
        public int OverdueTickets { get; init; }
        public int IssueDevices { get; init; }
        public int ReturnedToday { get; init; }
        public int TotalBorrowingCount { get; init; }
        public DataTable BorrowingList { get; init; } = new DataTable();
        public DataTable MonthlyBorrowStats { get; init; } = new DataTable();
    }

    internal static class DashboardRepository
    {
        public static DashboardSnapshot Load(int pageNumber = 1, int pageSize = 10)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            return new DashboardSnapshot
            {
                TotalDevices = Count(conn, "SELECT COUNT(*) FROM dbo.Devices"),
                BorrowedDevices = CountBorrowedDevices(conn),
                OverdueTickets = Count(conn, $"SELECT COUNT(*) FROM dbo.BorrowTickets WHERE {DbSchemaHelper.BuildActiveBorrowStatusCondition()} AND ExpectedReturnDate < GETDATE()"),
                IssueDevices = CountIssueDevices(conn),
                ReturnedToday = CountReturnedToday(conn),
                TotalBorrowingCount = CountTotalBorrowing(conn),
                BorrowingList = LoadBorrowingList(conn, pageNumber, pageSize),
                MonthlyBorrowStats = LoadMonthlyBorrowStats(conn)
            };
        }

        private static int Count(SqlConnection conn, string sql)
        {
            using SqlCommand cmd = new SqlCommand(sql, conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static int CountBorrowedDevices(SqlConnection conn)
        {
            string query = @"
                SELECT ISNULL(SUM(bd.Quantity - bd.ReturnedQuantity), 0)
                FROM dbo.BorrowTickets bt
                JOIN dbo.BorrowDetails bd ON bd.TicketID = bt.TicketID
                WHERE " + DbSchemaHelper.BuildActiveBorrowStatusCondition("bt");

            using SqlCommand cmd = new SqlCommand(query, conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static int CountTotalBorrowing(SqlConnection conn)
        {
            string query = @"
                SELECT COUNT(*)
                FROM dbo.BorrowTickets bt
                JOIN dbo.BorrowDetails bd ON bd.TicketID = bt.TicketID
                WHERE " + DbSchemaHelper.BuildActiveBorrowStatusCondition("bt") + @"
                  AND bd.Quantity > bd.ReturnedQuantity";

            using SqlCommand cmd = new SqlCommand(query, conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static int CountIssueDevices(SqlConnection conn)
        {
            if (!DbSchemaHelper.HasColumn(conn, "Devices", "Status"))
                return 0;

            using SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM dbo.Devices WHERE Status IN (@maintenanceStatus, @brokenStatus)", conn);
            cmd.Parameters.AddWithValue("@maintenanceStatus", DeviceStatus.Maintenance);
            cmd.Parameters.AddWithValue("@brokenStatus", "Hỏng");
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static int CountReturnedToday(SqlConnection conn)
        {
            if (!DbSchemaHelper.HasColumn(conn, "BorrowTickets", "ReturnDate"))
                return 0;

            using SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.BorrowTickets WHERE ReturnDate >= CAST(GETDATE() AS DATE)", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static DataTable LoadBorrowingListOnly(int pageNumber, int pageSize)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            return LoadBorrowingList(conn, pageNumber, pageSize);
        }

        private static DataTable LoadBorrowingList(SqlConnection conn, int pageNumber, int pageSize)
        {
            string deviceCodeExpression = DbSchemaHelper.HasColumn(conn, "BorrowDetails", "InstanceID") &&
                                          DbSchemaHelper.HasColumn(conn, "DeviceInstances", "AssetCode")
                ? "ISNULL(NULLIF(di.AssetCode, ''), ISNULL(NULLIF(d.DeviceCode, ''), 'TB' + CONVERT(VARCHAR, d.DeviceID)))"
                : "ISNULL(NULLIF(d.DeviceCode, ''), 'TB' + CONVERT(VARCHAR, d.DeviceID))";

            string instanceJoin = DbSchemaHelper.HasColumn(conn, "BorrowDetails", "InstanceID")
                ? "LEFT JOIN dbo.DeviceInstances di ON di.InstanceID = bd.InstanceID"
                : string.Empty;

            string query = @"
                SELECT 
                    " + deviceCodeExpression + @" AS [Mã TB],
                    d.DeviceName          AS [Tên Thiết Bị],
                    (bd.Quantity - bd.ReturnedQuantity) AS [Số lượng],
                    u.FullName            AS [Người mượn],
                    bt.BorrowDate         AS [Ngày Mượn],
                    bt.ExpectedReturnDate AS [Hạn Trả]
                FROM dbo.BorrowTickets bt
                JOIN dbo.Users u ON u.UserID = bt.UserID
                JOIN dbo.BorrowDetails bd ON bd.TicketID = bt.TicketID
                JOIN dbo.Devices d ON d.DeviceID = bd.DeviceID
                " + instanceJoin + @"
                WHERE " + DbSchemaHelper.BuildActiveBorrowStatusCondition("bt") + @"
                  AND bd.Quantity > bd.ReturnedQuantity
                ORDER BY bt.BorrowDate DESC
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
            da.SelectCommand.Parameters.AddWithValue("@limit", pageSize);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private static DataTable LoadMonthlyBorrowStats(SqlConnection conn)
        {
            using SqlDataAdapter da = new SqlDataAdapter(
                @"DECLARE @from DATE = DATEFROMPARTS(YEAR(DATEADD(MONTH, -5, GETDATE())), MONTH(DATEADD(MONTH, -5, GETDATE())), 1);

                  SELECT FORMAT(bt.BorrowDate, 'MM/yyyy') AS [Tháng],
                         COUNT(DISTINCT bt.TicketID) AS [Số phiếu],
                         ISNULL(SUM(bd.Quantity), 0) AS [Số thiết bị]
                  FROM dbo.BorrowTickets bt
                  LEFT JOIN dbo.BorrowDetails bd ON bd.TicketID = bt.TicketID
                  WHERE bt.BorrowDate >= @from
                  GROUP BY FORMAT(bt.BorrowDate, 'MM/yyyy'), YEAR(bt.BorrowDate), MONTH(bt.BorrowDate)
                  ORDER BY YEAR(bt.BorrowDate), MONTH(bt.BorrowDate)", conn);

            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}

