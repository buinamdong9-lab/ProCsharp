-- 69. THỦ TỤC KHÔI PHỤC NGƯỜI DÙNG
CREATE OR ALTER PROCEDURE [dbo].[sp_RestoreUser]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users SET IsActive = 1, IsLocked = 0 WHERE UserID = @id;
END
GO

