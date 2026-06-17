-- 4. THỦ TỤC CẬP NHẬT TRẠNG THÁI ĐĂNG NHẬP SAI & KHÓA TÀI KHOẢN
CREATE OR ALTER PROCEDURE [dbo].[sp_RegisterFailedLogin]
    @userId INT,
    @failedCount INT,
    @lockoutUntil DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Users 
    SET FailedLoginCount = @failedCount, LockoutUntil = @lockoutUntil 
    WHERE UserID = @userId;
END
GO

