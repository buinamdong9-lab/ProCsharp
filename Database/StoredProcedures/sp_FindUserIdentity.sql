-- 6. THỦ TỤC TÌM KIẾM DANH TÍNH NGƯỜI DÙNG
CREATE OR ALTER PROCEDURE [dbo].[sp_FindUserIdentity]
    @code NVARCHAR(100),
    @username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 UserID, Username
    FROM Users
    WHERE UserCode = @code OR Username = @username
    ORDER BY CASE WHEN UserCode = @code THEN 0 ELSE 1 END;
END
GO

