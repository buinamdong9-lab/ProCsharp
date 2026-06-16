-- 89. THỦ TỤC LẤY CHỦ SỞ HỮU PHIẾU MƯỢN
CREATE OR ALTER PROCEDURE [dbo].[sp_GetTicketOwnerId]
    @ticketId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UserID FROM BorrowTickets WHERE TicketID = @ticketId;
END
GO

