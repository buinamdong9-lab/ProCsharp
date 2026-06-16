SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF COL_LENGTH('dbo.Users', 'FailedLoginCount') IS NULL
BEGIN
    ALTER TABLE dbo.Users
    ADD FailedLoginCount INT NOT NULL
        CONSTRAINT DF_Users_FailedLoginCount DEFAULT (0);
END;

IF COL_LENGTH('dbo.Users', 'LockoutUntil') IS NULL
BEGIN
    ALTER TABLE dbo.Users
    ADD LockoutUntil DATETIME NULL;
END;

IF OBJECT_ID(N'dbo.AppSettings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AppSettings
    (
        SettingKey NVARCHAR(100) NOT NULL
            CONSTRAINT PK_AppSettings PRIMARY KEY,
        SettingValue NVARCHAR(MAX) NULL
    );
END;

IF OBJECT_ID(N'dbo.ReturnRequests', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ReturnRequests
    (
        ReturnRequestID INT IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_ReturnRequests PRIMARY KEY,
        TicketID INT NOT NULL,
        RequestedAt DATETIME NOT NULL,
        Note NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL
            CONSTRAINT DF_ReturnRequests_CreatedAt DEFAULT (GETDATE()),
        CONSTRAINT FK_ReturnRequests_BorrowTickets
            FOREIGN KEY (TicketID) REFERENCES dbo.BorrowTickets(TicketID)
    );

    CREATE UNIQUE INDEX UX_ReturnRequests_TicketID
        ON dbo.ReturnRequests(TicketID);
END;

IF OBJECT_ID(N'dbo.ReturnRequestDetails', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ReturnRequestDetails
    (
        ReturnRequestDetailID INT IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_ReturnRequestDetails PRIMARY KEY,
        ReturnRequestID INT NOT NULL,
        DeviceID INT NOT NULL,
        InstanceID INT NULL,
        BorrowQuantity INT NOT NULL,
        ReturnQuantity INT NOT NULL,
        Note NVARCHAR(500) NULL,
        CONSTRAINT FK_ReturnRequestDetails_ReturnRequests
            FOREIGN KEY (ReturnRequestID) REFERENCES dbo.ReturnRequests(ReturnRequestID),
        CONSTRAINT FK_ReturnRequestDetails_Devices
            FOREIGN KEY (DeviceID) REFERENCES dbo.Devices(DeviceID),
        CONSTRAINT FK_ReturnRequestDetails_DeviceInstances
            FOREIGN KEY (InstanceID) REFERENCES dbo.DeviceInstances(InstanceID)
    );
END;

IF COL_LENGTH('dbo.BorrowDetails', 'ReturnedQuantity') IS NULL
BEGIN
    ALTER TABLE dbo.BorrowDetails
    ADD ReturnedQuantity INT NOT NULL
        CONSTRAINT DF_BorrowDetails_ReturnedQuantity DEFAULT (0);
END;

IF OBJECT_ID(N'dbo.CK_BorrowDetails_ReturnedQuantity', N'C') IS NULL
BEGIN
    ALTER TABLE dbo.BorrowDetails WITH CHECK
    ADD CONSTRAINT CK_BorrowDetails_ReturnedQuantity
        CHECK (ReturnedQuantity >= 0 AND ReturnedQuantity <= Quantity);
END;

IF COL_LENGTH('dbo.BorrowDetails', 'InstanceID') IS NULL
BEGIN
    ALTER TABLE dbo.BorrowDetails
    ADD InstanceID INT NULL;
END;

IF COL_LENGTH('dbo.BorrowTickets', 'Note') IS NOT NULL
   AND EXISTS (
       SELECT 1
       FROM sys.columns c
       JOIN sys.types t ON t.user_type_id = c.user_type_id
       WHERE c.object_id = OBJECT_ID(N'dbo.BorrowTickets')
         AND c.name = N'Note'
         AND t.name = N'nvarchar'
         AND c.max_length <> -1
   )
BEGIN
    ALTER TABLE dbo.BorrowTickets
    ALTER COLUMN Note NVARCHAR(MAX) NULL;
END;

IF OBJECT_ID(N'dbo.CK_BorrowTickets_Status', N'C') IS NOT NULL
BEGIN
    ALTER TABLE dbo.BorrowTickets
    DROP CONSTRAINT CK_BorrowTickets_Status;
END;

ALTER TABLE dbo.BorrowTickets WITH CHECK
ADD CONSTRAINT CK_BorrowTickets_Status
CHECK (Status IN (
    N'PENDING',
    N'BORROWING',
    N'RETURN_PENDING',
    N'RETURNED',
    N'REJECTED',
    N'CANCELLED'
));

ALTER TABLE dbo.BorrowTickets
CHECK CONSTRAINT CK_BorrowTickets_Status;

COMMIT TRANSACTION;
GO

CREATE OR ALTER TRIGGER dbo.TR_BorrowDetails_RequireInstance
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
END;
GO
