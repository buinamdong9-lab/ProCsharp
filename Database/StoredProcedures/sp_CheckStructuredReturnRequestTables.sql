-- 88. THỦ TỤC KIỂM TRA CẤU TRÚC BẢNG YÊU CẦU TRẢ
CREATE OR ALTER PROCEDURE [dbo].[sp_CheckStructuredReturnRequestTables]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CASE
        WHEN OBJECT_ID('dbo.ReturnRequests', 'U') IS NOT NULL
         AND OBJECT_ID('dbo.ReturnRequestDetails', 'U') IS NOT NULL
        THEN 1 ELSE 0 END AS HasTables;
END
GO

