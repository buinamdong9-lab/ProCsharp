using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class ReportRepository : IReportRepository
    {
        public List<MonthlyStatsModel> GetMonthlyStats(DateTime from, DateTime to)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_GetReportMonthlyStats",
                new
                {
                    from,
                    to,
                    returnedStatus = BorrowTicketStatus.Returned,
                    borrowingStatus = BorrowTicketStatus.Borrowing,
                    returnPendingStatus = BorrowTicketStatus.ReturnPending
                },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new MonthlyStatsModel
                {
                    Month = dict["Tháng"]?.ToString() ?? "",
                    TicketCount = Convert.ToInt32(dict["Số phiếu mượn"]),
                    TotalBorrowedQty = Convert.ToInt32(dict["Tổng SL mượn"]),
                    ReturnedQty = Convert.ToInt32(dict["Đã trả"]),
                    OverdueQty = Convert.ToInt32(dict["Quá hạn"])
                };
            }).ToList();
        }

        public List<TopDeviceModel> GetTopDevices(DateTime from, DateTime to)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_GetReportTopDevices",
                new { from, to },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new TopDeviceModel
                {
                    Rank = Convert.ToInt32(dict["Hạng"]),
                    DeviceName = dict["Thiết bị"]?.ToString() ?? "",
                    BorrowCount = Convert.ToInt32(dict["Lượt mượn"]),
                    Ratio = dict["Tỉ lệ"]?.ToString() ?? ""
                };
            }).ToList();
        }

        public List<OverdueTicketModel> GetOverdueTickets(DateTime from, DateTime to)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_GetReportOverdueTickets",
                new
                {
                    from,
                    to,
                    borrowingStatus = BorrowTicketStatus.Borrowing,
                    returnPendingStatus = BorrowTicketStatus.ReturnPending
                },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new OverdueTicketModel
                {
                    TicketCode = dict["Số phiếu"]?.ToString() ?? "",
                    BorrowerName = dict["Người mượn"]?.ToString() ?? "",
                    RoomName = dict["Phòng"]?.ToString() ?? "",
                    BorrowDate = dict["Ngày mượn"]?.ToString() ?? "",
                    ExpectedReturnDate = dict["Hạn trả"]?.ToString() ?? "",
                    OverdueDays = Convert.ToInt32(dict["Số ngày quá hạn"]),
                    Status = dict["Tình trạng"]?.ToString() ?? ""
                };
            }).ToList();
        }
    }
}
