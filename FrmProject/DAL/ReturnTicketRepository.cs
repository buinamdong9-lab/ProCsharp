using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class ReturnTicketRepository : IReturnTicketRepository
    {
        private readonly IReturnRequestRepository _returnRequestRepository;

        public ReturnTicketRepository(IReturnRequestRepository returnRequestRepository)
        {
            _returnRequestRepository = returnRequestRepository;
        }
        public List<LookupItem> SearchBorrowingTickets(int currentUserId, AppRole appRole, string keyword = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.Query(
                "sp_SearchBorrowingTickets",
                new
                {
                    userId = currentUserId,
                    isUser = appRole == AppRole.User ? 1 : 0,
                    keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword
                },
                commandType: CommandType.StoredProcedure)
                .Select(row => new LookupItem((int)row.TicketID, (string)row.DisplayText))
                .ToList();
        }

        public ReturnTicketDetails? GetTicketDetails(int ticketId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            ReturnTicketDetails? details = conn.QueryFirstOrDefault<ReturnTicketDetails>(
                "sp_GetReturnTicketDetails",
                new { id = ticketId },
                commandType: CommandType.StoredProcedure);

            if (details == null)
                return null;

            DataTable dt = new();
            dt.Load(conn.ExecuteReader("sp_GetReturnTicketItems", new { id = ticketId }, commandType: CommandType.StoredProcedure));
            details.Items = dt;

            return details;
        }

        public void ApplyPendingReturnQuantities(int ticketId, DataTable dt)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            if (!_returnRequestRepository.TryLoadRequest(conn, null, ticketId, out _, out List<ReturnRequestItem> pendingItems))
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

        public bool TryLoadPendingReturnRequest(int ticketId, out DateTime requestedAt, out List<ReturnRequestItem> pendingItems)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            return _returnRequestRepository.TryLoadRequest(conn, null, ticketId, out requestedAt, out pendingItems);
        }

        public void SubmitReturnRequest(
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
                if (appRole == AppRole.User)
                {
                    int? ownerUserId = conn.QueryFirstOrDefault<int?>(
                        "sp_GetTicketOwnerId",
                        new { ticketId },
                        transaction: tran,
                        commandType: CommandType.StoredProcedure);
                    if (ownerUserId == null)
                        throw new InvalidOperationException("Phiếu mượn không tồn tại.");
                    if (ownerUserId.Value != currentUserId)
                        throw new InvalidOperationException("Bạn không có quyền thao tác trên phiếu này.");
                }

                _returnRequestRepository.SaveRequest(conn, tran, ticketId, requestedAt, returnItems, requestNote);

                string payload = ReturnRequestHelper.BuildPayload(requestedAt, returnItems);
                object? affected = conn.ExecuteScalar(
                    "sp_SubmitReturnRequestUpdate",
                    new
                    {
                        ticketId,
                        currentUserId,
                        isUser = appRole == AppRole.User ? 1 : 0,
                        requestNote,
                        payload,
                        returnPendingStatus = BorrowTicketStatus.ReturnPending,
                        borrowingStatus = BorrowTicketStatus.Borrowing
                    },
                    transaction: tran,
                    commandType: CommandType.StoredProcedure);

                if (affected == null || Convert.ToInt32(affected) == 0)
                    throw new InvalidOperationException("Phiếu không còn ở trạng thái đang mượn hoặc bạn không có quyền thao tác.");

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
