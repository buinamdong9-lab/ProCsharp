-- 85. THỦ TỤC PHÊ DUYỆT TRẢ THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_ApproveReturnRequest]
    @ticketId INT,
    @approvedByUserId INT,
    @returnPendingStatus NVARCHAR(50),
    @returnedStatus NVARCHAR(50),
    @borrowingStatus NVARCHAR(50),
    @retiredStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50),
    @maintenanceStatus NVARCHAR(50),
    @goodCondition NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Check ticket status
        DECLARE @currentStatus NVARCHAR(50);
        SELECT @currentStatus = Status FROM BorrowTickets WHERE TicketID = @ticketId;
        
        IF @currentStatus IS NULL
        BEGIN
            THROW 50030, N'Không tìm thấy phiếu mượn.', 1;
        END
        
        IF UPPER(@currentStatus) <> UPPER(@returnPendingStatus)
        BEGIN
            THROW 50031, N'Phiếu này không còn ở trạng thái chờ duyệt trả.', 1;
        END

        -- Find Request Master
        DECLARE @requestId INT, @requestedAt DATETIME;
        SELECT @requestId = ReturnRequestID, @requestedAt = RequestedAt 
        FROM ReturnRequests WHERE TicketID = @ticketId;

        IF @requestId IS NULL
        BEGIN
            THROW 50032, N'Không đọc được dữ liệu yêu cầu trả của phiếu này.', 1;
        END

        -- Cursor to process request details
        DECLARE @deviceId INT, @instanceId INT, @borrowQty INT, @returnQty INT, @itemNote NVARCHAR(500);
        
        DECLARE item_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT DeviceID, ISNULL(InstanceID, 0), BorrowQuantity, ReturnQuantity, ISNULL(Note, '')
        FROM ReturnRequestDetails
        WHERE ReturnRequestID = @requestId AND ReturnQuantity > 0;

        OPEN item_cursor;
        FETCH NEXT FROM item_cursor INTO @deviceId, @instanceId, @borrowQty, @returnQty, @itemNote;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Validate return quantity against BorrowDetails
            DECLARE @bdQuantity INT, @bdReturnedQuantity INT;
            SELECT @bdQuantity = Quantity, @bdReturnedQuantity = ReturnedQuantity
            FROM BorrowDetails WITH (UPDLOCK, HOLDLOCK)
            WHERE TicketID = @ticketId AND DeviceID = @deviceId AND ISNULL(InstanceID, 0) = @instanceId;

            IF @bdQuantity IS NULL
            BEGIN
                THROW 50033, N'Chi tiết trả không còn tồn tại trong phiếu mượn.', 1;
            END

            DECLARE @currentBorrowQty INT = @bdQuantity - @bdReturnedQuantity;
            IF @returnQty <= 0 OR @returnQty > @currentBorrowQty
            BEGIN
                THROW 50034, N'Số lượng trả không hợp lệ so với số lượng còn đang mượn.', 1;
            END

            -- Determine if returned as available
            DECLARE @isAvailable BIT = 0;
            DECLARE @trimmedNote NVARCHAR(500) = RTRIM(LTRIM(@itemNote));
            IF @trimmedNote = '' OR UPPER(@trimmedNote) = UPPER(@goodCondition) OR UPPER(@trimmedNote) LIKE UPPER(@goodCondition) + ' -%'
            BEGIN
                SET @isAvailable = 1;
            END

            -- Process Device Instance
            IF @instanceId > 0
            BEGIN
                DECLARE @deviceRetired BIT = 0;
                IF EXISTS (SELECT 1 FROM Devices WHERE DeviceID = @deviceId AND Status = @retiredStatus)
                BEGIN
                    SET @deviceRetired = 1;
                END

                -- Update instance status & condition
                DECLARE @instStatus NVARCHAR(50);
                IF @deviceRetired = 1
                    SET @instStatus = @retiredStatus;
                ELSE IF @isAvailable = 1
                    SET @instStatus = @availableStatus;
                ELSE
                    SET @instStatus = @maintenanceStatus;

                UPDATE DeviceInstances
                SET Status = @instStatus,
                    Condition = CASE WHEN @trimmedNote = '' THEN @goodCondition ELSE @trimmedNote END
                WHERE InstanceID = @instanceId;

                -- Recalculate quantities for the parent device
                UPDATE Devices
                SET TotalQuantity = (SELECT COUNT(*) FROM DeviceInstances WHERE DeviceID = @deviceId AND Status <> @retiredStatus),
                    AvailableQuantity = (
                        SELECT COUNT(*)
                        FROM DeviceInstances
                        WHERE DeviceID = @deviceId
                          AND Status = @availableStatus
                          AND ISNULL(NULLIF(Condition, ''), @goodCondition) = @goodCondition
                    )
                WHERE DeviceID = @deviceId;
            END
            ELSE
            BEGIN
                -- Bulk device
                IF @isAvailable = 1
                BEGIN
                    UPDATE Devices
                    SET AvailableQuantity = AvailableQuantity + @returnQty
                    WHERE DeviceID = @deviceId AND Status <> @retiredStatus;
                END
                ELSE
                BEGIN
                    UPDATE Devices
                    SET TotalQuantity = TotalQuantity - @returnQty
                    WHERE DeviceID = @deviceId AND Status <> @retiredStatus;
                END
            END

            -- Update BorrowDetails
            UPDATE BorrowDetails
            SET ReturnedQuantity = ReturnedQuantity + @returnQty,
                Note = CASE 
                    WHEN @trimmedNote = '' THEN Note
                    WHEN ISNULL(Note, '') = '' THEN @trimmedNote
                    ELSE Note + ' | ' + @trimmedNote
                END
            WHERE TicketID = @ticketId AND DeviceID = @deviceId AND ISNULL(InstanceID, 0) = @instanceId;

            FETCH NEXT FROM item_cursor INTO @deviceId, @instanceId, @borrowQty, @returnQty, @itemNote;
        END

        CLOSE item_cursor;
        DEALLOCATE item_cursor;

        -- Check remaining quantities on the ticket
        DECLARE @remainingQuantity INT;
        SELECT @remainingQuantity = ISNULL(SUM(Quantity - ReturnedQuantity), 0)
        FROM BorrowDetails
        WHERE TicketID = @ticketId;

        DECLARE @fullyReturned BIT = 0;
        IF @remainingQuantity = 0
            SET @fullyReturned = 1;

        DECLARE @nextStatus NVARCHAR(50);
        IF @fullyReturned = 1
            SET @nextStatus = @returnedStatus;
        ELSE
            SET @nextStatus = @borrowingStatus;

        DECLARE @approvalNote NVARCHAR(1000);
        IF @fullyReturned = 1
            SET @approvalNote = N'Admin đã duyệt yêu cầu trả thiết bị.';
        ELSE
            SET @approvalNote = N'Admin đã duyệt trả một phần. Còn ' + CAST(@remainingQuantity AS NVARCHAR(10)) + N' thiết bị chưa trả.';

        -- Update Borrow Ticket
        UPDATE BorrowTickets
        SET Status = @nextStatus,
            ReturnDate = CASE WHEN @fullyReturned = 1 THEN ISNULL(@requestedAt, GETDATE()) ELSE ReturnDate END,
            ReturnedBy = @approvedByUserId,
            ReturnNote = NULL,
            Note = CASE WHEN ISNULL(Note, '') = '' THEN @approvalNote ELSE Note + ' | ' + @approvalNote END
        WHERE TicketID = @ticketId;

        -- Clear return request
        DELETE FROM ReturnRequestDetails WHERE ReturnRequestID = @requestId;
        DELETE FROM ReturnRequests WHERE ReturnRequestID = @requestId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF CURSOR_STATUS('local', 'item_cursor') >= 0
        BEGIN
            CLOSE item_cursor;
            DEALLOCATE item_cursor;
        END
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

