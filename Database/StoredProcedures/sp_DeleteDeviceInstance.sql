-- 29. THỦ TỤC XÓA CÁ THỂ THIẾT BỊ & CẬP NHẬT SỐ LƯỢNG LOẠI
CREATE OR ALTER PROCEDURE [dbo].[sp_DeleteDeviceInstance]
    @deviceId INT,
    @instanceId INT,
    @note NVARCHAR(500),
    @retiredStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50),
    @goodCondition NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Kiểm tra xem có đang bị mượn không
        DECLARE @currentStatus NVARCHAR(50);
        SELECT @currentStatus = Status FROM dbo.DeviceInstances WHERE InstanceID = @instanceId AND DeviceID = @deviceId;
        
        IF @currentStatus = N'Đang mượn'
        BEGIN
            THROW 50004, N'Mã cá thể đang được mượn. Hãy xử lý trả thiết bị trước khi xóa mềm.', 1;
        END

        UPDATE dbo.DeviceInstances
        SET Status = @retiredStatus,
            Note = LTRIM(RTRIM(CONCAT(ISNULL(NULLIF(Note, ''), ''), CASE WHEN ISNULL(NULLIF(Note, ''), '') = '' THEN '' ELSE ' | ' END, @note)))
        WHERE InstanceID = @instanceId
          AND DeviceID = @deviceId;

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

