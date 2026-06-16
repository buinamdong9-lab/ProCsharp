-- 19. THỦ TỤC LƯU/THÊM/SỬA THIẾT BỊ
CREATE OR ALTER PROCEDURE [dbo].[sp_SaveDevice]
    @deviceId INT,
    @deviceCode NVARCHAR(100),
    @deviceName NVARCHAR(200),
    @categoryName NVARCHAR(100),
    @roomNameOrCode NVARCHAR(100),
    @totalQuantity INT,
    @borrowedQuantity INT,
    @status NVARCHAR(100),
    @note NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Lấy hoặc tạo CategoryID
        DECLARE @catID INT;
        SELECT @catID = CategoryID FROM dbo.DeviceCategories WHERE CategoryName = @categoryName;
        IF @catID IS NULL
        BEGIN
            INSERT INTO dbo.DeviceCategories (CategoryName) VALUES (@categoryName);
            SET @catID = SCOPE_IDENTITY();
        END

        -- 2. Tìm RoomID
        DECLARE @roomID INT = NULL;
        IF @roomNameOrCode IS NOT NULL AND @roomNameOrCode <> ''
        BEGIN
            SELECT TOP 1 @roomID = RoomID FROM dbo.Rooms WHERE RoomName = @roomNameOrCode OR RoomCode = @roomNameOrCode;
            IF @roomID IS NULL
            BEGIN
                THROW 50001, N'Vị trí/phòng không tồn tại. Hãy nhập đúng mã hoặc tên phòng.', 1;
            END
        END

        -- 3. Kiểm tra trùng mã thiết bị
        IF EXISTS (SELECT 1 FROM dbo.Devices WHERE DeviceCode = @deviceCode AND DeviceID <> CASE WHEN @deviceId < 0 THEN 0 ELSE @deviceId END)
        BEGIN
            THROW 50002, N'Mã thiết bị đã tồn tại. Vui lòng nhập mã khác.', 1;
        END

        -- 4. Insert hoặc Update
        IF @deviceId < 0
        BEGIN
            INSERT INTO dbo.Devices 
                (DeviceCode, DeviceName, CategoryID, RoomID, TotalQuantity, AvailableQuantity, Status, Note)
            VALUES 
                (@deviceCode, @deviceName, @catID, @roomID, @totalQuantity, @totalQuantity, @status, @note);
        END
        ELSE
        BEGIN
            IF @totalQuantity < @borrowedQuantity
            BEGIN
                DECLARE @errMsg NVARCHAR(500) = CONCAT(N'Không thể giảm tổng số lượng xuống ', @totalQuantity, N' vì hiện có ', @borrowedQuantity, N' thiết bị đang được mượn.');
                THROW 50003, @errMsg, 1;
            END

            UPDATE dbo.Devices SET
                DeviceCode = @deviceCode, 
                DeviceName = @deviceName, 
                CategoryID = @catID, 
                RoomID = @roomID,
                TotalQuantity = @totalQuantity, 
                AvailableQuantity = @totalQuantity - @borrowedQuantity,
                Status = @status, 
                Note = @note
            WHERE DeviceID = @deviceId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

