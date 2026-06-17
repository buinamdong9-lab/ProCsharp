-- =========================================================================
-- LỚP USER (USER REPOSITORY)
-- =========================================================================

-- 5. THỦ TỤC LẤY TẤT CẢ NGƯỜI DÙNG
CREATE OR ALTER PROCEDURE [dbo].[sp_GetAllUsers]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        ISNULL(u.UserCode, '')      AS [Mã ND],
        u.FullName                  AS [Họ tên],
        ISNULL(u.UserCode, '')      AS [Mã số],
        ISNULL(u.Email, '')         AS [Email],
        ISNULL(u.Department, '')    AS [Khoa/Bộ Môn],
        ISNULL(r.RoleName, '')      AS [Vai trò],
        ISNULL(u.Phone, '')         AS [Số điện thoại],
        (SELECT COUNT(*) FROM BorrowTickets bt 
         WHERE bt.UserID = u.UserID 
           AND bt.Status IN ('BORROWING', 'RETURN_PEND')) AS [Đang mượn],
        CASE
            WHEN ISNULL(u.IsActive, 1) = 0 THEN N'Ngừng hoạt động'
            WHEN ISNULL(u.IsLocked, 0) = 1 THEN N'Bị khóa'
            ELSE N'Hoạt động'
        END                         AS [Trạng thái]
    FROM Users u
    LEFT JOIN Roles r ON u.RoleID = r.RoleID
    ORDER BY u.FullName;
END
GO

