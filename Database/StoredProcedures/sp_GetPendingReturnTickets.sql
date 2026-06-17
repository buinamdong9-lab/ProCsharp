-- 87. THỦ TỤC LẤY DANH SÁCH YÊU CẦU TRẢ ĐANG CHỜ DUYỆT CHO ADMIN
CREATE OR ALTER PROCEDURE [dbo].[sp_GetPendingReturnTickets]
    @status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT bt.TicketID,
           ISNULL(bt.TicketCode, CAST(bt.TicketID AS VARCHAR)) AS [Số phiếu],
           u.FullName AS [Người mượn],
           CONVERT(VARCHAR, bt.BorrowDate, 103) AS [Ngày mượn],
           CONVERT(VARCHAR, bt.ExpectedReturnDate, 103) AS [Hạn trả],
           ISNULL(bt.Note, '') AS [Ghi chú]
    FROM BorrowTickets bt
    JOIN Users u ON u.UserID = bt.UserID
    WHERE bt.Status = @status
    ORDER BY bt.BorrowDate DESC;
END
GO

