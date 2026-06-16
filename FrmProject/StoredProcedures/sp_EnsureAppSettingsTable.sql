-- 77. THỦ TỤC ĐẢM BẢO BẢNG APPSETTINGS TỒN TẠI
CREATE OR ALTER PROCEDURE [dbo].[sp_EnsureAppSettingsTable]
AS
BEGIN
    SET NOCOUNT ON;
    IF OBJECT_ID('dbo.AppSettings', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.AppSettings (
            SettingKey   NVARCHAR(100) NOT NULL PRIMARY KEY,
            SettingValue NVARCHAR(500) NULL
        );
    END
END
GO

