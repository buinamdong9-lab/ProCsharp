-- 82. THỦ TỤC THÊM LOẠI THIẾT BỊ MỚI
CREATE OR ALTER PROCEDURE [dbo].[sp_AddDeviceCategory]
    @name NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO DeviceCategories (CategoryName) VALUES (@name);
END
GO

