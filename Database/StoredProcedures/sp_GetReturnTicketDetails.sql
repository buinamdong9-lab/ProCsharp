-- 39. THỦ TỤC LẤY THÔNG TIN PHIẾU TRẢ (CHI TIẾT PHIẾU MASTER)
CREATE OR ALTER PROCEDURE [dbo].[sp_GetReturnTicketDetails]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        ISNULL(bt.TicketCode, CAST(bt.TicketID AS VARCHAR)) AS TicketDisplay, 
        u.FullName, 
        bt.BorrowDate, 
        bt.ExpectedReturnDate, 
        bt.Status, 
        bt.ReturnNote
    FROM BorrowTickets bt
    JOIN Users u ON u.UserID = bt.UserID
    WHERE bt.TicketID = @id;
END
GO

