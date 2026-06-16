-- 31. THỦ TỤC CẬP NHẬT SỐ LƯỢNG THIẾT BỊ (THEO DÕI TỪ BLL HOẶC KHÁC)
CREATE OR ALTER PROCEDURE [dbo].[sp_UpdateDeviceQuantities]
    @id INT,
    @retiredStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50),
    @goodCondition NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Devices 
    SET TotalQuantity = (SELECT COUNT(*) FROM dbo.DeviceInstances WHERE DeviceID = @id AND Status <> @retiredStatus),
        AvailableQuantity = (
            SELECT COUNT(*)
            FROM dbo.DeviceInstances
            WHERE DeviceID = @id
              AND Status = @availableStatus
              AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
        )
    WHERE DeviceID = @id;
END
GO

