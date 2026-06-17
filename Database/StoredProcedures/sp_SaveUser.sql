-- 9. THỦ TỤC LƯU/THÊM/SỬA NGƯỜI DÙNG
CREATE OR ALTER PROCEDURE [dbo].[sp_SaveUser]
    @userId INT,
    @code NVARCHAR(100),
    @name NVARCHAR(200),
    @email NVARCHAR(200),
    @dept NVARCHAR(200),
    @phone NVARCHAR(50),
    @roleID INT,
    @username NVARCHAR(100),
    @password NVARCHAR(200) = NULL,
    @active BIT,
    @locked BIT,
    @isAdding BIT
AS
BEGIN
    SET NOCOUNT ON;
    IF @isAdding = 1 OR @userId < 0
    BEGIN
        INSERT INTO Users 
            (UserCode, FullName, Email, Department, Phone, RoleID, Username, PasswordHash, IsActive, IsLocked, CreatedAt)
        VALUES 
            (@code, @name, @email, @dept, @phone, @roleID, @username, ISNULL(@password, ''), @active, @locked, GETDATE());
    END
    ELSE
    BEGIN
        UPDATE Users
        SET UserCode = @code,
            FullName = @name,
            Email = @email,
            Department = @dept,
            Phone = @phone,
            RoleID = @roleID,
            Username = @username,
            IsActive = @active,
            IsLocked = @locked
        WHERE UserID = @userId;

        -- Chỉ cập nhật password nếu có truyền vào giá trị mới
        IF @password IS NOT NULL AND @password <> ''
        BEGIN
            UPDATE Users
            SET PasswordHash = @password
            WHERE UserID = @userId;
        END
    END
END
GO

