using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class TicketListRepository : ITicketListRepository
    {
        public List<TicketHistoryModel> SearchTickets(DateTime from, DateTime to, string keyword, string statusFilter, int currentUserId, AppRole appRole)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_SearchTickets",
                new
                {
                    from,
                    to,
                    keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword,
                    statusFilter = string.IsNullOrWhiteSpace(statusFilter) ? null : statusFilter,
                    currentUserId,
                    isUser = appRole == AppRole.User ? 1 : 0,
                    returnedStatus = BorrowTicketStatus.Returned,
                    rejectedStatus = BorrowTicketStatus.Rejected,
                    returnPendingStatus = BorrowTicketStatus.ReturnPending,
                    borrowingStatus = BorrowTicketStatus.Borrowing,
                    pendingStatus = BorrowTicketStatus.Pending
                },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new TicketHistoryModel
                {
                    TicketID = Convert.ToInt32(dict["ID phiếu"]),
                    TicketCode = dict["Số phiếu"]?.ToString() ?? "",
                    CreatedDate = dict["Ngày lập"]?.ToString() ?? "",
                    BorrowerName = dict["Người mượn"]?.ToString() ?? "",
                    RoomName = dict["Phòng"]?.ToString() ?? "",
                    Quantity = Convert.ToInt32(dict["Số lượng"]),
                    BorrowDate = dict["Ngày mượn"]?.ToString() ?? "",
                    ExpectedReturnDate = dict["Hạn trả"]?.ToString() ?? "",
                    ReturnDate = dict["Ngày trả"]?.ToString() ?? "",
                    Status = dict["Trạng thái"]?.ToString() ?? "",
                    Note = dict["Ghi chú"]?.ToString() ?? ""
                };
            }).ToList();
        }

        public TicketListStats GetStats(int currentUserId, AppRole appRole)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.QueryFirstOrDefault<TicketListStats>(
                "sp_GetTicketListStats",
                new
                {
                    currentUserId,
                    isUser = appRole == AppRole.User ? 1 : 0,
                    pendingStatus = BorrowTicketStatus.Pending,
                    borrowingStatus = BorrowTicketStatus.Borrowing,
                    returnPendingStatus = BorrowTicketStatus.ReturnPending
                },
                commandType: CommandType.StoredProcedure) ?? new TicketListStats();
        }

        public TicketDetailView? GetTicketDetail(int ticketId, int currentUserId, AppRole appRole)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            TicketDetailView? detail = conn.QueryFirstOrDefault<TicketDetailView>(
                "sp_GetTicketDetail",
                new
                {
                    ticketId,
                    currentUserId,
                    isUser = appRole == AppRole.User ? 1 : 0,
                    returnedStatus = BorrowTicketStatus.Returned,
                    rejectedStatus = BorrowTicketStatus.Rejected,
                    returnPendingStatus = BorrowTicketStatus.ReturnPending,
                    borrowingStatus = BorrowTicketStatus.Borrowing,
                    pendingStatus = BorrowTicketStatus.Pending
                },
                commandType: CommandType.StoredProcedure);

            if (detail == null)
                return null;

            var items = LoadBorrowDetailItems(conn, ticketId);
            if (items.Count == 0 && detail.StatusText == "Đã trả")
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

        private static List<TicketDetailItemModel> LoadBorrowDetailItems(SqlConnection conn, int ticketId)
        {
            var rows = conn.Query(
                "sp_LoadBorrowDetailItems",
                new { ticketId },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new TicketDetailItemModel
                {
                    DeviceCode = dict["Mã TB"]?.ToString() ?? "",
                    DeviceName = dict["Tên thiết bị"]?.ToString() ?? "",
                    Quantity = Convert.ToInt32(dict["Số lượng"]),
                    BorrowCondition = dict["Tình trạng khi mượn"]?.ToString() ?? "",
                    ReturnCondition = dict["Tình trạng khi trả"]?.ToString() ?? ""
                };
            }).ToList();
        }

        private static List<TicketDetailItemModel> LoadReturnedRequestItems(SqlConnection conn, int ticketId)
        {
            var rows = conn.Query(
                "sp_LoadReturnedRequestItems",
                new { ticketId },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new TicketDetailItemModel
                {
                    DeviceCode = dict["Mã TB"]?.ToString() ?? "",
                    DeviceName = dict["Tên thiết bị"]?.ToString() ?? "",
                    Quantity = Convert.ToInt32(dict["Số lượng"]),
                    BorrowCondition = dict["Tình trạng khi mượn"]?.ToString() ?? "",
                    ReturnCondition = dict["Tình trạng khi trả"]?.ToString() ?? ""
                };
            }).ToList();
        }
    }
}
