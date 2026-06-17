-- 45. THỦ TỤC LƯU CHI TIẾT YÊU CẦU TRẢ
CREATE OR ALTER PROCEDURE [dbo].[sp_SaveReturnRequestDetail]
    @requestId INT,
    @deviceId INT,
    @instanceId INT,
    @borrowQty INT,
    @returnQty INT,
    @note NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ReturnRequestDetails
        (ReturnRequestID, DeviceID, InstanceID, BorrowQuantity, ReturnQuantity, Note)
    VALUES
        (@requestId, @deviceId, CASE WHEN @instanceId <= 0 THEN NULL ELSE @instanceId END, @borrowQty, @returnQty, @note);
END
GO

