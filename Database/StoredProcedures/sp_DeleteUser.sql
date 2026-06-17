-- 7. THỦ TỤC XÓA NGƯỜI DÙNG (KHOÁ HOẠT ĐỘNG)
CREATE OR ALTER PROCEDURE [dbo].[sp_DeleteUser]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users
    SET IsActive = 0,
        IsLocked = 1
    WHERE UserID = @id;
END
GO

