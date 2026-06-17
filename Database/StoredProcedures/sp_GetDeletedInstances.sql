-- 63. THỦ TỤC LẤY CÁ THỂ THIẾT BỊ ĐÃ XÓA MỀM
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDeletedInstances]
    @status NVARCHAR(50),
    @kw NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT di.InstanceID, ISNULL(di.AssetCode, ''), d.DeviceName, di.Status, ISNULL(di.Note, '')
    FROM DeviceInstances di
    JOIN Devices d ON d.DeviceID = di.DeviceID
    WHERE di.Status = @status
      AND (@kw = '' OR di.AssetCode LIKE '%' + @kw + '%' OR d.DeviceName LIKE '%' + @kw + '%' OR ISNULL(di.Note, '') LIKE '%' + @kw + '%')
    ORDER BY d.DeviceName, di.AssetCode;
END
GO

