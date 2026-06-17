-- 42. THỦ TỤC ĐẢM BẢO SCHEMA BẢNG YÊU CẦU TRẢ THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_EnsureReturnRequestSchema]
AS
BEGIN
    SET NOCOUNT ON;
    IF OBJECT_ID('dbo.ReturnRequests', 'U') IS NULL
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
    END;
END
GO

