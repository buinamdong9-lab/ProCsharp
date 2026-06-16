-- 11. THỦ TỤC TÌM KIẾM NGƯỜI DÙNG
CREATE OR ALTER PROCEDURE [dbo].[sp_SearchUsers]
    @keyword NVARCHAR(200) = NULL,
    @role NVARCHAR(100) = NULL,
    @status NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ISNULL(u.UserCode, '') AS [Mã ND],
        u.FullName AS [Họ tên],
        ISNULL(u.UserCode, '') AS [Mã số],
        ISNULL(u.Email, '') AS [Email],
        ISNULL(u.Department, '') AS [Khoa/Bộ Môn],
        ISNULL(r.RoleName, '') AS [Vai trò],
        ISNULL(u.Phone, '') AS [Số điện thoại],
        (SELECT COUNT(*) FROM BorrowTickets bt 
         WHERE bt.UserID = u.UserID 
           AND bt.Status IN ('BORROWING', 'RETURN_PEND')) AS [Đang mượn],
        CASE
            WHEN ISNULL(u.IsActive, 1) = 0 THEN N'Ngừng hoạt động'
            WHEN ISNULL(u.IsLocked, 0) = 1 THEN N'Bị khóa'
            ELSE N'Hoạt động'
        END AS [Trạng thái]
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
         (@status = N'Bị khóa' AND ISNULL(u.IsLocked, 0) = 1))
    ORDER BY u.FullName;
END
GO

