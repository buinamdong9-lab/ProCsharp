using FrmProject.Models;
using FrmProject.BLL;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    internal static class ReturnRequestRepository
    {
        public static void SaveRequest(
            SqlConnection conn,
            SqlTransaction tran,
            int ticketId,
            DateTime requestedAt,
            IEnumerable<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)> items,
            string note)
        {
            EnsureSchema(conn, tran);

            using (var deleteCmd = new SqlCommand(
                @"DELETE d
                  FROM ReturnRequestDetails d
                  JOIN ReturnRequests r ON r.ReturnRequestID = d.ReturnRequestID
                  WHERE r.TicketID = @ticketId;

                  DELETE FROM ReturnRequests WHERE TicketID = @ticketId;", conn, tran))
            {
                deleteCmd.Parameters.AddWithValue("@ticketId", ticketId);
                deleteCmd.ExecuteNonQuery();
            }

            int requestId;
            using (var cmd = new SqlCommand(
                @"INSERT INTO ReturnRequests (TicketID, RequestedAt, Note)
                  OUTPUT INSERTED.ReturnRequestID
                  VALUES (@ticketId, @requestedAt, @note);", conn, tran))
            {
                cmd.Parameters.AddWithValue("@ticketId", ticketId);
                cmd.Parameters.AddWithValue("@requestedAt", requestedAt);
                cmd.Parameters.AddWithValue("@note", string.IsNullOrWhiteSpace(note) ? DBNull.Value : note.Trim());
                requestId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (var cmd = new SqlCommand(
                @"INSERT INTO ReturnRequestDetails
                    (ReturnRequestID, DeviceID, InstanceID, BorrowQuantity, ReturnQuantity, Note)
                  VALUES
                    (@requestId, @deviceId, @instanceId, @borrowQty, @returnQty, @note);", conn, tran))
            {
                cmd.Parameters.Add("@requestId", SqlDbType.Int).Value = requestId;
                cmd.Parameters.Add("@deviceId", SqlDbType.Int);
                cmd.Parameters.Add("@instanceId", SqlDbType.Int);
                cmd.Parameters.Add("@borrowQty", SqlDbType.Int);
                cmd.Parameters.Add("@returnQty", SqlDbType.Int);
                cmd.Parameters.Add("@note", SqlDbType.NVarChar, 255);

                foreach (var item in items.Where(x => x.ReturnQty > 0))
                {
                    cmd.Parameters["@deviceId"].Value = item.DeviceID;
                    cmd.Parameters["@instanceId"].Value = item.InstanceID <= 0 ? DBNull.Value : item.InstanceID;
                    cmd.Parameters["@borrowQty"].Value = item.BorrowQty;
                    cmd.Parameters["@returnQty"].Value = item.ReturnQty;
                    cmd.Parameters["@note"].Value = string.IsNullOrWhiteSpace(item.Note) ? DBNull.Value : item.Note.Trim();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool TryLoadRequest(
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
                using var cmd = new SqlCommand(
                    @"SELECT r.RequestedAt,
                             d.DeviceID,
                             ISNULL(d.InstanceID, 0),
                             d.BorrowQuantity,
                             d.ReturnQuantity,
                             ISNULL(d.Note, '')
                      FROM ReturnRequests r
                      JOIN ReturnRequestDetails d ON d.ReturnRequestID = r.ReturnRequestID
                      WHERE r.TicketID = @ticketId
                      ORDER BY d.ReturnRequestDetailID;", conn, tran);
                cmd.Parameters.AddWithValue("@ticketId", ticketId);

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (requestedAt == DateTime.MinValue)
                        requestedAt = reader.GetDateTime(0);

                    items.Add(new ReturnRequestItem
                    {
                        DeviceID = reader.GetInt32(1),
                        InstanceID = reader.GetInt32(2),
                        BorrowQty = reader.GetInt32(3),
                        ReturnQty = reader.GetInt32(4),
                        Note = reader.GetString(5)
                    });
                }

                if (items.Count > 0)
                    return true;
            }

            string? payload;
            using (var cmd = new SqlCommand("SELECT ReturnNote FROM BorrowTickets WHERE TicketID = @ticketId", conn, tran))
            {
                cmd.Parameters.AddWithValue("@ticketId", ticketId);
                payload = cmd.ExecuteScalar()?.ToString();
            }

            return ReturnRequestHelper.TryParsePayload(payload, out requestedAt, out items);
        }

        public static void ClearRequest(SqlConnection conn, SqlTransaction tran, int ticketId)
        {
            if (!HasStructuredTables(conn, tran))
                return;

            using var cmd = new SqlCommand(
                @"DELETE d
                  FROM ReturnRequestDetails d
                  JOIN ReturnRequests r ON r.ReturnRequestID = d.ReturnRequestID
                  WHERE r.TicketID = @ticketId;

                  DELETE FROM ReturnRequests WHERE TicketID = @ticketId;", conn, tran);
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            cmd.ExecuteNonQuery();
        }

        public static void EnsureSchema(SqlConnection conn, SqlTransaction? tran = null)
        {
            using var cmd = new SqlCommand(
                @"IF OBJECT_ID('dbo.ReturnRequests', 'U') IS NULL
                  BEGIN
                      CREATE TABLE dbo.ReturnRequests
                      (
                          ReturnRequestID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ReturnRequests PRIMARY KEY,
                          TicketID INT NOT NULL,
                          RequestedAt DATETIME NOT NULL,
                          Note NVARCHAR(500) NULL,
                          CreatedAt DATETIME NOT NULL CONSTRAINT DF_ReturnRequests_CreatedAt DEFAULT (GETDATE()),
                          CONSTRAINT FK_ReturnRequests_BorrowTickets FOREIGN KEY (TicketID) REFERENCES dbo.BorrowTickets(TicketID)
                      );
                      CREATE UNIQUE INDEX UX_ReturnRequests_TicketID ON dbo.ReturnRequests(TicketID);
                  END;

                  IF OBJECT_ID('dbo.ReturnRequestDetails', 'U') IS NULL
                  BEGIN
                      CREATE TABLE dbo.ReturnRequestDetails
                      (
                          ReturnRequestDetailID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ReturnRequestDetails PRIMARY KEY,
                          ReturnRequestID INT NOT NULL,
                          DeviceID INT NOT NULL,
                          InstanceID INT NULL,
                          BorrowQuantity INT NOT NULL,
                          ReturnQuantity INT NOT NULL,
                          Note NVARCHAR(255) NULL,
                          CONSTRAINT FK_ReturnRequestDetails_ReturnRequests FOREIGN KEY (ReturnRequestID) REFERENCES dbo.ReturnRequests(ReturnRequestID),
                          CONSTRAINT FK_ReturnRequestDetails_Devices FOREIGN KEY (DeviceID) REFERENCES dbo.Devices(DeviceID),
                          CONSTRAINT FK_ReturnRequestDetails_DeviceInstances FOREIGN KEY (InstanceID) REFERENCES dbo.DeviceInstances(InstanceID)
                      );
                  END;", conn, tran);
            cmd.ExecuteNonQuery();
        }

        private static bool HasStructuredTables(SqlConnection conn, SqlTransaction? tran)
        {
            using var cmd = new SqlCommand(
                @"SELECT CASE
                    WHEN OBJECT_ID('dbo.ReturnRequests', 'U') IS NOT NULL
                     AND OBJECT_ID('dbo.ReturnRequestDetails', 'U') IS NOT NULL
                    THEN 1 ELSE 0 END", conn, tran);
            return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
        }
    }
}

