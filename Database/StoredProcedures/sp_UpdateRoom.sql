-- 60. THỦ TỤC CẬP NHẬT PHÒNG HỌC
CREATE OR ALTER PROCEDURE [dbo].[sp_UpdateRoom]
    @roomId INT,
    @code NVARCHAR(100),
    @name NVARCHAR(200),
    @type NVARCHAR(100),
    @floor NVARCHAR(50),
    @cap INT,
    @status NVARCHAR(100),
    @note NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Rooms SET
        RoomCode = @code, RoomName = @name, RoomType = @type,
        Floor = @floor, Capacity = @cap, Status = @status, Note = @note
    WHERE RoomID = @roomId;
END
GO

