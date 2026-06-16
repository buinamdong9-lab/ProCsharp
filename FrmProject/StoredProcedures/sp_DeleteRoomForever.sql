-- 72. THỦ TỤC XÓA VĨNH VIỄN PHÒNG HỌC
CREATE OR ALTER PROCEDURE [dbo].[sp_DeleteRoomForever]
    @id INT,
    @retiredStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Devices WHERE RoomID = @id)
        BEGIN
            THROW 50010, N'Không thể xóa vĩnh viễn phòng vì vẫn còn thiết bị gắn với phòng này.', 1;
        END

        DELETE FROM Rooms WHERE RoomID = @id AND Status = @retiredStatus;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

