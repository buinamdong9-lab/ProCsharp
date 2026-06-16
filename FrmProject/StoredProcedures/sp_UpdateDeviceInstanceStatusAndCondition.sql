-- 30. THỦ TỤC CẬP NHẬT TRẠNG THÁI CÁ THỂ THIẾT BỊ & CẬP NHẬT SỐ LƯỢNG LOẠI
CREATE OR ALTER PROCEDURE [dbo].[sp_UpdateDeviceInstanceStatusAndCondition]
    @deviceId INT,
    @instanceId INT,
    @status NVARCHAR(50),
    @condition NVARCHAR(50),
    @retiredStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50),
    @goodCondition NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE dbo.DeviceInstances
        SET Status = @status,
            Condition = @condition
        WHERE InstanceID = @instanceId
          AND DeviceID = @deviceId;

        IF @@ROWCOUNT = 0
        BEGIN
            THROW 50005, N'Không tìm thấy mã cá thể cần cập nhật.', 1;
        END

        -- Cập nhật số lượng thiết bị
        UPDATE dbo.Devices 
        SET TotalQuantity = (SELECT COUNT(*) FROM dbo.DeviceInstances WHERE DeviceID = @deviceId AND Status <> @retiredStatus),
            AvailableQuantity = (
                SELECT COUNT(*)
                FROM dbo.DeviceInstances
                WHERE DeviceID = @deviceId
                  AND Status = @availableStatus
                  AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
            )
        WHERE DeviceID = @deviceId;

        COMMIT TRANSACTION;
   END TRY
   BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
   END CATCH
END
GO

