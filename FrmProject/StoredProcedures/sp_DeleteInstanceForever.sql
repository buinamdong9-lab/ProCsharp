-- 71. THỦ TỤC XÓA VĨNH VIỄN CÁ THỂ THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_DeleteInstanceForever]
    @id INT,
    @retiredStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM BorrowDetails WHERE InstanceID = @id)
        BEGIN
            THROW 50009, N'Không thể xóa vĩnh viễn mã cá thể vì còn lịch sử phiếu mượn.', 1;
        END

        DECLARE @deviceId INT;
        SELECT @deviceId = DeviceID FROM DeviceInstances WHERE InstanceID = @id;
        
        DELETE FROM DeviceInstances WHERE InstanceID = @id AND Status = @retiredStatus;

        -- Cập nhật lại số lượng thiết bị
        IF @deviceId IS NOT NULL
        BEGIN
            UPDATE Devices
            SET TotalQuantity = (SELECT COUNT(*) FROM DeviceInstances WHERE DeviceID = @deviceId AND Status <> @retiredStatus),
                AvailableQuantity = (
                    SELECT COUNT(*)
                    FROM DeviceInstances
                    WHERE DeviceID = @deviceId
                      AND Status = @availableStatus
                      AND ISNULL(NULLIF(Condition, ''), N'Tốt') = N'Tốt'
                )
            WHERE DeviceID = @deviceId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

