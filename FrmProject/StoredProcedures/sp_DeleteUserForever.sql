-- 73. THỦ TỤC XÓA VĨNH VIỄN NGƯỜI DÙNG
CREATE OR ALTER PROCEDURE [dbo].[sp_DeleteUserForever]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM BorrowTickets WHERE UserID = @id OR CreatedBy = @id OR ReturnedBy = @id)
        BEGIN
            THROW 50011, N'Không thể xóa vĩnh viễn người dùng vì còn lịch sử phiếu mượn/trả.', 1;
        END

        DELETE FROM Users WHERE UserID = @id AND ISNULL(IsActive, 1) = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

