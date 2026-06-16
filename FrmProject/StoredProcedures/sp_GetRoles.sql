-- 81. THỦ TỤC LẤY DANH SÁCH VAI TRÒ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetRoles]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.RoleID AS [Mã vai trò],
           r.RoleName AS [Tên vai trò],
           COUNT(u.UserID) AS [Số người dùng]
    FROM Roles r
    LEFT JOIN Users u ON u.RoleID = r.RoleID
    GROUP BY r.RoleID, r.RoleName
    ORDER BY r.RoleName;
END
GO

