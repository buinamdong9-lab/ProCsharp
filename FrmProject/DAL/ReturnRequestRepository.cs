using FrmProject.Models;
using FrmProject.BLL;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Dapper;

namespace FrmProject.DAL
{
    public class ReturnRequestRepository : IReturnRequestRepository
    {
        public void SaveRequest(
            SqlConnection conn,
            SqlTransaction tran,
            int ticketId,
            DateTime requestedAt,
            IEnumerable<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)> items,
            string note)
        {
            ValidateSchema(conn, tran);

            int requestId = conn.ExecuteScalar<int>(
                "sp_SaveReturnRequestMaster",
                new
                {
                    ticketId,
                    requestedAt,
                    note = string.IsNullOrWhiteSpace(note) ? null : note.Trim()
                },
                transaction: tran,
                commandType: CommandType.StoredProcedure);

            foreach (var item in items.Where(x => x.ReturnQty > 0))
            {
                conn.Execute(
                    "sp_SaveReturnRequestDetail",
                    new
                    {
                        requestId,
                        deviceId = item.DeviceID,
                        instanceId = item.InstanceID,
                        borrowQty = item.BorrowQty,
                        returnQty = item.ReturnQty,
                        note = string.IsNullOrWhiteSpace(item.Note) ? null : item.Note.Trim()
                    },
                    transaction: tran,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public bool TryLoadRequest(
            SqlConnection conn,
            SqlTransaction? tran,
            int ticketId,
            out DateTime requestedAt,
            out List<ReturnRequestItem> items)
        {
            requestedAt = DateTime.MinValue;
            items = new List<ReturnRequestItem>();

            if (HasStructuredTables(conn, tran))
            {
                var queryResults = conn.Query(
                    "sp_LoadReturnRequest",
                    new { ticketId },
                    transaction: tran,
                    commandType: CommandType.StoredProcedure);

                foreach (var reader in queryResults)
                {
                    var dict = (IDictionary<string, object>)reader;
                    var keys = dict.Keys.ToList();

                    if (requestedAt == DateTime.MinValue)
                        requestedAt = (DateTime)dict[keys[0]];

                    items.Add(new ReturnRequestItem
                    {
                        DeviceID = Convert.ToInt32(dict[keys[1]]),
                        InstanceID = Convert.ToInt32(dict[keys[2]]),
                        BorrowQty = Convert.ToInt32(dict[keys[3]]),
                        ReturnQty = Convert.ToInt32(dict[keys[4]]),
                        Note = dict[keys[5]]?.ToString() ?? string.Empty
                    });
                }

                if (items.Count > 0)
                    return true;
            }

            string? payload = conn.ExecuteScalar<string>(
                "sp_GetReturnNotePayload",
                new { ticketId },
                transaction: tran,
                commandType: CommandType.StoredProcedure);

            return ReturnRequestHelper.TryParsePayload(payload, out requestedAt, out items);
        }

        public void ClearRequest(SqlConnection conn, SqlTransaction tran, int ticketId)
        {
            conn.Execute(
                "sp_ClearReturnRequest",
                new { ticketId },
                transaction: tran,
                commandType: CommandType.StoredProcedure);
        }

        public void ValidateSchema(SqlConnection conn, SqlTransaction? tran = null)
        {
            if (tran == null)
            {
                DbSchemaHelper.ValidateReturnRequestSchema(conn);
                return;
            }

            RequireTable(conn, tran, "ReturnRequests");
            RequireTable(conn, tran, "ReturnRequestDetails");
        }

        private static bool HasStructuredTables(SqlConnection conn, SqlTransaction? tran)
        {
            return conn.ExecuteScalar<int>(
                @"SELECT CASE
                    WHEN OBJECT_ID(N'dbo.ReturnRequests', N'U') IS NOT NULL
                     AND OBJECT_ID(N'dbo.ReturnRequestDetails', N'U') IS NOT NULL
                    THEN 1 ELSE 0 END",
                transaction: tran) == 1;
        }

        private static void RequireTable(SqlConnection conn, SqlTransaction tran, string tableName)
        {
            if (conn.ExecuteScalar<int>(
                "SELECT CASE WHEN OBJECT_ID(@tableName, N'U') IS NULL THEN 0 ELSE 1 END",
                new { tableName = $"dbo.{tableName}" },
                transaction: tran) != 1)
            {
                throw new InvalidOperationException($"Database thiếu bảng dbo.{tableName}. Vui lòng chạy migration trước khi mở ứng dụng.");
            }
        }
    }
}
