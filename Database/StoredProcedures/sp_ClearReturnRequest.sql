-- 43. THỦ TỤC XÓA YÊU CẦU TRẢ PHIẾU
CREATE OR ALTER PROCEDURE [dbo].[sp_ClearReturnRequest]
    @ticketId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF OBJECT_ID('dbo.ReturnRequests', 'U') IS NOT NULL AND OBJECT_ID('dbo.ReturnRequestDetails', 'U') IS NOT NULL
    BEGIN
        DELETE d
        FROM ReturnRequestDetails d
        JOIN ReturnRequests r ON r.ReturnRequestID = d.ReturnRequestID
        WHERE r.TicketID = @ticketId;

        DELETE FROM ReturnRequests WHERE TicketID = @ticketId;
    END
END
GO

