-- 83. THỦ TỤC DUYỆT PHIẾU MƯỢN
CREATE OR ALTER PROCEDURE [dbo].[sp_ApproveBorrowTicket]
    @ticketId INT,
    @pendingStatus NVARCHAR(50),
    @borrowingStatus NVARCHAR(50),
    @maintenanceStatus NVARCHAR(50),
    @retiredStatus NVARCHAR(50),
    @availableStatus NVARCHAR(50),
    @borrowedStatus NVARCHAR(50),
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
            THROW 50020, N'Không tìm thấy phiếu mượn.', 1;
        END
        
        IF UPPER(@currentStatus) <> UPPER(@pendingStatus)
        BEGIN
            THROW 50021, N'Phiếu này không còn ở trạng thái chờ duyệt.', 1;
        END

        -- 2. Validate borrow items count
        IF NOT EXISTS (SELECT 1 FROM BorrowDetails WHERE TicketID = @ticketId)
        BEGIN
            THROW 50022, N'Phiếu này chưa có chi tiết thiết bị để phê duyệt.', 1;
        END

        -- Temp variables to validate and process each item
        DECLARE @deviceId INT, @instanceId INT, @deviceName NVARCHAR(255), @availableQty INT, @requestedQty INT;
        DECLARE @deviceStatus NVARCHAR(50), @instanceStatus NVARCHAR(50), @instanceCondition NVARCHAR(50);

        DECLARE item_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT d.DeviceID,
               ISNULL(bd.InstanceID, 0),
               d.DeviceName,
               d.AvailableQuantity,
               bd.Quantity,
               ISNULL(d.Status, ''),
               ISNULL(di.Status, ''),
               ISNULL(di.Condition, '')
        FROM BorrowDetails bd
        JOIN Devices d WITH (UPDLOCK, HOLDLOCK) ON d.DeviceID = bd.DeviceID
        LEFT JOIN DeviceInstances di WITH (UPDLOCK, HOLDLOCK) ON di.InstanceID = bd.InstanceID
        WHERE bd.TicketID = @ticketId;

        OPEN item_cursor;
        FETCH NEXT FROM item_cursor INTO @deviceId, @instanceId, @deviceName, @availableQty, @requestedQty, @deviceStatus, @instanceStatus, @instanceCondition;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Validate Device status
            IF UPPER(@deviceStatus) = UPPER(@maintenanceStatus)
            BEGIN
                DECLARE @errMaintenance NVARCHAR(1000) = N'Thiết bị ''' + @deviceName + N''' đang ở trạng thái bảo trì, không thể xuất kho.';
                THROW 50023, @errMaintenance, 1;
            END

            IF UPPER(@deviceStatus) = UPPER(@retiredStatus)
            BEGIN
                DECLARE @errRetired NVARCHAR(1000) = N'Thiết bị ''' + @deviceName + N''' đã ngừng sử dụng, không thể xuất kho.';
                THROW 50024, @errRetired, 1;
            END

            -- Validate Quantity
            IF @availableQty < @requestedQty
            BEGIN
                DECLARE @errQty NVARCHAR(1000) = N'Thiết bị ''' + @deviceName + N''' chỉ còn ' + CAST(@availableQty AS NVARCHAR(10)) + N', không đủ ' + CAST(@requestedQty AS NVARCHAR(10)) + N' để phê duyệt.';
                THROW 50025, @errQty, 1;
            END

            -- Validate Instance if present
            IF @instanceId > 0
            BEGIN
                IF UPPER(@instanceStatus) <> UPPER(@availableStatus)
                BEGIN
                    DECLARE @errInstStatus NVARCHAR(1000) = N'Cá thể của thiết bị ''' + @deviceName + N''' không còn ở trạng thái có sẵn, không thể phê duyệt mượn.';
                    THROW 50026, @errInstStatus, 1;
                END

                -- Normalize condition (empty/null -> Good)
                DECLARE @normCondition NVARCHAR(50) = ISNULL(NULLIF(@instanceCondition, ''), @goodCondition);
                IF UPPER(@normCondition) <> UPPER(@goodCondition)
                BEGIN
                    DECLARE @errCondition NVARCHAR(1000) = N'Cá thể của thiết bị ''' + @deviceName + N''' có tình trạng ''' + @instanceCondition + N''', chỉ thiết bị tình trạng Tốt mới được mượn.';
                    THROW 50027, @errCondition, 1;
                END
                
                -- Update instance
                UPDATE DeviceInstances
                SET Status = @borrowedStatus
                WHERE InstanceID = @instanceId;
            END

            -- Update Device available qty
            UPDATE Devices
            SET AvailableQuantity = AvailableQuantity - @requestedQty
            WHERE DeviceID = @deviceId;

            FETCH NEXT FROM item_cursor INTO @deviceId, @instanceId, @deviceName, @availableQty, @requestedQty, @deviceStatus, @instanceStatus, @instanceCondition;
        END

        CLOSE item_cursor;
        DEALLOCATE item_cursor;

        -- 3. Update Borrow Ticket Status
        UPDATE BorrowTickets SET Status = @borrowingStatus WHERE TicketID = @ticketId;

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

