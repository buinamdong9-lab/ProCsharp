using System.Collections.Concurrent;
using System.Data;
using Microsoft.Data.SqlClient;

namespace FrmProject.DAL
{
    public enum AppRole
    {
        User,
        Staff,
        Admin
    }

    public sealed class PermissionSet
    {
        public bool Dashboard { get; init; }
        public bool Devices { get; init; }
        public bool Rooms { get; init; }
        public bool Users { get; init; }
        public bool BorrowTicket { get; init; }
        public bool ReturnDevice { get; init; }
        public bool TicketList { get; init; }
        public bool Reports { get; init; }
        public bool Settings { get; init; }
    }

    public static class DbSchemaHelper
    {
        private static readonly ConcurrentDictionary<string, bool> _columnCache = new();

        /// <summary>Clears the schema cache. Call when DB schema changes at runtime (rare).</summary>
        public static void ClearCache() => _columnCache.Clear();

        public static bool HasColumn(SqlConnection conn, string tableName, string columnName, SqlTransaction? tran = null)
        {
            SqlConnectionStringBuilder csb = new(conn.ConnectionString);
            string key = $"{csb.DataSource}|{csb.InitialCatalog}|{tableName}.{columnName}";

            return _columnCache.GetOrAdd(key, _ =>
            {
                using var cmd = new SqlCommand(
                    "SELECT CASE WHEN COL_LENGTH(@tableName, @columnName) IS NULL THEN 0 ELSE 1 END", conn, tran);
                cmd.Parameters.AddWithValue("@tableName", tableName);
                cmd.Parameters.AddWithValue("@columnName", columnName);
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            });
        }

        /// <summary>Returns the raw columns needed for GROUP BY when using status CASE expression.</summary>
        public static string GetUserGroupByColumns(SqlConnection conn, string tableAlias = "u")
        {
            var cols = new List<string>();
            if (HasColumn(conn, "Users", "IsActive"))
                cols.Add($"{tableAlias}.IsActive");
            if (HasColumn(conn, "Users", "IsLocked"))
                cols.Add($"{tableAlias}.IsLocked");
            return string.Join(", ", cols);
        }

        public static string GetBorrowTicketDisplayExpression(SqlConnection conn, string tableAlias = "bt", SqlTransaction? tran = null)
        {
            return HasColumn(conn, "BorrowTickets", "TicketCode", tran)
                ? $"ISNULL({tableAlias}.TicketCode, 'PM' + CONVERT(VARCHAR, {tableAlias}.TicketID))"
                : $"'PM' + CONVERT(VARCHAR, {tableAlias}.TicketID)";
        }

        public static string GetBorrowTicketDisplayExpressionForCast(SqlConnection conn, string tableAlias = "bt", SqlTransaction? tran = null)
        {
            return HasColumn(conn, "BorrowTickets", "TicketCode", tran)
                ? $"ISNULL({tableAlias}.TicketCode, CAST({tableAlias}.TicketID AS VARCHAR))"
                : $"'PM' + CAST({tableAlias}.TicketID AS VARCHAR)";
        }

        public static string GetReturnDateColumnExpression(SqlConnection conn, string tableAlias = "bt", SqlTransaction? tran = null)
        {
            if (HasColumn(conn, "BorrowTickets", "ReturnDate", tran))
                return $"{tableAlias}.ReturnDate";

            if (HasColumn(conn, "BorrowTickets", "ActualReturnDate", tran))
                return $"{tableAlias}.ActualReturnDate";

            return "NULL";
        }

        public static string BuildLoginWhereClause(SqlConnection conn, string tableAlias = "u")
        {
            List<string> conditions = new List<string> { $"{tableAlias}.Username = @username" };
            conditions.AddRange(BuildEnabledUserConditions(conn, tableAlias));
            return string.Join(" AND ", conditions);
        }

        public static string BuildEnabledUserWhereClause(SqlConnection conn, string tableAlias = "u")
        {
            List<string> conditions = BuildEnabledUserConditions(conn, tableAlias);
            return conditions.Count == 0 ? "1 = 1" : string.Join(" AND ", conditions);
        }

        public static string GetUserStatusCaseExpression(SqlConnection conn, string tableAlias = "u")
        {
            bool hasIsActive = HasColumn(conn, "Users", "IsActive");
            bool hasIsLocked = HasColumn(conn, "Users", "IsLocked");

            if (hasIsActive && hasIsLocked)
            {
                return $@"CASE
                            WHEN ISNULL({tableAlias}.IsActive, 1) = 0 THEN N'Ngừng hoạt động'
                            WHEN ISNULL({tableAlias}.IsLocked, 0) = 1 THEN N'Bị khóa'
                            ELSE N'Hoạt động'
                         END";
            }

            if (hasIsActive)
                return $"CASE WHEN ISNULL({tableAlias}.IsActive, 1) = 1 THEN N'Hoạt động' ELSE N'Ngừng hoạt động' END";

            if (hasIsLocked)
                return $"CASE WHEN ISNULL({tableAlias}.IsLocked, 0) = 0 THEN N'Hoạt động' ELSE N'Bị khóa' END";

            return "N'Hoạt động'";
        }

        public static string BuildUserStatusFilterClause(SqlConnection conn, string selectedStatus, string tableAlias = "u")
        {
            bool hasIsActive = HasColumn(conn, "Users", "IsActive");
            bool hasIsLocked = HasColumn(conn, "Users", "IsLocked");

            if (string.IsNullOrWhiteSpace(selectedStatus) || selectedStatus.Contains("Tất cả"))
                return string.Empty;

            bool wantsActive = selectedStatus.Contains("Hoạt động");
            bool wantsInactive = selectedStatus.Contains("Ngừng");
            bool wantsLocked = selectedStatus.Contains("khóa", StringComparison.OrdinalIgnoreCase);

            if (wantsActive)
            {
                List<string> parts = new List<string>();
                if (hasIsActive)
                    parts.Add($"ISNULL({tableAlias}.IsActive, 1) = 1");
                if (hasIsLocked)
                    parts.Add($"ISNULL({tableAlias}.IsLocked, 0) = 0");
                return parts.Count == 0 ? string.Empty : string.Join(" AND ", parts);
            }

            if (wantsInactive && hasIsActive)
                return $"ISNULL({tableAlias}.IsActive, 1) = 0";

            if (wantsLocked && hasIsLocked)
                return $"ISNULL({tableAlias}.IsLocked, 0) = 1";

            return string.Empty;
        }

        public static AppRole ResolveRole(string roleName)
        {
            string normalizedRole = roleName.Trim().ToLowerInvariant();

            if (normalizedRole.Contains("admin") || normalizedRole.Contains("quản trị"))
                return AppRole.Admin;

            if (normalizedRole.Contains("thủ kho") || normalizedRole.Contains("quan ly") ||
                normalizedRole.Contains("quản lý") || normalizedRole.Contains("staff"))
                return AppRole.Staff;

            return AppRole.User;
        }

        public static PermissionSet BuildPermissions(AppRole role) =>
            role switch
            {
                AppRole.Admin => new PermissionSet
                {
                    Dashboard = true,
                    Devices = true,
                    Rooms = true,
                    Users = true,
                    BorrowTicket = true,
                    ReturnDevice = true,
                    TicketList = true,
                    Reports = true,
                    Settings = true
                },
                AppRole.Staff => new PermissionSet
                {
                    Dashboard = true,
                    Devices = true,
                    Rooms = true,
                    Users = false,
                    BorrowTicket = true,
                    ReturnDevice = true,
                    TicketList = true,
                    Reports = true,
                    Settings = false
                },
                _ => new PermissionSet
                {
                    Dashboard = false,
                    Devices = false,
                    Rooms = false,
                    Users = false,
                    BorrowTicket = true,
                    ReturnDevice = true,
                    TicketList = true,
                    Reports = false,
                    Settings = false
                }
            };

        public static string BuildActiveBorrowStatusCondition(string tableAlias = "")
        {
            string statusColumn = string.IsNullOrWhiteSpace(tableAlias)
                ? "Status"
                : $"{tableAlias}.Status";

            return $"{statusColumn} IN ('{BorrowTicketStatus.Borrowing}', '{BorrowTicketStatus.ReturnPending}')";
        }

        public static void EnsureBorrowTicketStatusConstraint(SqlConnection conn, SqlTransaction? tran = null)
        {
            using var cmd = new SqlCommand(
                $@"IF OBJECT_ID(N'dbo.CK_BorrowTickets_Status', N'C') IS NOT NULL
                   BEGIN
                       ALTER TABLE dbo.BorrowTickets DROP CONSTRAINT CK_BorrowTickets_Status;
                   END;

                   ALTER TABLE dbo.BorrowTickets WITH CHECK ADD CONSTRAINT CK_BorrowTickets_Status
                   CHECK (Status IN (
                       N'{BorrowTicketStatus.Pending}',
                       N'{BorrowTicketStatus.Borrowing}',
                       N'{BorrowTicketStatus.ReturnPending}',
                       N'{BorrowTicketStatus.Returned}',
                       N'{BorrowTicketStatus.Rejected}',
                       N'CANCELLED'
                   ));

                   ALTER TABLE dbo.BorrowTickets CHECK CONSTRAINT CK_BorrowTickets_Status;", conn, tran);
            cmd.ExecuteNonQuery();
        }

        public static void EnsureBorrowTicketNoteCapacity(SqlConnection conn, SqlTransaction? tran = null)
        {
            if (!HasColumn(conn, "BorrowTickets", "Note", tran))
                return;

            using var cmd = new SqlCommand(
                @"IF EXISTS (
                      SELECT 1
                      FROM sys.columns c
                      JOIN sys.types t ON t.user_type_id = c.user_type_id
                      WHERE c.object_id = OBJECT_ID(N'dbo.BorrowTickets')
                        AND c.name = N'Note'
                        AND t.name = N'nvarchar'
                        AND c.max_length <> -1
                  )
                  BEGIN
                      ALTER TABLE dbo.BorrowTickets ALTER COLUMN Note NVARCHAR(MAX) NULL;
                  END;", conn, tran);
            cmd.ExecuteNonQuery();
        }

        public static void EnsureReturnedQuantitySchemaAndRestore(SqlConnection conn, SqlTransaction? tran = null)
        {
            ClearCache();

            if (!HasColumn(conn, "BorrowDetails", "ReturnedQuantity", tran))
            {
                using (var cmd = new SqlCommand(
                    "ALTER TABLE dbo.BorrowDetails ADD ReturnedQuantity INT NOT NULL CONSTRAINT DF_BorrowDetails_ReturnedQuantity DEFAULT 0", conn, tran))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SqlCommand(
                    "ALTER TABLE dbo.BorrowDetails WITH CHECK ADD CONSTRAINT CK_BorrowDetails_ReturnedQuantity CHECK (ReturnedQuantity >= 0 AND ReturnedQuantity <= Quantity)", conn, tran))
                {
                    cmd.ExecuteNonQuery();
                }

                ClearCache();
            }

            if (HasColumn(conn, "ReturnRequestDetails", "ReturnQuantity", tran))
            {
                bool hasInstanceId = HasColumn(conn, "BorrowDetails", "InstanceID", tran);

                string selectQuery = hasInstanceId
                    ? @"SELECT rr.TicketID, rrd.DeviceID, rrd.InstanceID, rrd.BorrowQuantity, rrd.ReturnQuantity, ISNULL(rrd.Note, '')
                        FROM dbo.ReturnRequestDetails rrd
                        JOIN dbo.ReturnRequests rr ON rr.ReturnRequestID = rrd.ReturnRequestID
                        WHERE NOT EXISTS (
                            SELECT 1 FROM dbo.BorrowDetails bd
                            WHERE bd.TicketID = rr.TicketID
                              AND bd.DeviceID = rrd.DeviceID
                              AND ISNULL(bd.InstanceID, 0) = ISNULL(rrd.InstanceID, 0)
                        )"
                    : @"SELECT rr.TicketID, rrd.DeviceID, 0 AS InstanceID, rrd.BorrowQuantity, rrd.ReturnQuantity, ISNULL(rrd.Note, '')
                        FROM dbo.ReturnRequestDetails rrd
                        JOIN dbo.ReturnRequests rr ON rr.ReturnRequestID = rrd.ReturnRequestID
                        WHERE NOT EXISTS (
                            SELECT 1 FROM dbo.BorrowDetails bd
                            WHERE bd.TicketID = rr.TicketID
                              AND bd.DeviceID = rrd.DeviceID
                        )";

                var rowsToRestore = new List<(int TicketID, int DeviceID, int? InstanceID, int BorrowQty, int ReturnQty, string Note)>();
                using (var selectCmd = new SqlCommand(selectQuery, conn, tran))
                {
                    using var reader = selectCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        rowsToRestore.Add((
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.IsDBNull(2) ? null : reader.GetInt32(2),
                            reader.GetInt32(3),
                            reader.GetInt32(4),
                            reader.GetString(5)
                        ));
                    }
                }

                if (rowsToRestore.Count > 0)
                {
                    string insertQuery = hasInstanceId
                        ? @"INSERT INTO dbo.BorrowDetails (TicketID, DeviceID, InstanceID, Quantity, ReturnedQuantity, Note)
                            VALUES (@ticketID, @deviceID, @instanceID, @quantity, @returnedQuantity, @note)"
                        : @"INSERT INTO dbo.BorrowDetails (TicketID, DeviceID, Quantity, ReturnedQuantity, Note)
                            VALUES (@ticketID, @deviceID, @quantity, @returnedQuantity, @note)";

                    using var insertCmd = new SqlCommand(insertQuery, conn, tran);
                    insertCmd.Parameters.Add("@ticketID", SqlDbType.Int);
                    insertCmd.Parameters.Add("@deviceID", SqlDbType.Int);
                    if (hasInstanceId)
                        insertCmd.Parameters.Add("@instanceID", SqlDbType.Int);
                    insertCmd.Parameters.Add("@quantity", SqlDbType.Int);
                    insertCmd.Parameters.Add("@returnedQuantity", SqlDbType.Int);
                    insertCmd.Parameters.Add("@note", SqlDbType.NVarChar, 255);

                    foreach (var row in rowsToRestore)
                    {
                        insertCmd.Parameters["@ticketID"].Value = row.TicketID;
                        insertCmd.Parameters["@deviceID"].Value = row.DeviceID;
                        if (hasInstanceId)
                        {
                            insertCmd.Parameters["@instanceID"].Value = row.InstanceID.HasValue && row.InstanceID.Value > 0
                                ? row.InstanceID.Value
                                : DBNull.Value;
                        }
                        insertCmd.Parameters["@quantity"].Value = row.BorrowQty;
                        insertCmd.Parameters["@returnedQuantity"].Value = row.ReturnQty;
                        insertCmd.Parameters["@note"].Value = string.IsNullOrWhiteSpace(row.Note) ? DBNull.Value : row.Note;

                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void EnsureBorrowDetailInstanceIntegrity(SqlConnection conn)
        {
            if (!HasColumn(conn, "BorrowDetails", "InstanceID") ||
                !HasColumn(conn, "DeviceInstances", "InstanceID"))
            {
                throw new InvalidOperationException(
                    "Schema hiện tại chưa có BorrowDetails.InstanceID hoặc DeviceInstances.InstanceID.");
            }

            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand(
                    @"IF OBJECT_ID(N'dbo.BorrowDetailInstanceMigrationAudit', N'U') IS NULL
                      BEGIN
                          CREATE TABLE dbo.BorrowDetailInstanceMigrationAudit
                          (
                              AuditID INT IDENTITY(1,1) NOT NULL
                                  CONSTRAINT PK_BorrowDetailInstanceMigrationAudit PRIMARY KEY,
                              BorrowDetailID INT NOT NULL,
                              TicketID INT NOT NULL,
                              DeviceID INT NOT NULL,
                              InstanceID INT NOT NULL,
                              AssetCode NVARCHAR(50) NOT NULL,
                              MigratedAt DATETIME2 NOT NULL
                                  CONSTRAINT DF_BorrowDetailInstanceMigrationAudit_MigratedAt DEFAULT SYSUTCDATETIME(),
                              CONSTRAINT UQ_BorrowDetailInstanceMigrationAudit_BorrowDetailID UNIQUE (BorrowDetailID)
                          );
                      END;", conn, tran))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SqlCommand(
                    $@"IF OBJECT_ID('tempdb..#LegacyBorrowRows') IS NOT NULL DROP TABLE #LegacyBorrowRows;
                       IF OBJECT_ID('tempdb..#AvailableInstances') IS NOT NULL DROP TABLE #AvailableInstances;
                       IF OBJECT_ID('tempdb..#InstanceAssignments') IS NOT NULL DROP TABLE #InstanceAssignments;

                       SELECT bd.BorrowDetailID,
                              bd.TicketID,
                              bd.DeviceID,
                              ROW_NUMBER() OVER (
                                  PARTITION BY bd.DeviceID
                                  ORDER BY bt.BorrowDate, bd.BorrowDetailID
                              ) AS SequenceNumber
                       INTO #LegacyBorrowRows
                       FROM dbo.BorrowDetails bd WITH (UPDLOCK, HOLDLOCK)
                       JOIN dbo.BorrowTickets bt ON bt.TicketID = bd.TicketID
                       WHERE bd.InstanceID IS NULL
                         AND bd.Quantity = 1
                         AND bd.Quantity > bd.ReturnedQuantity
                         AND bt.Status IN (
                             N'{BorrowTicketStatus.Pending}',
                             N'{BorrowTicketStatus.Borrowing}',
                             N'{BorrowTicketStatus.ReturnPending}'
                         );

                       SELECT di.InstanceID,
                              di.DeviceID,
                              di.AssetCode,
                              ROW_NUMBER() OVER (
                                  PARTITION BY di.DeviceID
                                  ORDER BY di.AssetCode, di.InstanceID
                              ) AS SequenceNumber
                       INTO #AvailableInstances
                       FROM dbo.DeviceInstances di WITH (UPDLOCK, HOLDLOCK)
                       WHERE di.Status = @availableStatus
                         AND ISNULL(NULLIF(di.Condition, ''), @goodCondition) = @goodCondition
                         AND NOT EXISTS (
                             SELECT 1
                             FROM dbo.BorrowDetails existing
                             JOIN dbo.BorrowTickets ticket ON ticket.TicketID = existing.TicketID
                             WHERE existing.InstanceID = di.InstanceID
                               AND existing.Quantity > existing.ReturnedQuantity
                               AND ticket.Status IN (
                                   N'{BorrowTicketStatus.Pending}',
                                   N'{BorrowTicketStatus.Borrowing}',
                                   N'{BorrowTicketStatus.ReturnPending}'
                               )
                         );

                       SELECT legacy.BorrowDetailID,
                              legacy.TicketID,
                              legacy.DeviceID,
                              available.InstanceID,
                              available.AssetCode
                       INTO #InstanceAssignments
                       FROM #LegacyBorrowRows legacy
                       LEFT JOIN #AvailableInstances available
                         ON available.DeviceID = legacy.DeviceID
                        AND available.SequenceNumber = legacy.SequenceNumber;

                       IF EXISTS (SELECT 1 FROM #InstanceAssignments WHERE InstanceID IS NULL)
                       BEGIN
                           THROW 51001, N'Không đủ cá thể Có sẵn/Tốt để đối soát BorrowDetails cũ.', 1;
                       END;

                       INSERT INTO dbo.BorrowDetailInstanceMigrationAudit
                           (BorrowDetailID, TicketID, DeviceID, InstanceID, AssetCode)
                       SELECT assignment.BorrowDetailID,
                              assignment.TicketID,
                              assignment.DeviceID,
                              assignment.InstanceID,
                              assignment.AssetCode
                       FROM #InstanceAssignments assignment
                       WHERE NOT EXISTS (
                           SELECT 1
                           FROM dbo.BorrowDetailInstanceMigrationAudit audit
                           WHERE audit.BorrowDetailID = assignment.BorrowDetailID
                       );

                       UPDATE detail
                       SET InstanceID = assignment.InstanceID,
                           Note = CASE
                               WHEN ISNULL(detail.Note, '') = ''
                                   THEN N'Migration tự động gán cá thể ' + assignment.AssetCode
                               ELSE detail.Note + N' | Migration tự động gán cá thể ' + assignment.AssetCode
                           END
                       FROM dbo.BorrowDetails detail
                       JOIN #InstanceAssignments assignment
                         ON assignment.BorrowDetailID = detail.BorrowDetailID;

                       UPDATE instance
                       SET Status = @borrowedStatus
                       FROM dbo.DeviceInstances instance
                       JOIN #InstanceAssignments assignment
                         ON assignment.InstanceID = instance.InstanceID;

                       UPDATE device
                       SET TotalQuantity = counts.TotalQuantity,
                           AvailableQuantity = counts.AvailableQuantity
                       FROM dbo.Devices device
                       CROSS APPLY (
                           SELECT COUNT(*) AS TotalQuantity,
                                  SUM(CASE
                                      WHEN instance.Status = @availableStatus
                                       AND ISNULL(NULLIF(instance.Condition, ''), @goodCondition) = @goodCondition
                                      THEN 1 ELSE 0
                                  END) AS AvailableQuantity
                           FROM dbo.DeviceInstances instance
                           WHERE instance.DeviceID = device.DeviceID
                             AND instance.Status <> @retiredStatus
                       ) counts
                       WHERE EXISTS (
                           SELECT 1
                           FROM #InstanceAssignments assignment
                           WHERE assignment.DeviceID = device.DeviceID
                       );", conn, tran))
                {
                    cmd.Parameters.Add("@availableStatus", SqlDbType.NVarChar, 50).Value = DeviceStatus.Available;
                    cmd.Parameters.Add("@borrowedStatus", SqlDbType.NVarChar, 50).Value = DeviceStatus.Borrowed;
                    cmd.Parameters.Add("@retiredStatus", SqlDbType.NVarChar, 50).Value = DeviceStatus.Retired;
                    cmd.Parameters.Add("@goodCondition", SqlDbType.NVarChar, 50).Value = DeviceCondition.Good;
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SqlCommand(
                    @"CREATE OR ALTER TRIGGER dbo.TR_BorrowDetails_RequireInstance
                      ON dbo.BorrowDetails
                      AFTER INSERT, UPDATE
                      AS
                      BEGIN
                          SET NOCOUNT ON;

                          IF EXISTS (
                              SELECT 1
                              FROM inserted row
                              WHERE row.InstanceID IS NULL
                          )
                          BEGIN
                              THROW 51002, N'Mỗi BorrowDetails phải gắn với một InstanceID.', 1;
                          END;

                          IF EXISTS (
                              SELECT 1
                              FROM inserted row
                              LEFT JOIN dbo.DeviceInstances instance
                                ON instance.InstanceID = row.InstanceID
                               AND instance.DeviceID = row.DeviceID
                              WHERE instance.InstanceID IS NULL
                                 OR row.Quantity <> 1
                          )
                          BEGIN
                              THROW 51003, N'InstanceID không thuộc thiết bị hoặc Quantity khác 1.', 1;
                          END;
                      END;", conn, tran))
                {
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }


        private static List<string> BuildEnabledUserConditions(SqlConnection conn, string tableAlias)
        {
            List<string> conditions = new List<string>();

            if (HasColumn(conn, "Users", "IsActive"))
                conditions.Add($"ISNULL({tableAlias}.IsActive, 1) = 1");

            if (HasColumn(conn, "Users", "IsLocked"))
                conditions.Add($"ISNULL({tableAlias}.IsLocked, 0) = 0");

            return conditions;
        }
    }
}

