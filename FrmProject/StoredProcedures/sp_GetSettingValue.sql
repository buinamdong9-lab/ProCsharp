-- 78. THỦ TỤC LẤY GIÁ TRỊ CẤU HÌNH
CREATE OR ALTER PROCEDURE [dbo].[sp_GetSettingValue]
    @key NVARCHAR(100),
    @defaultValue NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @val NVARCHAR(500);
    SELECT @val = SettingValue FROM dbo.AppSettings WHERE SettingKey = @key;
    SELECT ISNULL(@val, @defaultValue) AS SettingValue;
END
GO

