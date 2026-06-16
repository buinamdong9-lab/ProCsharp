-- 50. THỦ TỤC CHI TIẾT PHIẾU MƯỢN TRẢ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetTicketDetail]
    @ticketId INT,
    @currentUserId INT,
    @isUser BIT,
    @returnedStatus NVARCHAR(50),
    @rejectedStatus NVARCHAR(50),
    @returnPendingStatus NVARCHAR(50),
    @borrowingStatus NVARCHAR(50),
    @pendingStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT ISNULL(bt.TicketCode, CAST(bt.TicketID AS VARCHAR)) AS TicketDisplay,
           u.FullName AS BorrowerName,
           ISNULL(roomInfo.RoomName, '') AS RoomName,
           ISNULL(bt.Purpose, '') AS Purpose,
           bt.BorrowDate,
           bt.ExpectedReturnDate,
           ISNULL(approver.FullName, '') AS ApprovedByName,
           bt.ReturnDate,
           CASE
               WHEN bt.Status = @returnedStatus OR bt.ReturnDate IS NOT NULL THEN N'Đã trả'
               WHEN bt.Status = @rejectedStatus THEN N'Từ chối'
               WHEN bt.Status = @returnPendingStatus THEN N'Chờ duyệt trả'
               WHEN bt.Status = @borrowingStatus AND bt.ExpectedReturnDate < GETDATE() THEN N'Quá hạn'
               WHEN bt.Status = @borrowingStatus THEN N'Đang mượn'
               WHEN bt.Status = @pendingStatus THEN N'Chờ duyệt'
               ELSE bt.Status
           END AS StatusText,
           CASE
               WHEN bt.ExpectedReturnDate < GETDATE() AND bt.Status IN (@borrowingStatus, @returnPendingStatus)
               THEN DATEDIFF(DAY, bt.ExpectedReturnDate, GETDATE())
               ELSE 0
           END AS OverdueDays
    FROM BorrowTickets bt
    JOIN Users u ON u.UserID = bt.UserID
    LEFT JOIN Users approver ON approver.UserID = bt.ReturnedBy
    OUTER APPLY (
        SELECT TOP 1 ISNULL(r.RoomName, '') AS RoomName
        FROM BorrowDetails bd
        JOIN Devices d ON d.DeviceID = bd.DeviceID
        LEFT JOIN Rooms r ON r.RoomID = d.RoomID
        WHERE bd.TicketID = bt.TicketID
        ORDER BY r.RoomName
    ) roomInfo
    WHERE bt.TicketID = @ticketId
      AND (@isUser = 0 OR bt.UserID = @currentUserId);
END
GO

