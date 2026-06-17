-- =========================================================================
-- LỚP PHIẾU TRẢ (RETURN TICKET & RETURN REQUEST REPOSITORIES)
-- =========================================================================

-- 38. THỦ TỤC TÌM KIẾM PHIẾU ĐANG MƯỢN ĐỂ TRẢ
CREATE OR ALTER PROCEDURE [dbo].[sp_SearchBorrowingTickets]
    @userId INT,
    @isUser BIT,
    @keyword NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        bt.TicketID, 
        ISNULL(bt.TicketCode, 'PM' + CONVERT(VARCHAR, bt.TicketID)) + ' - ' + u.FullName + ' (' + CONVERT(VARCHAR, bt.BorrowDate, 103) + ')' AS DisplayText
    FROM BorrowTickets bt
    JOIN Users u ON u.UserID = bt.UserID
    WHERE bt.Status IN ('BORROWING', 'RETURN_PENDING')
      AND (@isUser = 0 OR bt.UserID = @userId)
      AND (@keyword IS NULL OR @keyword = '' OR 
           CONVERT(VARCHAR, bt.TicketID) LIKE '%' + @keyword + '%' OR 
           bt.TicketCode LIKE '%' + @keyword + '%' OR 
           u.FullName LIKE '%' + @keyword + '%')
    ORDER BY bt.BorrowDate DESC;
END
GO

