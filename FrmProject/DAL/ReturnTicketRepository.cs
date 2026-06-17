using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
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
                .Select(row => {
                    var dict = (IDictionary<string, object>)row;
                    string text = "";
                    if (dict.TryGetValue("DisplayText", out var val) && val != null)
                        text = val.ToString()!;
                    else if (dict.TryGetValue("DisplayName", out var val2) && val2 != null)
                        text = val2.ToString()!;
                    return new LookupItem((int)row.TicketID, text);
                })
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

            var rows = conn.Query(
                "sp_GetReturnTicketItems",
                new { id = ticketId },
                commandType: CommandType.StoredProcedure);

            details.Items = rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new ReturnTicketItemModel
                {
                    DeviceID = Convert.ToInt32(dict["DeviceID"]),
                    InstanceID = Convert.ToInt32(dict["InstanceID"]),
                    AssetCode = dict["Mã tài sản"]?.ToString() ?? "",
                    DeviceName = dict["Tên thiết bị"]?.ToString() ?? "",
                    BorrowQty = Convert.ToInt32(dict["SL mượn"]),
                    ReturnQty = Convert.ToInt32(dict["SL trả"]),
                    BorrowCondition = dict["Tình trạng khi mượn"]?.ToString() ?? "",
                    ReturnCondition = dict["Tình trạng khi trả"]?.ToString() ?? "",
                    Note = dict["Ghi chú"]?.ToString() ?? ""
                };
            }).ToList();

            return details;
        }

        public void ApplyPendingReturnQuantities(int ticketId, List<ReturnTicketItemModel> items)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            if (!_returnRequestRepository.TryLoadRequest(conn, null, ticketId, out _, out List<ReturnRequestItem> pendingItems))
                return;

            foreach (var row in items)
            {
                int deviceId = row.DeviceID;
                int instanceId = row.InstanceID;
                ReturnRequestItem? pendingItem = pendingItems.FirstOrDefault(item =>
                    item.DeviceID == deviceId && (item.InstanceID == 0 || item.InstanceID == instanceId));

                if (pendingItem != null)
                {
                    row.ReturnQty = pendingItem.ReturnQty;
                    if (!string.IsNullOrWhiteSpace(pendingItem.Note))
                    {
                        row.ReturnCondition = pendingItem.Note;
                        row.Note = pendingItem.Note;
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

        public void ApproveReturn(int ticketId, int approvedByUserId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_ApproveReturnRequest",
                new
                {
                    ticketId,
                    approvedByUserId,
                    returnPendingStatus = BorrowTicketStatus.ReturnPending,
                    returnedStatus = BorrowTicketStatus.Returned,
                    borrowingStatus = BorrowTicketStatus.Borrowing,
                    retiredStatus = DeviceStatus.Retired,
                    availableStatus = DeviceStatus.Available,
                    maintenanceStatus = DeviceStatus.Maintenance,
                    goodCondition = DeviceCondition.Good
                },
                commandType: CommandType.StoredProcedure);
        }

        public void RejectReturn(int ticketId, string reason)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_RejectReturnRequest",
                new
                {
                    ticketId,
                    borrowingStatus = BorrowTicketStatus.Borrowing,
                    returnPendingStatus = BorrowTicketStatus.ReturnPending,
                    reason
                },
                commandType: CommandType.StoredProcedure);
        }

        public List<PendingReturnTicketModel> GetPendingReturnTickets()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_GetPendingReturnTickets",
                new { status = BorrowTicketStatus.ReturnPending },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new PendingReturnTicketModel
                {
                    TicketID = Convert.ToInt32(dict["TicketID"]),
                    TicketCode = dict["Số phiếu"]?.ToString() ?? "",
                    BorrowerName = dict["Người mượn"]?.ToString() ?? "",
                    BorrowDate = dict["Ngày mượn"]?.ToString() ?? "",
                    ExpectedReturnDate = dict["Hạn trả"]?.ToString() ?? "",
                    Note = dict["Ghi chú"]?.ToString() ?? ""
                };
            }).ToList();
        }

        public string GetTicketStatus(SqlConnection conn, SqlTransaction? tran, int ticketId)
        {
            return conn.ExecuteScalar<string>(
                "sp_GetTicketStatus",
                new { ticketId },
                transaction: tran,
                commandType: CommandType.StoredProcedure) ?? string.Empty;
        }

        public void VerifyTicketOwnership(SqlConnection conn, SqlTransaction tran, int ticketId, int userId)
        {
            int? ownerUserId = conn.QueryFirstOrDefault<int?>(
                "sp_GetTicketOwnerId",
                new { ticketId },
                transaction: tran,
                commandType: CommandType.StoredProcedure);
            if (ownerUserId == null)
                throw new InvalidOperationException("Phiếu mượn không tồn tại.");
            if (ownerUserId.Value != userId)
                throw new InvalidOperationException("Bạn không có quyền thao tác trên phiếu này.");
        }
    }
}
