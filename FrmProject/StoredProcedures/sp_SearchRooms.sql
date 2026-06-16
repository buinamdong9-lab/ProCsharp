-- 57. THỦ TỤC TÌM KIẾM PHÒNG HỌC
CREATE OR ALTER PROCEDURE [dbo].[sp_SearchRooms]
    @keyword NVARCHAR(100) = NULL,
    @type NVARCHAR(100) = NULL,
    @status NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT RoomCode AS [Mã phòng], RoomName AS [Tên phòng],
           RoomType AS [Loại], Floor AS [Tầng],
           Capacity AS [Sức chứa], Status AS [Trạng thái]
    FROM Rooms
    WHERE 
        (@keyword IS NULL OR @keyword = '' OR RoomCode LIKE '%' + @keyword + '%' OR RoomName LIKE '%' + @keyword + '%')
        AND (@type IS NULL OR @type = '' OR @type = N'Tất cả' OR RoomType = @type)
        AND (@status IS NULL OR @status = '' OR @status = N'Tất cả' OR Status = @status);
END
GO

