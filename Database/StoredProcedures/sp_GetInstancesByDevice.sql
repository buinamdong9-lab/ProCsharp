-- 24. THỦ TỤC LẤY CÁ THỂ THIẾT BỊ THEO MÃ LOẠI
CREATE OR ALTER PROCEDURE [dbo].[sp_GetInstancesByDevice]
    @deviceId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT InstanceID, AssetCode AS [Mã tài sản], Status AS [Trạng thái], Condition AS [Tình trạng] 
    FROM dbo.DeviceInstances 
    WHERE DeviceID = @deviceId 
    ORDER BY InstanceID;
END
GO

