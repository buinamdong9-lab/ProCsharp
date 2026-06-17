-- 51. THỦ TỤC LẤY CHI TIẾT THIẾT BỊ MƯỢN CỦA PHIẾU
CREATE OR ALTER PROCEDURE [dbo].[sp_LoadBorrowDetailItems]
    @ticketId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT ISNULL(di.AssetCode, ISNULL(d.DeviceCode, '')) AS [Mã TB],
           d.DeviceName AS [Tên thiết bị],
           bd.Quantity AS [OriginalQuantity],
           bd.Quantity AS [Số lượng],
           ISNULL(NULLIF(di.Condition, ''), N'Tốt') AS [Tình trạng khi mượn],
           ISNULL(returnInfo.ReturnCondition, '') AS [Tình trạng khi trả]
    FROM BorrowDetails bd
    JOIN Devices d ON d.DeviceID = bd.DeviceID
    LEFT JOIN DeviceInstances di ON di.InstanceID = bd.InstanceID
    OUTER APPLY (
        SELECT TOP 1 ISNULL(rd.Note, '') AS ReturnCondition
        FROM ReturnRequests rr
        JOIN ReturnRequestDetails rd ON rd.ReturnRequestID = rr.ReturnRequestID
        WHERE rr.TicketID = bd.TicketID
          AND rd.DeviceID = bd.DeviceID
          AND ISNULL(rd.InstanceID, 0) = ISNULL(bd.InstanceID, 0)
        ORDER BY rd.ReturnRequestDetailID DESC
    ) returnInfo
    WHERE bd.TicketID = @ticketId
    ORDER BY d.DeviceName, di.AssetCode;
END
GO

