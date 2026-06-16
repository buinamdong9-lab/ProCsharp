using Microsoft.Data.SqlClient;
using System.Data;
using System;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class ReportRepository : IReportRepository
    {
        public DataTable GetMonthlyStats(DateTime from, DateTime to)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            using SqlCommand cmd = new SqlCommand("sp_GetReportMonthlyStats", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);
            cmd.Parameters.AddWithValue("@returnedStatus", BorrowTicketStatus.Returned);
            cmd.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
            cmd.Parameters.AddWithValue("@returnPendingStatus", BorrowTicketStatus.ReturnPending);

            DataTable dt = new DataTable();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public DataTable GetTopDevices(DateTime from, DateTime to)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            using SqlCommand cmd = new SqlCommand("sp_GetReportTopDevices", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            DataTable dt = new DataTable();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public DataTable GetOverdueTickets(DateTime from, DateTime to)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            using SqlCommand cmd = new SqlCommand("sp_GetReportOverdueTickets", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);
            cmd.Parameters.AddWithValue("@borrowingStatus", BorrowTicketStatus.Borrowing);
            cmd.Parameters.AddWithValue("@returnPendingStatus", BorrowTicketStatus.ReturnPending);

            DataTable dt = new DataTable();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
    }
}

