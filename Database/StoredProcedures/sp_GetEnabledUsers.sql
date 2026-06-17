-- =========================================================================
-- LỚP PHIẾU MƯỢN (BORROW TICKET REPOSITORY)
-- =========================================================================

-- 32. THỦ TỤC LẤY NGƯỜI DÙNG ĐANG HOẠT ĐỘNG
CREATE OR ALTER PROCEDURE [dbo].[sp_GetEnabledUsers]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UserID, u.FullName 
    FROM Users u 
    WHERE ISNULL(u.IsActive, 1) = 1 AND ISNULL(u.IsLocked, 0) = 0 
    ORDER BY u.FullName;
END
GO

