-- 54. THỦ TỤC LẤY DANH SÁCH THIẾT BỊ ĐANG ĐƯỢC MƯỢN PHÂN TRANG (DASHBOARD)
CREATE OR ALTER PROCEDURE [dbo].[sp_LoadDashboardBorrowingList]
    @offset INT,
    @limit INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        ISNULL(NULLIF(di.AssetCode, ''), ISNULL(NULLIF(d.DeviceCode, ''), 'TB' + CONVERT(VARCHAR, d.DeviceID))) AS [Mã TB],
        d.DeviceName          AS [Tên Thiết Bị],
        (bd.Quantity - bd.ReturnedQuantity) AS [Số lượng],
        u.FullName            AS [Người mượn],
        bt.BorrowDate         AS [Ngày Mượn],
        bt.ExpectedReturnDate AS [Hạn Trả]
    FROM dbo.BorrowTickets bt
    JOIN dbo.Users u ON u.UserID = bt.UserID
    JOIN dbo.BorrowDetails bd ON bd.TicketID = bt.TicketID
    JOIN dbo.Devices d ON d.DeviceID = bd.DeviceID
    LEFT JOIN dbo.DeviceInstances di ON di.InstanceID = bd.InstanceID
    WHERE bt.Status IN ('BORROWING', 'RETURN_PENDING')
      AND bd.Quantity > bd.ReturnedQuantity
    ORDER BY bt.BorrowDate DESC
    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
END
GO

