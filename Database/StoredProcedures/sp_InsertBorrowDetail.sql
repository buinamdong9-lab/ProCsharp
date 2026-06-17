-- 37. THỦ TỤC THÊM CHI TIẾT PHIẾU MƯỢN CÓ RÀNG BUỘC
CREATE OR ALTER PROCEDURE [dbo].[sp_InsertBorrowDetail]
    @ticketID INT,
    @deviceID INT,
    @instanceID INT,
    @qty INT,
    @availableStatus NVARCHAR(50),
    @goodCondition NVARCHAR(50),
    @pendingStatus NVARCHAR(50),
    @borrowingStatus NVARCHAR(50),
    @returnPendingStatus NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO BorrowDetails (TicketID, DeviceID, InstanceID, Quantity)
    SELECT @ticketID, @deviceID, @instanceID, @qty
    WHERE EXISTS (
        SELECT 1
        FROM DeviceInstances WITH (UPDLOCK, HOLDLOCK)
        WHERE InstanceID = @instanceID
          AND DeviceID = @deviceID
          AND Status = @availableStatus
          AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
    )
      AND NOT EXISTS (
        SELECT 1
        FROM BorrowDetails existing
        JOIN BorrowTickets ticket ON ticket.TicketID = existing.TicketID
        WHERE existing.InstanceID = @instanceID
          AND ticket.Status IN (@pendingStatus, @borrowingStatus, @returnPendingStatus)
          AND existing.Quantity > existing.ReturnedQuantity
      );

    SELECT @@ROWCOUNT;
END
GO

