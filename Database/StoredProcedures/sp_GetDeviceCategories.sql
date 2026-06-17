-- 80. THỦ TỤC LẤY DANH SÁCH LOẠI THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDeviceCategories]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dc.CategoryID AS [Mã loại], 
           dc.CategoryName AS [Tên loại],
           COUNT(d.DeviceID) AS [Số thiết bị],
           '' AS [Thao tác]
    FROM DeviceCategories dc
    LEFT JOIN Devices d ON d.CategoryID = dc.CategoryID
    GROUP BY dc.CategoryID, dc.CategoryName
    ORDER BY dc.CategoryName;
END
GO

