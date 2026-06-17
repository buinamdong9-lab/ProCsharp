-- 41. THỦ TỤC CẬP NHẬT TRẠNG THÁI YÊU CẦU TRẢ PHIẾU
CREATE OR ALTER PROCEDURE [dbo].[sp_SubmitReturnRequestUpdate]
    @ticketId INT,
    @currentUserId INT,
    @isUser BIT,
    @requestNote NVARCHAR(MAX),
    @payload NVARCHAR(MAX),
    @returnPendingStatus NVARCHAR(50),
    @borrowingStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE BorrowTickets 
    SET Status = @returnPendingStatus,
        ReturnNote = @payload,
        Note = CASE WHEN ISNULL(Note, '') = '' THEN @requestNote ELSE ISNULL(Note, '') + ' | ' + @requestNote END
    WHERE TicketID = @ticketId 
      AND Status = @borrowingStatus
      AND (@isUser = 0 OR UserID = @currentUserId);
    
    SELECT @@ROWCOUNT;
END
GO

