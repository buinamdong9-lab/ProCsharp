-- 27. THỦ TỤC KIỂM TRA TRÙNG ASSET CODE
CREATE OR ALTER PROCEDURE [dbo].[sp_AssetCodeExists]
    @assetCode NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM dbo.DeviceInstances WHERE AssetCode = @assetCode;
END
GO

