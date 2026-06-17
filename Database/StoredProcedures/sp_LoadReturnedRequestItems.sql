-- 52. THỦ TỤC LẤY CHI TIẾT YÊU CẦU TRẢ ĐÃ THÀNH CÔNG
CREATE OR ALTER PROCEDURE [dbo].[sp_LoadReturnedRequestItems]
    @ticketId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT ISNULL(di.AssetCode, ISNULL(d.DeviceCode, '')) AS [Mã TB],
           d.DeviceName AS [Tên thiết bị],
           rd.ReturnQuantity AS [Số lượng],
           ISNULL(NULLIF(di.Condition, ''), N'Tốt') AS [Tình trạng khi mượn],
           ISNULL(NULLIF(rd.Note, ''), N'Tốt') AS [Tình trạng khi trả]
    FROM ReturnRequests rr
    JOIN ReturnRequestDetails rd ON rd.ReturnRequestID = rr.ReturnRequestID
    JOIN Devices d ON d.DeviceID = rd.DeviceID
    LEFT JOIN DeviceInstances di ON di.InstanceID = rd.InstanceID
    WHERE rr.TicketID = @ticketId
    ORDER BY d.DeviceName, di.AssetCode;
END
GO

