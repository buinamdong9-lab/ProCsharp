-- =========================================================================
-- LỚP THỐNG KÊ DANH SÁCH PHIẾU (TICKET LIST REPOSITORY)
-- =========================================================================

-- 48. THỦ TỤC TÌM KIẾM PHIẾU MƯỢN TRẢ
CREATE OR ALTER PROCEDURE [dbo].[sp_SearchTickets]
    @from DATETIME,
    @to DATETIME,
    @keyword NVARCHAR(200) = NULL,
    @statusFilter NVARCHAR(100) = NULL,
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
    
    SELECT 
        bt.TicketID AS [ID phiếu],
        ISNULL(bt.TicketCode, CAST(bt.TicketID AS VARCHAR)) AS [Số phiếu],
        CONVERT(VARCHAR, bt.BorrowDate, 103) AS [Ngày lập],
        u.FullName AS [Người mượn],
        ISNULL(roomInfo.RoomName, '') AS [Phòng],
        ISNULL((SELECT SUM(bd2.Quantity) FROM BorrowDetails bd2 WHERE bd2.TicketID = bt.TicketID), 0) AS [Số lượng],
        CONVERT(VARCHAR, bt.BorrowDate, 103) AS [Ngày mượn],
        CONVERT(VARCHAR, bt.ExpectedReturnDate, 103) AS [Hạn trả],
        CASE WHEN bt.ReturnDate IS NOT NULL 
             THEN CONVERT(VARCHAR, bt.ReturnDate, 103) 
             ELSE '' END AS [Ngày trả],
        CASE 
            WHEN bt.Status = @returnedStatus OR bt.ReturnDate IS NOT NULL THEN N'Đã trả'
            WHEN bt.Status = @rejectedStatus THEN N'Từ chối'
            WHEN bt.Status = @returnPendingStatus THEN N'Chờ duyệt trả'
            WHEN bt.Status = @borrowingStatus AND bt.ExpectedReturnDate < GETDATE() THEN N'Quá hạn'
            WHEN bt.Status = @borrowingStatus THEN N'Đang mượn'
            WHEN bt.Status = @pendingStatus THEN N'Chờ duyệt'
            ELSE bt.Status
        END AS [Trạng thái],
        bt.Note AS [Ghi chú]
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
    WHERE bt.BorrowDate BETWEEN @from AND @to
      AND (@isUser = 0 OR bt.UserID = @currentUserId)
      AND (@keyword IS NULL OR @keyword = '' OR 
           CAST(bt.TicketID AS VARCHAR) LIKE '%' + @keyword + '%' OR 
           ISNULL(bt.TicketCode, '') LIKE '%' + @keyword + '%' OR 
           (@isUser = 0 AND u.FullName LIKE '%' + @keyword + '%'))
      AND (
           @statusFilter IS NULL OR @statusFilter = '' OR @statusFilter = N'Tất cả' OR
           (@statusFilter = N'Đang mượn' AND bt.Status IN (@borrowingStatus, @returnPendingStatus) AND bt.ExpectedReturnDate >= GETDATE() AND bt.ReturnDate IS NULL) OR
           (@statusFilter = N'Quá hạn' AND bt.Status IN (@borrowingStatus, @returnPendingStatus) AND bt.ExpectedReturnDate < GETDATE() AND bt.ReturnDate IS NULL) OR
           (@statusFilter = N'Đã trả' AND (bt.Status = @returnedStatus OR bt.ReturnDate IS NOT NULL)) OR
           (@statusFilter = N'Chờ duyệt' AND bt.Status = @pendingStatus) OR
           (@statusFilter = N'Chờ duyệt trả' AND bt.Status = @returnPendingStatus AND bt.ReturnDate IS NULL) OR
           (@statusFilter = N'Từ chối' AND bt.Status = @rejectedStatus)
      )
    ORDER BY bt.BorrowDate DESC;
END
GO

