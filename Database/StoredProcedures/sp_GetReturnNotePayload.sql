-- 47. THỦ TỤC LẤY CHUỖI RETURN NOTE TỪ PHIẾU MƯỢN
CREATE OR ALTER PROCEDURE [dbo].[sp_GetReturnNotePayload]
    @ticketId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ReturnNote FROM BorrowTickets WHERE TicketID = @ticketId;
END
GO

