-- 59. THỦ TỤC THÊM PHÒNG HỌC MỚI
CREATE OR ALTER PROCEDURE [dbo].[sp_InsertRoom]
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
    INSERT INTO Rooms 
        (RoomCode, RoomName, RoomType, Floor, Capacity, Status, Note)
    VALUES 
        (@code, @name, @type, @floor, @cap, @status, @note);
END
GO

