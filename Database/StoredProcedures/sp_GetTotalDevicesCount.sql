-- 16. THỦ TỤC ĐẾM TỔNG SỐ THIẾT BỊ PHÙ HỢP BỘ LỌC
CREATE OR ALTER PROCEDURE [dbo].[sp_GetTotalDevicesCount]
    @keyword NVARCHAR(200) = NULL,
    @categoryName NVARCHAR(100) = NULL,
    @status NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*)
    FROM dbo.Devices d
    JOIN dbo.DeviceCategories dc ON dc.CategoryID = d.CategoryID
    LEFT JOIN dbo.Rooms r ON r.RoomID = d.RoomID
    WHERE 
        (@keyword IS NULL OR @keyword = '' OR d.DeviceName LIKE '%' + @keyword + '%' OR d.Note LIKE '%' + @keyword + '%')
        AND (@categoryName IS NULL OR @categoryName = '' OR dc.CategoryName = @categoryName)
        AND (@status IS NULL OR @status = '' OR d.Status = @status);
END
GO

