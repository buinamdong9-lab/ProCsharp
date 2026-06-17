-- 68. THỦ TỤC KHÔI PHỤC PHÒNG HỌC
CREATE OR ALTER PROCEDURE [dbo].[sp_RestoreRoom]
    @id INT,
    @activeStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Rooms SET Status = @activeStatus WHERE RoomID = @id;
END
GO

