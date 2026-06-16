using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using FrmProject.Models;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    /// <summary>
    /// Shared service for approving/rejecting RETURN_PENDING tickets.
    /// Used by both UcDanhsachphieu and UcTrathietbi.
    /// </summary>
    public class ReturnApprovalService : IReturnApprovalService
    {
        /// <summary>
        /// Approves a RETURN_PENDING ticket: restores inventory and marks RETURNED.
        /// </summary>
        public void ApproveReturn(int ticketId, int approvedByUserId)
        {
            ApproveReturn(ticketId, approvedByUserId, null);
        }

        public void ApproveReturn(int ticketId, int approvedByUserId, List<(int DeviceID, int InstanceID)>? selectedItems = null)
        {
            if (selectedItems != null && selectedItems.Count > 0)
                throw new NotSupportedException("Duyệt trả từng phần theo danh sách thiết bị chọn không được hỗ trợ.");

            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            using SqlCommand cmd = new SqlCommand("sp_ApproveReturnRequest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            cmd.Parameters.AddWithValue("@approvedByUserId", approvedByUserId);
            cmd.Parameters.AddWithValue("@returnPendingStatus", BorrowTicketStatus.ReturnPending);
            cmd.Parameters.AddWithValue("@returnedStatus", BorrowTicketStatus.Returned);
            cmd.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
            cmd.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
            cmd.Parameters.AddWithValue("@availableStatus", DeviceStatus.Available);
            cmd.Parameters.AddWithValue("@maintenanceStatus", DeviceStatus.Maintenance);
            cmd.Parameters.AddWithValue("@goodCondition", DeviceCondition.Good);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Rejects a RETURN_PENDING ticket: reverts to BORROWING and clears ReturnNote.
        /// </summary>
        public void RejectReturn(int ticketId, string reason)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            using SqlCommand cmd = new SqlCommand("sp_RejectReturnRequest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            cmd.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
            cmd.Parameters.AddWithValue("@returnPendingStatus", BorrowTicketStatus.ReturnPending);
            cmd.Parameters.AddWithValue("@reason", reason);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Verifies that a ticket belongs to a specific user. Throws if not.
        /// </summary>
        public void VerifyTicketOwnership(SqlConnection conn, SqlTransaction tran, int ticketId, int userId)
        {
            using var cmd = new SqlCommand("sp_GetTicketOwnerId", conn, tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            var result = cmd.ExecuteScalar();
            if (result == null)
                throw new InvalidOperationException("Phiếu mượn không tồn tại.");
            int ownerUserId = Convert.ToInt32(result);
            if (ownerUserId != userId)
                throw new InvalidOperationException("Bạn không có quyền thao tác trên phiếu này.");
        }

        /// <summary>
        /// Loads pending return tickets for admin review.
        /// </summary>
        public DataTable GetPendingReturnTickets()
        {
            DataTable dt = new DataTable();
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            using SqlCommand cmd = new SqlCommand("sp_GetPendingReturnTickets", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@status", BorrowTicketStatus.ReturnPending);

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public string GetTicketStatus(SqlConnection conn, SqlTransaction? tran, int ticketId)
        {
            using var cmd = new SqlCommand("sp_GetTicketStatus", conn, tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            return cmd.ExecuteScalar()?.ToString() ?? string.Empty;
        }
    }
}
