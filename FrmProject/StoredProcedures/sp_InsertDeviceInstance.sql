-- 28. THỦ TỤC THÊM MỚI CÁ THỂ THIẾT BỊ & CẬP NHẬT SỐ LƯỢNG LOẠI
CREATE OR ALTER PROCEDURE [dbo].[sp_InsertDeviceInstance]
    @devId INT,
    @code NVARCHAR(100),
    @status NVARCHAR(50),
    @cond NVARCHAR(50),
    @retiredStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50),
    @goodCondition NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO dbo.DeviceInstances (DeviceID, AssetCode, Status, Condition) 
        VALUES (@devId, @code, @status, @cond);

        -- Cập nhật số lượng thiết bị
        UPDATE dbo.Devices 
        SET TotalQuantity = (SELECT COUNT(*) FROM dbo.DeviceInstances WHERE DeviceID = @devId AND Status <> @retiredStatus),
            AvailableQuantity = (
                SELECT COUNT(*)
                FROM dbo.DeviceInstances
                WHERE DeviceID = @devId
                  AND Status = @availableStatus
                  AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
            )
        WHERE DeviceID = @devId;

        COMMIT TRANSACTION;
   END TRY
   BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
   END CATCH
END
GO

