-- 17. THỦ TỤC LẤY DANH SÁCH THIẾT BỊ PHÂN TRANG (PAGED)
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDevicesPaged]
    @pageNumber INT,
    @pageSize INT,
    @keyword NVARCHAR(200) = NULL,
    @categoryName NVARCHAR(100) = NULL,
    @status NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @offset INT = (@pageNumber - 1) * @pageSize;
    SELECT 
        d.DeviceID,
        d.DeviceCode,
        d.DeviceName        AS [Tên thiết bị],
        d.DeviceCode        AS [Mã thiết bị],
        dc.CategoryName     AS [Loại thiết bị],
        ISNULL(r.RoomName, '') AS [Vị trí],
        d.Status            AS [Tình trạng],
        d.TotalQuantity     AS [Số lượng],
        d.AvailableQuantity AS AvailableQuantity,
        ISNULL(d.Note, '')  AS [Ghi chú]
    FROM dbo.Devices d
    JOIN dbo.DeviceCategories dc ON dc.CategoryID = d.CategoryID
    LEFT JOIN dbo.Rooms r ON r.RoomID = d.RoomID
    WHERE 
        (@keyword IS NULL OR @keyword = '' OR d.DeviceName LIKE '%' + @keyword + '%' OR d.Note LIKE '%' + @keyword + '%')
        AND (@categoryName IS NULL OR @categoryName = '' OR dc.CategoryName = @categoryName)
        AND (@status IS NULL OR @status = '' OR d.Status = @status)
    ORDER BY d.DeviceID
    OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
END
GO

