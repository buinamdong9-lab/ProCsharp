-- 10. THỦ TỤC ĐỔI MẬT KHẨU
CREATE OR ALTER PROCEDURE [dbo].[sp_ResetPassword]
    @id INT,
    @hash NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users 
    SET PasswordHash = @hash 
    WHERE UserID = @id;
END
GO

