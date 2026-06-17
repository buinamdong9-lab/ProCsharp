-- 75. THỦ TỤC THỐNG KÊ TOP 10 THIẾT BỊ ĐƯỢC MƯỢN NHIỀU NHẤT
CREATE OR ALTER PROCEDURE [dbo].[sp_GetReportTopDevices]
    @from DATETIME,
    @to DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 10
        ROW_NUMBER() OVER (ORDER BY SUM(bd.Quantity) DESC) AS [Hạng],
        d.DeviceName AS [Thiết bị],
        SUM(bd.Quantity) AS [Lượt mượn],
        ISNULL(CAST(ROUND(SUM(bd.Quantity) * 100.0 / 
            NULLIF((SELECT ISNULL(SUM(bd2.Quantity), 0)
                    FROM BorrowDetails bd2
                    JOIN BorrowTickets bt2 ON bt2.TicketID = bd2.TicketID
                    WHERE bt2.BorrowDate BETWEEN @from AND @to), 0), 1) AS VARCHAR) + '%', '0%') AS [Tỉ lệ]
    FROM BorrowDetails bd
    JOIN BorrowTickets bt ON bt.TicketID = bd.TicketID
    JOIN Devices d ON d.DeviceID = bd.DeviceID
    WHERE bt.BorrowDate BETWEEN @from AND @to
    GROUP BY d.DeviceName
    ORDER BY [Lượt mượn] DESC;
END
GO

