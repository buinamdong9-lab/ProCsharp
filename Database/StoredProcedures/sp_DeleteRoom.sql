-- 61. THỦ TỤC XÓA MỀM PHÒNG HỌC
CREATE OR ALTER PROCEDURE [dbo].[sp_DeleteRoom]
    @id INT,
    @note NVARCHAR(500),
    @retiredStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Rooms
    SET Status = @retiredStatus,
        Note = LTRIM(RTRIM(CONCAT(ISNULL(NULLIF(Note, ''), ''), CASE WHEN ISNULL(NULLIF(Note, ''), '') = '' THEN '' ELSE ' | ' END, @note)))
    WHERE RoomID = @id;
END
GO

