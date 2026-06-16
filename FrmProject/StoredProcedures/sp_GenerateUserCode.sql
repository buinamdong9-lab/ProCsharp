-- 8. THỦ TỤC SINH MÃ NGƯỜI DÙNG CAO NHẤT
CREATE OR ALTER PROCEDURE [dbo].[sp_GenerateUserCode]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MAX(CAST(SUBSTRING(UserCode, 3, LEN(UserCode) - 2) AS INT)) 
    FROM Users 
    WHERE UserCode LIKE 'ND[0-9]%';
END
GO

