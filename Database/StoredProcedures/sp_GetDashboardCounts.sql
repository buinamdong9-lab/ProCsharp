-- =========================================================================
-- LỚP TRANG CHỦ (DASHBOARD REPOSITORY)
-- =========================================================================

-- 53. THỦ TỤC LẤY CÁC CON SỐ THỐNG KÊ TRÊN TRANG CHỦ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDashboardCounts]
    @maintenanceStatus NVARCHAR(50),
    @brokenStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @totalDevices INT = (SELECT COUNT(*) FROM dbo.Devices);
    
    DECLARE @borrowedDevices INT = (
        SELECT ISNULL(SUM(bd.Quantity - bd.ReturnedQuantity), 0)
        FROM dbo.BorrowTickets bt
        JOIN dbo.BorrowDetails bd ON bd.TicketID = bt.TicketID
        WHERE bt.Status IN ('BORROWING', 'RETURN_PENDING')
    );
    
    DECLARE @overdueTickets INT = (
        SELECT COUNT(*) 
        FROM dbo.BorrowTickets 
        WHERE Status IN ('BORROWING', 'RETURN_PENDING') 
          AND ExpectedReturnDate < GETDATE()
    );
    
    DECLARE @issueDevices INT = (
        SELECT COUNT(*) 
        FROM dbo.Devices 
        WHERE Status IN (@maintenanceStatus, @brokenStatus)
    );
    
    DECLARE @returnedToday INT = (
        SELECT COUNT(*) 
        FROM dbo.BorrowTickets 
        WHERE ReturnDate >= CAST(GETDATE() AS DATE)
    );
    
    DECLARE @totalBorrowingCount INT = (
        SELECT COUNT(*)
        FROM dbo.BorrowTickets bt
        JOIN dbo.BorrowDetails bd ON bd.TicketID = bt.TicketID
        WHERE bt.Status IN ('BORROWING', 'RETURN_PENDING')
          AND bd.Quantity > bd.ReturnedQuantity
    );
    
    SELECT @totalDevices AS TotalDevices,
           @borrowedDevices AS BorrowedDevices,
           @overdueTickets AS OverdueTickets,
           @issueDevices AS IssueDevices,
           @returnedToday AS ReturnedToday,
           @totalBorrowingCount AS TotalBorrowingCount;
END
GO

