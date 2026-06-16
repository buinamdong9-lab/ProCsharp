-- 26. THỦ TỤC LẤY TOÀN BỘ ASSET CODES CỦA MỘT THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetInstanceAssetCodesByDevice]
    @deviceId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AssetCode FROM dbo.DeviceInstances WHERE DeviceID = @deviceId;
END
GO

