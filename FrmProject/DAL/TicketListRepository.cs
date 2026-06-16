using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class TicketListRepository : ITicketListRepository
    {
        public DataTable SearchTickets(DateTime from, DateTime to, string keyword, string statusFilter, int currentUserId, AppRole appRole)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            DataTable dt = new DataTable();
            dt.Load(conn.ExecuteReader(
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
                commandType: CommandType.StoredProcedure));
            return dt;
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
            items.Load(conn.ExecuteReader(
                "sp_LoadBorrowDetailItems",
                new { ticketId },
                commandType: CommandType.StoredProcedure));
            return items;
        }

        private static DataTable LoadReturnedRequestItems(SqlConnection conn, int ticketId)
        {
            DataTable items = new DataTable();
            items.Load(conn.ExecuteReader(
                "sp_LoadReturnedRequestItems",
                new { ticketId },
                commandType: CommandType.StoredProcedure));
            return items;
        }
    }
}
