-- 33. THỦ TỤC LẤY DANH SÁCH PHÒNG ĐANG HOẠT ĐỘNG
CREATE OR ALTER PROCEDURE [dbo].[sp_GetRooms]
    @retiredStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT RoomID, RoomName
    FROM Rooms
    WHERE ISNULL(Status, N'Hoạt động') <> @retiredStatus
    ORDER BY RoomName;
END
GO

