-- 74. THỦ TỤC THỐNG KÊ MƯỢN TRẢ THEO THÁNG
CREATE OR ALTER PROCEDURE [dbo].[sp_GetReportMonthlyStats]
    @from DATETIME,
    @to DATETIME,
    @returnedStatus NVARCHAR(50),
    @borrowingStatus NVARCHAR(50),
    @returnPendingStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        FORMAT(bt.BorrowDate, 'MM/yyyy') AS [Tháng],
        COUNT(DISTINCT bt.TicketID) AS [Số phiếu mượn],
        SUM(tQty.Qty) AS [Tổng SL mượn],
        SUM(CASE WHEN bt.Status = @returnedStatus OR bt.ReturnDate IS NOT NULL THEN tQty.Qty ELSE 0 END) AS [Đã trả],
        SUM(CASE WHEN bt.Status IN (@borrowingStatus, @returnPendingStatus) AND bt.ExpectedReturnDate < GETDATE() AND bt.ReturnDate IS NULL
            THEN tQty.Qty ELSE 0 END) AS [Quá hạn]
    FROM BorrowTickets bt
    CROSS APPLY (
        SELECT COALESCE(
            (SELECT SUM(bd2.Quantity) FROM BorrowDetails bd2 WHERE bd2.TicketID = bt.TicketID),
            (SELECT SUM(rrd.ReturnQuantity)
             FROM ReturnRequests rr
             JOIN ReturnRequestDetails rrd ON rrd.ReturnRequestID = rr.ReturnRequestID
             WHERE rr.TicketID = bt.TicketID),
            0
        ) AS Qty
    ) tQty
    WHERE bt.BorrowDate BETWEEN @from AND @to
    GROUP BY FORMAT(bt.BorrowDate, 'MM/yyyy'), YEAR(bt.BorrowDate), MONTH(bt.BorrowDate)
    ORDER BY YEAR(bt.BorrowDate), MONTH(bt.BorrowDate);
END
GO

