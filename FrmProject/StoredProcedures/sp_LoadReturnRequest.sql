-- 46. THỦ TỤC TẢI THÔNG TIN YÊU CẦU TRẢ
CREATE OR ALTER PROCEDURE [dbo].[sp_LoadReturnRequest]
    @ticketId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF OBJECT_ID('dbo.ReturnRequests', 'U') IS NOT NULL AND OBJECT_ID('dbo.ReturnRequestDetails', 'U') IS NOT NULL
    BEGIN
        SELECT r.RequestedAt,
               d.DeviceID,
               ISNULL(d.InstanceID, 0),
               d.BorrowQuantity,
               d.ReturnQuantity,
               ISNULL(d.Note, '')
        FROM ReturnRequests r
        JOIN ReturnRequestDetails d ON d.ReturnRequestID = r.ReturnRequestID
        WHERE r.TicketID = @ticketId
        ORDER BY d.ReturnRequestDetailID;
    END
END
GO

