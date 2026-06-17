-- 25. THỦ TỤC LẤY DEVICE CODE
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDeviceCodeOnly]
    @deviceId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DeviceCode FROM dbo.Devices WHERE DeviceID = @deviceId;
END
GO

