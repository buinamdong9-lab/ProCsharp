-- 76. THỦ TỤC LẤY DANH SÁCH PHIẾU QUÁ HẠN CHO BÁO CÁO
CREATE OR ALTER PROCEDURE [dbo].[sp_GetReportOverdueTickets]
    @from DATETIME,
    @to DATETIME,
    @borrowingStatus NVARCHAR(50),
    @returnPendingStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        ISNULL(bt.TicketCode, CAST(bt.TicketID AS VARCHAR)) AS [Số phiếu],
        u.FullName AS [Người mượn],
        ISNULL(roomInfo.RoomName, '') AS [Phòng],
        CONVERT(VARCHAR, bt.BorrowDate, 103) AS [Ngày mượn],
        CONVERT(VARCHAR, bt.ExpectedReturnDate, 103) AS [Hạn trả],
        DATEDIFF(DAY, bt.ExpectedReturnDate, GETDATE()) AS [Số ngày quá hạn],
        N'Cần xử lý' AS [Tình trạng]
    FROM BorrowTickets bt
    JOIN Users u ON u.UserID = bt.UserID
    OUTER APPLY (
        SELECT TOP 1 ISNULL(r.RoomName, '') AS RoomName
        FROM BorrowDetails bd
        JOIN Devices d ON d.DeviceID = bd.DeviceID
        LEFT JOIN Rooms r ON r.RoomID = d.RoomID
        WHERE bd.TicketID = bt.TicketID
        ORDER BY r.RoomName
    ) roomInfo
    WHERE bt.Status IN (@borrowingStatus, @returnPendingStatus)
      AND bt.BorrowDate BETWEEN @from AND @to
      AND bt.ExpectedReturnDate < GETDATE()
    ORDER BY bt.ExpectedReturnDate;
END
GO

