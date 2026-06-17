-- 23. THỦ TỤC LẤY THIẾT BỊ THEO PHÒNG
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDevicesByRoom]
    @roomCode NVARCHAR(100),
    @retiredStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        d.DeviceCode        AS [Mã TB],
        d.DeviceName        AS [Tên thiết bị],
        d.TotalQuantity     AS [SL],
        d.Status            AS [Tình trạng]
    FROM Devices d
    JOIN Rooms r ON d.RoomID = r.RoomID
    WHERE r.RoomCode = @roomCode
      AND d.Status <> @retiredStatus;
END
GO

