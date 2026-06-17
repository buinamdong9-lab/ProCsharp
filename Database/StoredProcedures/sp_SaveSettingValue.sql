-- 79. THỦ TỤC LƯU GIÁ TRỊ CẤU HÌNH
CREATE OR ALTER PROCEDURE [dbo].[sp_SaveSettingValue]
    @key NVARCHAR(100),
    @val NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    MERGE dbo.AppSettings AS t
    USING (SELECT @key AS SettingKey) AS s
    ON t.SettingKey = s.SettingKey
    WHEN MATCHED THEN UPDATE SET SettingValue = @val
    WHEN NOT MATCHED THEN INSERT (SettingKey, SettingValue) VALUES (@key, @val);
END
GO

