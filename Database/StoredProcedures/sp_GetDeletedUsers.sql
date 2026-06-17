-- 65. THỦ TỤC LẤY NGƯỜI DÙNG ĐÃ NGỪNG HOẠT ĐỘNG
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDeletedUsers]
    @kw NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UserID, ISNULL(UserCode, ''), FullName, N'Ngừng hoạt động', ISNULL(Email, '')
    FROM Users
    WHERE ISNULL(IsActive, 1) = 0
      AND (@kw = '' OR FullName LIKE '%' + @kw + '%' OR ISNULL(UserCode, '') LIKE '%' + @kw + '%' OR ISNULL(Email, '') LIKE '%' + @kw + '%')
    ORDER BY FullName;
END
GO

