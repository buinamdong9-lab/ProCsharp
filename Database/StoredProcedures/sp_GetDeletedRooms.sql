-- 64. THỦ TỤC LẤY PHÒNG HỌC ĐÃ XÓA MỀM
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDeletedRooms]
    @status NVARCHAR(50),
    @kw NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT RoomID, ISNULL(RoomCode, ''), RoomName, Status, ISNULL(Note, '')
    FROM Rooms
    WHERE Status = @status
      AND (@kw = '' OR RoomCode LIKE '%' + @kw + '%' OR RoomName LIKE '%' + @kw + '%' OR ISNULL(Note, '') LIKE '%' + @kw + '%')
    ORDER BY RoomName;
END
GO

