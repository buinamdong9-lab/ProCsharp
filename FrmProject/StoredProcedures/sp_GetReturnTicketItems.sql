-- 40. THỦ TỤC LẤY DANH SÁCH CHI TIẾT THIẾT BỊ MƯỢN ĐỂ TRẢ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetReturnTicketItems]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT bd.DeviceID,
           bd.InstanceID,
           di.AssetCode AS [Mã tài sản],
           d.DeviceName AS [Tên thiết bị],
           (bd.Quantity - bd.ReturnedQuantity) AS [SL mượn],
           (bd.Quantity - bd.ReturnedQuantity) AS [SL trả],
           ISNULL(NULLIF(di.Condition, ''), N'Tốt') AS [Tình trạng khi mượn],
           N'Tốt' AS [Tình trạng khi trả],
           ISNULL(bd.Note, '') AS [Ghi chú]
    FROM BorrowDetails bd
    JOIN Devices d ON d.DeviceID = bd.DeviceID
    LEFT JOIN DeviceInstances di ON di.InstanceID = bd.InstanceID
    WHERE bd.TicketID = @id AND bd.Quantity > bd.ReturnedQuantity;
END
GO

