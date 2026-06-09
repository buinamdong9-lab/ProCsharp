SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.BorrowDetailInstanceMigrationAudit', N'U') IS NULL
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
END;

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
  AND bt.Status IN (N'PENDING', N'BORROWING', N'RETURN_PENDING');

SELECT di.InstanceID,
       di.DeviceID,
       di.AssetCode,
       ROW_NUMBER() OVER (
           PARTITION BY di.DeviceID
           ORDER BY di.AssetCode, di.InstanceID
       ) AS SequenceNumber
INTO #AvailableInstances
FROM dbo.DeviceInstances di WITH (UPDLOCK, HOLDLOCK)
WHERE di.Status = N'Có sẵn'
  AND ISNULL(NULLIF(di.Condition, ''), N'Tốt') = N'Tốt'
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.BorrowDetails existing
      JOIN dbo.BorrowTickets ticket ON ticket.TicketID = existing.TicketID
      WHERE existing.InstanceID = di.InstanceID
        AND existing.Quantity > existing.ReturnedQuantity
        AND ticket.Status IN (N'PENDING', N'BORROWING', N'RETURN_PENDING')
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
SET Status = N'Đang mượn'
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
               WHEN instance.Status = N'Có sẵn'
                AND ISNULL(NULLIF(instance.Condition, ''), N'Tốt') = N'Tốt'
               THEN 1 ELSE 0
           END) AS AvailableQuantity
    FROM dbo.DeviceInstances instance
    WHERE instance.DeviceID = device.DeviceID
      AND instance.Status <> N'Ngừng sử dụng'
) counts
WHERE EXISTS (
    SELECT 1
    FROM #InstanceAssignments assignment
    WHERE assignment.DeviceID = device.DeviceID
);

EXEC(N'
CREATE OR ALTER TRIGGER dbo.TR_BorrowDetails_RequireInstance
ON dbo.BorrowDetails
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM inserted row WHERE row.InstanceID IS NULL)
        THROW 51002, N''Mỗi BorrowDetails phải gắn với một InstanceID.'', 1;

    IF EXISTS (
        SELECT 1
        FROM inserted row
        LEFT JOIN dbo.DeviceInstances instance
          ON instance.InstanceID = row.InstanceID
         AND instance.DeviceID = row.DeviceID
        WHERE instance.InstanceID IS NULL OR row.Quantity <> 1
    )
        THROW 51003, N''InstanceID không thuộc thiết bị hoặc Quantity khác 1.'', 1;
END;
');

COMMIT TRANSACTION;
