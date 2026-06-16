-- 49. THỦ TỤC THỐNG KÊ SỐ LƯỢNG PHIẾU
CREATE OR ALTER PROCEDURE [dbo].[sp_GetTicketListStats]
    @currentUserId INT,
    @isUser BIT,
    @pendingStatus NVARCHAR(50),
    @borrowingStatus NVARCHAR(50),
    @returnPendingStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        -- Total
        (SELECT COUNT(*) FROM BorrowTickets 
         WHERE (@isUser = 0 OR UserID = @currentUserId)) AS Total,
        -- Pending
        (SELECT COUNT(*) FROM BorrowTickets 
         WHERE Status = @pendingStatus AND (@isUser = 0 OR UserID = @currentUserId)) AS Pending,
        -- Active
        (SELECT COUNT(*) FROM BorrowTickets 
         WHERE Status IN (@borrowingStatus, @returnPendingStatus) AND (@isUser = 0 OR UserID = @currentUserId)) AS Active,
        -- Overdue
        (SELECT COUNT(*) FROM BorrowTickets 
         WHERE Status IN (@borrowingStatus, @returnPendingStatus) AND ExpectedReturnDate < GETDATE() AND (@isUser = 0 OR UserID = @currentUserId)) AS Overdue;
END
GO

