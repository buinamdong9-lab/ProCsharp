-- 20. THỦ TỤC XÓA (VÀ XÓA MỀM) THIẾT BỊ & CÁC CÁ THỂ THIẾT BỊ KHẢ DỤNG
CREATE OR ALTER PROCEDURE [dbo].[sp_DeleteDevice]
    @id INT,
    @note NVARCHAR(500),
    @retiredStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE dbo.Devices
        SET Status = @retiredStatus,
            AvailableQuantity = 0,
            Note = LTRIM(RTRIM(CONCAT(ISNULL(NULLIF(Note, ''), ''), CASE WHEN ISNULL(NULLIF(Note, ''), '') = '' THEN '' ELSE ' | ' END, @note)))
        WHERE DeviceID = @id;

        UPDATE dbo.DeviceInstances
        SET Status = @retiredStatus,
            Note = LTRIM(RTRIM(CONCAT(ISNULL(NULLIF(Note, ''), ''), CASE WHEN ISNULL(NULLIF(Note, ''), '') = '' THEN '' ELSE ' | ' END, CONCAT(N'Xóa mềm theo thiết bị ', @note))))
        WHERE DeviceID = @id
          AND Status = @availableStatus;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

