-- 66. THỦ TỤC KHÔI PHỤC THIẾT BỊ ĐÃ XÓA
CREATE OR ALTER PROCEDURE [dbo].[sp_RestoreDevice]
    @id INT,
    @goodStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50),
    @retiredStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE Devices SET Status = @goodStatus WHERE DeviceID = @id;
        
        UPDATE DeviceInstances
        SET Status = @availableStatus
        WHERE DeviceID = @id
          AND Status = @retiredStatus
          AND ISNULL(Note, '') LIKE N'%Xóa mềm theo thiết bị%';

        -- Cập nhật lại số lượng
        UPDATE Devices
        SET TotalQuantity = (SELECT COUNT(*) FROM DeviceInstances WHERE DeviceID = @id AND Status <> @retiredStatus),
            AvailableQuantity = (
                SELECT COUNT(*)
                FROM DeviceInstances
                WHERE DeviceID = @id
                  AND Status = @availableStatus
                  AND ISNULL(NULLIF(Condition, ''), N'Tốt') = N'Tốt'
            )
        WHERE DeviceID = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

