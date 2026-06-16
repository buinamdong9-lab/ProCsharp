-- 84. THỦ TỤC TỪ CHỐI DUYỆT PHIẾU MƯỢN
CREATE OR ALTER PROCEDURE [dbo].[sp_RejectBorrowTicket]
    @ticketId INT,
    @rejectedStatus NVARCHAR(50),
    @pendingStatus NVARCHAR(50),
    @reason NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @currentStatus NVARCHAR(50);
    SELECT @currentStatus = Status FROM BorrowTickets WHERE TicketID = @ticketId;
    
    IF @currentStatus IS NULL
    BEGIN
        THROW 50028, N'Không tìm thấy phiếu mượn.', 1;
    END
    
    IF UPPER(@currentStatus) <> UPPER(@pendingStatus)
    BEGIN
        THROW 50029, N'Phiếu này không còn ở trạng thái chờ duyệt.', 1;
    END

    UPDATE BorrowTickets 
    SET Status = @rejectedStatus, 
        Note = ISNULL(Note, '') + ' | Từ chối: ' + @reason 
    WHERE TicketID = @ticketId;
END
GO

