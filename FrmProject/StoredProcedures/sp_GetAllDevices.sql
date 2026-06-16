-- =========================================================================
-- LỚP DEVICE (DEVICE & DEVICE INSTANCE REPOSITORY)
-- =========================================================================

-- 15. THỦ TỤC LẤY TẤT CẢ THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetAllDevices]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        d.DeviceID,
        d.DeviceCode,
        d.DeviceName        AS [Tên thiết bị],
        d.DeviceCode        AS [Mã thiết bị],
        dc.CategoryName     AS [Loại thiết bị],
        ISNULL(r.RoomName, '') AS [Vị trí],
        d.Status            AS [Tình trạng],
        d.TotalQuantity     AS [Số lượng],
        d.AvailableQuantity AS AvailableQuantity,
        ISNULL(d.Note, '')  AS [Ghi chú]
    FROM dbo.Devices d
    JOIN dbo.DeviceCategories dc ON dc.CategoryID = d.CategoryID
    LEFT JOIN dbo.Rooms r ON r.RoomID = d.RoomID
    ORDER BY d.DeviceID;
END
GO

