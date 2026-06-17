-- 86. THỦ TỤC TỪ CHỐI TRẢ THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_RejectReturnRequest]
    @ticketId INT,
    @borrowingStatus NVARCHAR(50),
    @returnPendingStatus NVARCHAR(50),
    @reason NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @note NVARCHAR(1000) = N'Yêu cầu trả bị từ chối: ' + RTRIM(LTRIM(@reason));
        
        UPDATE BorrowTickets
        SET Status = @borrowingStatus,
            ReturnNote = NULL,
            Note = CASE WHEN ISNULL(Note, '') = '' THEN @note ELSE Note + ' | ' + @note END
        WHERE TicketID = @ticketId AND Status = @returnPendingStatus;

        IF @@ROWCOUNT = 0
        BEGIN
            THROW 50035, N'Phiếu này không còn ở trạng thái chờ duyệt trả.', 1;
        END

        -- Clear request
        DELETE rrd FROM ReturnRequestDetails rrd
        JOIN ReturnRequests rr ON rr.ReturnRequestID = rrd.ReturnRequestID
        WHERE rr.TicketID = @ticketId;

        DELETE FROM ReturnRequests WHERE TicketID = @ticketId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

