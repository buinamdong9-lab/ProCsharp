-- 13. THỦ TỤC ĐẾM TỔNG SỐ NGƯỜI DÙNG PHÙ HỢP BỘ LỌC
CREATE OR ALTER PROCEDURE [dbo].[sp_GetTotalUsersCount]
    @keyword NVARCHAR(200) = NULL,
    @role NVARCHAR(100) = NULL,
    @status NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(DISTINCT u.UserID)
    FROM Users u
    LEFT JOIN Roles r ON u.RoleID = r.RoleID
    WHERE 
        (@keyword IS NULL OR @keyword = '' OR 
         u.FullName LIKE '%' + @keyword + '%' OR 
         ISNULL(u.UserCode, '') LIKE '%' + @keyword + '%' OR 
         ISNULL(u.Email, '') LIKE '%' + @keyword + '%' OR 
         ISNULL(u.Username, '') LIKE '%' + @keyword + '%')
        AND
        (@role IS NULL OR @role = '' OR ISNULL(r.RoleName, '') LIKE '%' + @role + '%')
        AND
        (@status IS NULL OR @status = '' OR @status = N'Tất cả' OR
         (@status = N'Hoạt động' AND ISNULL(u.IsActive, 1) = 1 AND ISNULL(u.IsLocked, 0) = 0) OR
         (@status = N'Ngừng hoạt động' AND ISNULL(u.IsActive, 1) = 0) OR
         (@status = N'Bị khóa' AND ISNULL(u.IsLocked, 0) = 1));
END
GO

