using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class BorrowTicketRepository : IBorrowTicketRepository
    {
        public List<LookupItem> GetEnabledUsers()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.Query(
                "sp_GetEnabledUsers",
                commandType: CommandType.StoredProcedure)
                .Select(row => new LookupItem((int)row.UserID, (string)row.FullName))
                .ToList();
        }

        public List<LookupItem> GetRooms()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.Query(
                "sp_GetRooms",
                new { retiredStatus = RoomStatus.Retired },
                commandType: CommandType.StoredProcedure)
                .Select(row => new LookupItem((int)row.RoomID, (string)row.RoomName))
                .ToList();
        }

        public List<LookupItem> GetBorrowableDevices()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.Query(
                "sp_GetBorrowableDevices",
                new
                {
                    maintenanceStatus = DeviceStatus.Maintenance,
                    retiredStatus = DeviceStatus.Retired,
                    retiredRoomStatus = RoomStatus.Retired,
                    availableStatus = DeviceStatus.Available,
                    goodCondition = DeviceCondition.Good
                },
                commandType: CommandType.StoredProcedure)
                .Select(row => new LookupItem((int)row.DeviceID, (string)row.DisplayName))
                .ToList();
        }

        public List<LookupItem> GetAvailableInstances(int deviceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.Query(
                "sp_GetAvailableInstances",
                new
                {
                    devId = deviceId,
                    availableStatus = DeviceStatus.Available,
                    goodCondition = DeviceCondition.Good
                },
                commandType: CommandType.StoredProcedure)
                .Select(row => new LookupItem((int)row.InstanceID, (string)row.AssetCode))
                .ToList();
        }

        public int CreatePendingTicket(BorrowTicketDraft draft)
        {
            if (draft.Items.Count == 0)
                throw new InvalidOperationException("Phiếu mượn phải có ít nhất một cá thể thiết bị.");

            if (draft.Items.Any(item => item.InstanceId <= 0 || item.Quantity != 1))
                throw new InvalidOperationException("Mỗi dòng phiếu mượn phải gắn với một cá thể thiết bị hợp lệ và có số lượng bằng 1.");

            if (draft.Items.Select(item => item.InstanceId).Distinct().Count() != draft.Items.Count)
                throw new InvalidOperationException("Một cá thể thiết bị không được xuất hiện nhiều lần trong cùng phiếu.");

            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();

            try
            {
                // 1. Thêm phiếu mượn (Master)
                int ticketId = conn.ExecuteScalar<int>(
                    "sp_InsertBorrowTicket",
                    new
                    {
                        userID = draft.BorrowerId,
                        createdBy = draft.BorrowerId,
                        ticketCode = string.IsNullOrWhiteSpace(draft.TicketCode) ? null : draft.TicketCode,
                        borrowDate = draft.BorrowDate,
                        returnDate = draft.ExpectedReturnDate,
                        purpose = string.IsNullOrWhiteSpace(draft.Purpose) ? null : draft.Purpose,
                        status = BorrowTicketStatus.Pending,
                        note = string.IsNullOrWhiteSpace(draft.Note) ? null : draft.Note
                    },
                    transaction: tran,
                    commandType: CommandType.StoredProcedure);

                // 2. Thêm từng chi tiết phiếu mượn (Details)
                foreach (BorrowTicketDraftItem item in draft.Items)
                {
                    object? affected = conn.ExecuteScalar(
                        "sp_InsertBorrowDetail",
                        new
                        {
                            ticketID = ticketId,
                            deviceID = item.DeviceId,
                            instanceID = item.InstanceId,
                            qty = item.Quantity,
                            availableStatus = DeviceStatus.Available,
                            goodCondition = DeviceCondition.Good,
                            pendingStatus = BorrowTicketStatus.Pending,
                            borrowingStatus = BorrowTicketStatus.Borrowing,
                            returnPendingStatus = BorrowTicketStatus.ReturnPending
                        },
                        transaction: tran,
                        commandType: CommandType.StoredProcedure);

                    if (affected == null || Convert.ToInt32(affected) != 1)
                    {
                        throw new InvalidOperationException(
                            "Một cá thể đã được chọn ở phiếu khác hoặc không còn ở trạng thái Có sẵn/Tốt.");
                    }
                }

                tran.Commit();
                return ticketId;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public void ApproveBorrow(int ticketId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_ApproveBorrowTicket",
                new
                {
                    ticketId,
                    pendingStatus = BorrowTicketStatus.Pending,
                    borrowingStatus = BorrowTicketStatus.Borrowing,
                    maintenanceStatus = DeviceStatus.Maintenance,
                    retiredStatus = DeviceStatus.Retired,
                    availableStatus = DeviceStatus.Available,
                    borrowedStatus = DeviceStatus.Borrowed,
                    goodCondition = DeviceCondition.Good
                },
                commandType: CommandType.StoredProcedure);
        }

        public void RejectBorrow(int ticketId, string reason)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_RejectBorrowTicket",
                new
                {
                    ticketId,
                    rejectedStatus = BorrowTicketStatus.Rejected,
                    pendingStatus = BorrowTicketStatus.Pending,
                    reason
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
