-- 18. THỦ TỤC LẤY DANH MỤC THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetCategories]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoryID, CategoryName FROM dbo.DeviceCategories ORDER BY CategoryName;
END
GO

