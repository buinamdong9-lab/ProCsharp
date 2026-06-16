-- 70. THỦ TỤC XÓA VĨNH VIỄN THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_DeleteDeviceForever]
    @id INT,
    @retiredStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM BorrowDetails WHERE DeviceID = @id)
        BEGIN
            THROW 50008, N'Không thể xóa vĩnh viễn thiết bị vì còn lịch sử phiếu mượn.', 1;
        END

        DELETE FROM DeviceInstances WHERE DeviceID = @id;
        DELETE FROM Devices WHERE DeviceID = @id AND Status = @retiredStatus;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

