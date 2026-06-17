-- =========================================================================
-- FILE: Database_StoredProcedures.sql
-- Mô tả: Chứa toàn bộ câu lệnh tạo Stored Procedure cho dự án FrmProject.
-- Cách dùng: Chạy toàn bộ file này trên SQL Server Management Studio (SSMS).
-- =========================================================================

-- 1. THỦ TỤC ĐẢM BẢO CÁC CỘT BẢO MẬT ĐĂNG NHẬP (LỚP AUTH)
CREATE OR ALTER PROCEDURE [dbo].[sp_EnsureLoginSecurityColumns]
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Thêm cột FailedLoginCount nếu chưa có
    IF COL_LENGTH('dbo.Users', 'FailedLoginCount') IS NULL
    BEGIN
        ALTER TABLE dbo.Users
        ADD FailedLoginCount INT NOT NULL
            CONSTRAINT DF_Users_FailedLoginCount DEFAULT (0);
    END;

    -- Thêm cột LockoutUntil nếu chưa có
    IF COL_LENGTH('dbo.Users', 'LockoutUntil') IS NULL
    BEGIN
        ALTER TABLE dbo.Users
        ADD LockoutUntil DATETIME NULL;
    END;
END
GO

