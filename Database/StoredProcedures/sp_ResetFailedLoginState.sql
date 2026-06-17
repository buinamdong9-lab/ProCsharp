-- 3. THỦ TỤC RESET TRẠNG THÁI ĐĂNG NHẬP SAI
CREATE OR ALTER PROCEDURE [dbo].[sp_ResetFailedLoginState]
    @userId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Users 
    SET FailedLoginCount = 0, LockoutUntil = NULL 
    WHERE UserID = @userId;
END
GO

