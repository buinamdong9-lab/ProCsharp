-- 90. THỦ TỤC LẤY TRẠNG THÁI PHIẾU MƯỢN
CREATE OR ALTER PROCEDURE [dbo].[sp_GetTicketStatus]
    @ticketId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Status FROM BorrowTickets WHERE TicketID = @ticketId;
END
GO

