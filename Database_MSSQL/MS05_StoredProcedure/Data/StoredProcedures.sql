-- =============================================
-- MSSQL 預存程序範例
-- =============================================

-- =============================================
-- 1. 基本查詢預存程序
-- =============================================
CREATE OR ALTER PROCEDURE sp_GetAllCustomers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Email, Phone, City, CreatedAt
    FROM Customers
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- 2. 帶輸入參數的預存程序
-- =============================================
CREATE OR ALTER PROCEDURE sp_GetCustomerById
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Email, Phone, City, CreatedAt
    FROM Customers
    WHERE Id = @CustomerId;
END
GO

-- =============================================
-- 3. 多個輸入參數的預存程序
-- =============================================
CREATE OR ALTER PROCEDURE sp_SearchCustomers
    @City NVARCHAR(50) = NULL,
    @NamePattern NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Email, Phone, City, CreatedAt
    FROM Customers
    WHERE (@City IS NULL OR City = @City)
      AND (@NamePattern IS NULL OR Name LIKE '%' + @NamePattern + '%')
    ORDER BY Name;
END
GO

-- =============================================
-- 4. 新增資料並返回 ID（輸出參數）
-- =============================================
CREATE OR ALTER PROCEDURE sp_AddCustomer
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20) = NULL,
    @City NVARCHAR(50) = NULL,
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- 檢查 Email 是否已存在
    IF EXISTS(SELECT 1 FROM Customers WHERE Email = @Email)
    BEGIN
        RAISERROR('Email 已存在', 16, 1);
        RETURN -1;
    END

    -- 插入資料
    INSERT INTO Customers (Name, Email, Phone, City, CreatedAt)
    VALUES (@Name, @Email, @Phone, @City, GETDATE());

    -- 取得新增的 ID
    SET @NewId = SCOPE_IDENTITY();

    RETURN 0; -- 成功
END
GO

-- =============================================
-- 5. 更新資料
-- =============================================
CREATE OR ALTER PROCEDURE sp_UpdateCustomer
    @CustomerId INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20) = NULL,
    @City NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 檢查客戶是否存在
    IF NOT EXISTS(SELECT 1 FROM Customers WHERE Id = @CustomerId)
    BEGIN
        RAISERROR('客戶不存在', 16, 1);
        RETURN -1;
    END

    -- 檢查 Email 是否被其他客戶使用
    IF EXISTS(SELECT 1 FROM Customers WHERE Email = @Email AND Id != @CustomerId)
    BEGIN
        RAISERROR('Email 已被其他客戶使用', 16, 1);
        RETURN -2;
    END

    -- 更新資料
    UPDATE Customers
    SET Name = @Name,
        Email = @Email,
        Phone = @Phone,
        City = @City
    WHERE Id = @CustomerId;

    RETURN 0; -- 成功
END
GO

-- =============================================
-- 6. 刪除資料
-- =============================================
CREATE OR ALTER PROCEDURE sp_DeleteCustomer
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 檢查客戶是否存在
    IF NOT EXISTS(SELECT 1 FROM Customers WHERE Id = @CustomerId)
    BEGIN
        RAISERROR('客戶不存在', 16, 1);
        RETURN -1;
    END

    -- 刪除客戶
    DELETE FROM Customers WHERE Id = @CustomerId;

    RETURN 0; -- 成功
END
GO

-- =============================================
-- 7. 統計查詢
-- =============================================
CREATE OR ALTER PROCEDURE sp_GetCustomerStatistics
    @TotalCount INT OUTPUT,
    @CityCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- 總客戶數
    SELECT @TotalCount = COUNT(*) FROM Customers;

    -- 城市數量
    SELECT @CityCount = COUNT(DISTINCT City) FROM Customers WHERE City IS NOT NULL;
END
GO

-- =============================================
-- 8. 複雜業務邏輯 - 建立訂單
-- =============================================
CREATE OR ALTER PROCEDURE sp_CreateOrder
    @CustomerId INT,
    @OrderItems NVARCHAR(MAX), -- JSON 格式的訂單明細
    @OrderId INT OUTPUT,
    @TotalAmount DECIMAL(18,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        -- 檢查客戶是否存在
        IF NOT EXISTS(SELECT 1 FROM Customers WHERE Id = @CustomerId)
        BEGIN
            RAISERROR('客戶不存在', 16, 1);
            RETURN -1;
        END

        -- 建立訂單主檔
        INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, Status)
        VALUES (@CustomerId, GETDATE(), 0, 'Pending');

        SET @OrderId = SCOPE_IDENTITY();

        -- 這裡應該解析 JSON 並插入訂單明細
        -- 為了簡化範例，這裡省略

        SET @TotalAmount = 0; -- 應該從訂單明細計算

        -- 更新訂單總金額
        UPDATE Orders SET TotalAmount = @TotalAmount WHERE Id = @OrderId;

        COMMIT TRANSACTION;
        RETURN 0; -- 成功
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -99;
    END CATCH
END
GO

-- =============================================
-- 9. 批次處理
-- =============================================
CREATE OR ALTER PROCEDURE sp_DeleteInactiveCustomers
    @DaysSinceCreated INT,
    @DeletedCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Customers
    WHERE DATEDIFF(DAY, CreatedAt, GETDATE()) > @DaysSinceCreated
      AND Id NOT IN (SELECT DISTINCT CustomerId FROM Orders);

    SET @DeletedCount = @@ROWCOUNT;

    RETURN 0;
END
GO

-- =============================================
-- 10. 檢查預存程序
-- =============================================
CREATE OR ALTER PROCEDURE sp_CheckEmailExists
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS(SELECT 1 FROM Customers WHERE Email = @Email)
        RETURN 1; -- 存在
    ELSE
        RETURN 0; -- 不存在
END
GO

-- =============================================
-- 測試預存程序
-- =============================================

-- 測試 1: 取得所有客戶
EXEC sp_GetAllCustomers;

-- 測試 2: 取得特定客戶
EXEC sp_GetCustomerById @CustomerId = 1;

-- 測試 3: 搜尋客戶
EXEC sp_SearchCustomers @City = '台北市';
EXEC sp_SearchCustomers @NamePattern = '王';

-- 測試 4: 新增客戶（帶輸出參數）
DECLARE @NewCustomerId INT;
EXEC sp_AddCustomer
    @Name = '測試客戶',
    @Email = 'test@example.com',
    @Phone = '0900-000-000',
    @City = '台北市',
    @NewId = @NewCustomerId OUTPUT;
SELECT @NewCustomerId AS NewCustomerId;

-- 測試 5: 取得統計資料
DECLARE @Total INT, @Cities INT;
EXEC sp_GetCustomerStatistics @TotalCount = @Total OUTPUT, @CityCount = @Cities OUTPUT;
SELECT @Total AS TotalCustomers, @Cities AS TotalCities;

-- 測試 6: 檢查 Email
DECLARE @ReturnValue INT;
EXEC @ReturnValue = sp_CheckEmailExists @Email = 'test@example.com';
SELECT @ReturnValue AS EmailExists;
