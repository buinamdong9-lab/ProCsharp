-- 55. THỦ TỤC LẤY THỐNG KÊ MƯỢN TRẢ THEO THÁNG (DASHBOARD)
CREATE OR ALTER PROCEDURE [dbo].[sp_LoadDashboardMonthlyStats]
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @from DATE = DATEFROMPARTS(YEAR(DATEADD(MONTH, -5, GETDATE())), MONTH(DATEADD(MONTH, -5, GETDATE())), 1);

    SELECT FORMAT(bt.BorrowDate, 'MM/yyyy') AS [Tháng],
           COUNT(DISTINCT bt.TicketID) AS [Số phiếu],
           ISNULL(SUM(bd.Quantity), 0) AS [Số thiết bị]
    FROM dbo.BorrowTickets bt
    LEFT JOIN dbo.BorrowDetails bd ON bd.TicketID = bt.TicketID
    WHERE bt.BorrowDate >= @from
    GROUP BY FORMAT(bt.BorrowDate, 'MM/yyyy'), YEAR(bt.BorrowDate), MONTH(bt.BorrowDate)
    ORDER BY YEAR(bt.BorrowDate), MONTH(bt.BorrowDate);
END
GO

