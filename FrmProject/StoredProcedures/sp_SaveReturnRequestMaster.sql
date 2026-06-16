-- 44. THỦ TỤC LƯU YÊU CẦU TRẢ MASTER
CREATE OR ALTER PROCEDURE [dbo].[sp_SaveReturnRequestMaster]
    @ticketId INT,
    @requestedAt DATETIME,
    @note NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Xóa yêu cầu cũ trước
    IF OBJECT_ID('dbo.ReturnRequests', 'U') IS NOT NULL AND OBJECT_ID('dbo.ReturnRequestDetails', 'U') IS NOT NULL
    BEGIN
        DELETE d
        FROM ReturnRequestDetails d
        JOIN ReturnRequests r ON r.ReturnRequestID = d.ReturnRequestID
        WHERE r.TicketID = @ticketId;

        DELETE FROM ReturnRequests WHERE TicketID = @ticketId;
    END

    -- Thêm mới ReturnRequests
    INSERT INTO ReturnRequests (TicketID, RequestedAt, Note)
    VALUES (@ticketId, @requestedAt, @note);
    
    SELECT SCOPE_IDENTITY();
END
GO

