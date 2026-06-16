-- =========================================================================
-- LỚP THÙNG RÁC (RECYCLE BIN REPOSITORY)
-- =========================================================================

-- 62. THỦ TỤC LẤY THIẾT BỊ ĐÃ XÓA MỀM
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDeletedDevices]
    @status NVARCHAR(50),
    @kw NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DeviceID, ISNULL(DeviceCode, ''), DeviceName, Status, ISNULL(Note, '')
    FROM Devices
    WHERE Status = @status
      AND (@kw = '' OR DeviceName LIKE '%' + @kw + '%' OR ISNULL(DeviceCode, '') LIKE '%' + @kw + '%' OR ISNULL(Note, '') LIKE '%' + @kw + '%')
    ORDER BY DeviceName;
END
GO

