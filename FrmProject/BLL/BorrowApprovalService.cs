using Microsoft.Data.SqlClient;
using System.Data;
using System;
using FrmProject.Models;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    /// <summary>
    /// Shared service for approving/rejecting PENDING borrow tickets.
    /// </summary>
    public class BorrowApprovalService : IBorrowApprovalService
    {
        public void ApproveBorrow(int ticketId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            using SqlCommand cmd = new SqlCommand("sp_ApproveBorrowTicket", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            cmd.Parameters.AddWithValue("@pendingStatus", BorrowTicketStatus.Pending);
            cmd.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
            cmd.Parameters.AddWithValue("@maintenanceStatus", DeviceStatus.Maintenance);
            cmd.Parameters.AddWithValue("@retiredStatus", DeviceStatus.Retired);
            cmd.Parameters.AddWithValue("@availableStatus", DeviceStatus.Available);
            cmd.Parameters.AddWithValue("@borrowedStatus", DeviceStatus.Borrowed);
            cmd.Parameters.AddWithValue("@goodCondition", DeviceCondition.Good);

            cmd.ExecuteNonQuery();
        }

        public void RejectBorrow(int ticketId, string reason)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            using SqlCommand cmd = new SqlCommand("sp_RejectBorrowTicket", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            cmd.Parameters.AddWithValue("@rejectedStatus", BorrowTicketStatus.Rejected);
            cmd.Parameters.AddWithValue("@pendingStatus", BorrowTicketStatus.Pending);
            cmd.Parameters.AddWithValue("@reason", reason);

            cmd.ExecuteNonQuery();
        }
    }
}
