using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class DashboardRepository : IDashboardRepository
    {
        public DashboardSnapshot Load(int pageNumber = 1, int pageSize = 10)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            var counts = conn.QueryFirstOrDefault(
                "sp_GetDashboardCounts",
                new
                {
                    maintenanceStatus = DeviceStatus.Maintenance,
                    brokenStatus = "Hỏng"
                },
                commandType: CommandType.StoredProcedure);

            var borrowingList = LoadBorrowingList(conn, pageNumber, pageSize);
            var monthlyBorrowStats = LoadMonthlyBorrowStats(conn);

            if (counts == null)
            {
                return new DashboardSnapshot
                {
                    BorrowingList = borrowingList,
                    MonthlyBorrowStats = monthlyBorrowStats
                };
            }

            return new DashboardSnapshot
            {
                TotalDevices = counts.TotalDevices ?? 0,
                BorrowedDevices = counts.BorrowedDevices ?? 0,
                OverdueTickets = counts.OverdueTickets ?? 0,
                IssueDevices = counts.IssueDevices ?? 0,
                ReturnedToday = counts.ReturnedToday ?? 0,
                TotalBorrowingCount = counts.TotalBorrowingCount ?? 0,
                BorrowingList = borrowingList,
                MonthlyBorrowStats = monthlyBorrowStats
            };
        }

        public DataTable LoadBorrowingListOnly(int pageNumber, int pageSize)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            return LoadBorrowingList(conn, pageNumber, pageSize);
        }

        private static DataTable LoadBorrowingList(SqlConnection conn, int pageNumber, int pageSize)
        {
            DataTable dt = new DataTable();
            dt.Load(conn.ExecuteReader(
                "sp_LoadDashboardBorrowingList",
                new
                {
                    offset = (pageNumber - 1) * pageSize,
                    limit = pageSize
                },
                commandType: CommandType.StoredProcedure));
            return dt;
        }

        private static DataTable LoadMonthlyBorrowStats(SqlConnection conn)
        {
            DataTable dt = new DataTable();
            dt.Load(conn.ExecuteReader("sp_LoadDashboardMonthlyStats", commandType: CommandType.StoredProcedure));
            return dt;
        }
    }
}
