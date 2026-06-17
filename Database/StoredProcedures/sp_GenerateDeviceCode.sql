-- 21. THỦ TỤC TỰ SINH MÃ THIẾT BỊ CAO NHẤT
CREATE OR ALTER PROCEDURE [dbo].[sp_GenerateDeviceCode]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MAX(CAST(SUBSTRING(DeviceCode, 3, LEN(DeviceCode) - 2) AS INT)) 
    FROM dbo.Devices 
    WHERE DeviceCode LIKE 'TB[0-9]%';
END
GO

