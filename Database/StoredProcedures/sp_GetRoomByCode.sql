-- 58. THỦ TỤC LẤY THÔNG TIN PHÒNG HỌC THEO MÃ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetRoomByCode]
    @code NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT RoomID, Floor, Capacity, Note FROM Rooms WHERE RoomCode = @code;
END
GO

