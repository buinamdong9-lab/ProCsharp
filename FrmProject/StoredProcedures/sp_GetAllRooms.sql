-- =========================================================================
-- LỚP PHÒNG HỌC (ROOM REPOSITORY)
-- =========================================================================

-- 56. THỦ TỤC LẤY TẤT CẢ PHÒNG HỌC
CREATE OR ALTER PROCEDURE [dbo].[sp_GetAllRooms]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        RoomCode AS [Mã phòng],
        RoomName AS [Tên phòng],
        RoomType AS [Loại],
        Floor AS [Tầng],
        Capacity AS [Sức chứa],
        Status AS [Trạng thái]
    FROM Rooms;
END
GO

