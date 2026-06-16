-- 22. THỦ TỤC LẤY DANH SÁCH THIẾT BỊ KHẢ DỤNG ĐỂ MƯỢN
CREATE OR ALTER PROCEDURE [dbo].[sp_GetAvailableDevices]
    @maintenanceStatus NVARCHAR(50),
    @retiredStatus NVARCHAR(50),
    @retiredRoomStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        d.DeviceID,
        d.DeviceName,
        dc.CategoryName,
        ISNULL(r.RoomName, N'') AS RoomName,
        d.Status,
        d.AvailableQuantity,
        ISNULL(d.Note, N'') AS Note
    FROM dbo.Devices d
    INNER JOIN dbo.DeviceCategories dc 
        ON d.CategoryID = dc.CategoryID
    LEFT JOIN dbo.Rooms r 
        ON d.RoomID = r.RoomID
    WHERE d.AvailableQuantity > 0
      AND d.Status NOT IN (@maintenanceStatus, @retiredStatus)
      AND ISNULL(r.Status, N'Hoạt động') <> @retiredRoomStatus
    ORDER BY d.DeviceID;
END
GO

