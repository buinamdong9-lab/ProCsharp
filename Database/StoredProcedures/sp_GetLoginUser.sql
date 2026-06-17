-- 2. THỦ TỤC LẤY THÔNG TIN ĐĂNG NHẬP
CREATE OR ALTER PROCEDURE [dbo].[sp_GetLoginUser]
    @username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT u.UserID,
           u.FullName,
           ISNULL(r.RoleName, 'User') AS RoleName,
           ISNULL(u.PasswordHash, '') AS PasswordHash,
           ISNULL(u.FailedLoginCount, 0) AS FailedLoginCount,
           u.LockoutUntil,
           ISNULL(u.IsActive, 1) AS IsActive,
           ISNULL(u.IsLocked, 0) AS IsLocked
    FROM Users u
    LEFT JOIN Roles r ON u.RoleID = r.RoleID
    WHERE u.Username = @username;
END
GO

