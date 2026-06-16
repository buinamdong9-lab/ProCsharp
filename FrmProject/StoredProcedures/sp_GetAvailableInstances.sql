-- 35. THỦ TỤC LẤY CÁC CÁ THỂ THIẾT BỊ KHẢ DỤNG CỦA LOẠI THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_GetAvailableInstances]
    @devId INT,
    @availableStatus NVARCHAR(50),
    @goodCondition NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT InstanceID, AssetCode
    FROM DeviceInstances
    WHERE DeviceID = @devId
      AND Status = @availableStatus
      AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
    ORDER BY AssetCode;
END
GO

