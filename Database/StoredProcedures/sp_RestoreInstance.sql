-- 67. THỦ TỤC KHÔI PHỤC CÁ THỂ THIẾT BỊ ĐÃ XÓA
CREATE OR ALTER PROCEDURE [dbo].[sp_RestoreInstance]
    @id INT,
    @availableStatus NVARCHAR(50),
    @retiredStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @deviceId INT;
        SELECT @deviceId = DeviceID FROM DeviceInstances WHERE InstanceID = @id;
        IF @deviceId IS NULL
        BEGIN
            THROW 50006, N'Không tìm thấy mã cá thể cần xử lý.', 1;
        END

        DECLARE @deviceStatus NVARCHAR(50);
        SELECT @deviceStatus = ISNULL(Status, '') FROM Devices WHERE DeviceID = @deviceId;
        IF @deviceStatus = @retiredStatus
        BEGIN
            THROW 50007, N'Thiết bị cha đang ở thùng rác. Hãy khôi phục thiết bị trước.', 1;
        END

        UPDATE DeviceInstances SET Status = @availableStatus WHERE InstanceID = @id;

        -- Cập nhật lại số lượng thiết bị
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

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

