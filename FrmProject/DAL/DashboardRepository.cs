using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
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

        public List<DashboardBorrowingItemModel> LoadBorrowingListOnly(int pageNumber, int pageSize)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            return LoadBorrowingList(conn, pageNumber, pageSize);
        }

        private static List<DashboardBorrowingItemModel> LoadBorrowingList(SqlConnection conn, int pageNumber, int pageSize)
        {
            var rows = conn.Query(
                "sp_LoadDashboardBorrowingList",
                new
                {
                    offset = (pageNumber - 1) * pageSize,
                    limit = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new DashboardBorrowingItemModel
                {
                    DeviceCode = dict["Mã TB"]?.ToString() ?? "",
                    DeviceName = dict["Tên Thiết Bị"]?.ToString() ?? "",
                    Quantity = Convert.ToInt32(dict["Số lượng"]),
                    BorrowerName = dict["Người mượn"]?.ToString() ?? "",
                    BorrowDate = Convert.ToDateTime(dict["Ngày Mượn"]),
                    ExpectedReturnDate = Convert.ToDateTime(dict["Hạn Trả"])
                };
            }).ToList();
        }

        private static List<DashboardMonthlyStatsModel> LoadMonthlyBorrowStats(SqlConnection conn)
        {
            var rows = conn.Query("sp_LoadDashboardMonthlyStats", commandType: CommandType.StoredProcedure);
            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new DashboardMonthlyStatsModel
                {
                    Month = dict["Tháng"]?.ToString() ?? "",
                    TicketCount = Convert.ToInt32(dict["Số phiếu"]),
                    DeviceCount = Convert.ToInt32(dict["Số thiết bị"])
                };
            }).ToList();
        }
    }
}
