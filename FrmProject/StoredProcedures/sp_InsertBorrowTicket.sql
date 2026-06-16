-- 36. THỦ TỤC TẠO PHIẾU MƯỢN (MASTER)
CREATE OR ALTER PROCEDURE [dbo].[sp_InsertBorrowTicket]
    @userID INT,
    @createdBy INT,
    @ticketCode NVARCHAR(100),
    @borrowDate DATETIME,
    @returnDate DATETIME,
    @purpose NVARCHAR(250),
    @status NVARCHAR(50),
    @note NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO BorrowTickets 
        (UserID, CreatedBy, TicketCode, BorrowDate, ExpectedReturnDate, Purpose, Status, Note)
    VALUES 
        (@userID, @createdBy, @ticketCode, @borrowDate, @returnDate, @purpose, @status, @note);
    
    SELECT SCOPE_IDENTITY();
END
GO

