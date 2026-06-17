-- 34. THỦ TỤC LẤY CÁC THIẾT BỊ CÓ THỂ CHO MƯỢN
CREATE OR ALTER PROCEDURE [dbo].[sp_GetBorrowableDevices]
    @maintenanceStatus NVARCHAR(50),
    @retiredStatus NVARCHAR(50),
    @retiredRoomStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50),
    @goodCondition NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT d.DeviceID, d.DeviceName + ' (' + ISNULL(d.DeviceCode, '') + ')' AS DisplayName
    FROM Devices d
    LEFT JOIN Rooms r ON r.RoomID = d.RoomID
    WHERE d.Status NOT IN (@maintenanceStatus, @retiredStatus)
      AND ISNULL(r.Status, N'Hoạt động') <> @retiredRoomStatus
      AND EXISTS (
          SELECT 1
          FROM DeviceInstances di
          WHERE di.DeviceID = d.DeviceID
            AND di.Status = @availableStatus
            AND ISNULL(NULLIF(di.Condition, ''), @goodCondition) = @goodCondition
      )
    ORDER BY d.DeviceName;
END
GO

