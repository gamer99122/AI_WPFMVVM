-- =============================================
-- 建立資料表
-- =============================================
USE WpfMvvmDemo;
GO

-- =============================================
-- 1. Customers 資料表（客戶）
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
BEGIN
    CREATE TABLE Customers (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        Phone NVARCHAR(20),
        City NVARCHAR(50),
        Address NVARCHAR(200),
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,

        INDEX IX_Customers_Email (Email),
        INDEX IX_Customers_City (City),
        INDEX IX_Customers_CreatedAt (CreatedAt)
    );

    PRINT 'Customers 資料表建立成功';
END
ELSE
BEGIN
    PRINT 'Customers 資料表已存在';
END
GO

-- =============================================
-- 2. Products 資料表（產品）
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500),
        Category NVARCHAR(50) NOT NULL,
        Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
        Stock INT NOT NULL DEFAULT 0 CHECK (Stock >= 0),
        MinStock INT NOT NULL DEFAULT 10,
        SKU NVARCHAR(50) UNIQUE,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME2 NULL,

        INDEX IX_Products_Category (Category),
        INDEX IX_Products_SKU (SKU),
        INDEX IX_Products_Name (Name)
    );

    PRINT 'Products 資料表建立成功';
END
ELSE
BEGIN
    PRINT 'Products 資料表已存在';
END
GO

-- =============================================
-- 3. Orders 資料表（訂單主檔）
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        Id INT PRIMARY KEY IDENTITY(1,1),
        OrderNumber NVARCHAR(50) NOT NULL UNIQUE,
        CustomerId INT NOT NULL,
        OrderDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        RequiredDate DATETIME2 NULL,
        ShippedDate DATETIME2 NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
        -- Status: Pending, Processing, Shipped, Delivered, Cancelled
        TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        ShippingAddress NVARCHAR(200),
        Notes NVARCHAR(500),
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME2 NULL,

        CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId)
            REFERENCES Customers(Id) ON DELETE NO ACTION,

        INDEX IX_Orders_CustomerId (CustomerId),
        INDEX IX_Orders_OrderNumber (OrderNumber),
        INDEX IX_Orders_OrderDate (OrderDate),
        INDEX IX_Orders_Status (Status),

        CONSTRAINT CK_Orders_Status CHECK (Status IN ('Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled'))
    );

    PRINT 'Orders 資料表建立成功';
END
ELSE
BEGIN
    PRINT 'Orders 資料表已存在';
END
GO

-- =============================================
-- 4. OrderItems 資料表（訂單明細）
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItems')
BEGIN
    CREATE TABLE OrderItems (
        Id INT PRIMARY KEY IDENTITY(1,1),
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        ProductName NVARCHAR(100) NOT NULL,
        Quantity INT NOT NULL CHECK (Quantity > 0),
        UnitPrice DECIMAL(18,2) NOT NULL CHECK (UnitPrice >= 0),
        Discount DECIMAL(5,2) NOT NULL DEFAULT 0 CHECK (Discount >= 0 AND Discount <= 1),
        Subtotal AS (Quantity * UnitPrice * (1 - Discount)) PERSISTED,

        CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId)
            REFERENCES Orders(Id) ON DELETE CASCADE,

        CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId)
            REFERENCES Products(Id) ON DELETE NO ACTION,

        INDEX IX_OrderItems_OrderId (OrderId),
        INDEX IX_OrderItems_ProductId (ProductId)
    );

    PRINT 'OrderItems 資料表建立成功';
END
ELSE
BEGIN
    PRINT 'OrderItems 資料表已存在';
END
GO

-- =============================================
-- 5. Categories 資料表（產品類別）
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(50) NOT NULL UNIQUE,
        Description NVARCHAR(200),
        ParentId INT NULL,
        DisplayOrder INT NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

        CONSTRAINT FK_Categories_Parent FOREIGN KEY (ParentId)
            REFERENCES Categories(Id) ON DELETE NO ACTION
    );

    PRINT 'Categories 資料表建立成功';
END
ELSE
BEGIN
    PRINT 'Categories 資料表已存在';
END
GO

-- =============================================
-- 6. InventoryTransactions 資料表（庫存異動記錄）
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'InventoryTransactions')
BEGIN
    CREATE TABLE InventoryTransactions (
        Id INT PRIMARY KEY IDENTITY(1,1),
        ProductId INT NOT NULL,
        TransactionType NVARCHAR(20) NOT NULL,
        -- TransactionType: Purchase, Sale, Adjustment, Return
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2),
        Reference NVARCHAR(100),
        Notes NVARCHAR(200),
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        CreatedBy NVARCHAR(50),

        CONSTRAINT FK_InventoryTransactions_Products FOREIGN KEY (ProductId)
            REFERENCES Products(Id) ON DELETE NO ACTION,

        INDEX IX_InventoryTransactions_ProductId (ProductId),
        INDEX IX_InventoryTransactions_CreatedAt (CreatedAt),

        CONSTRAINT CK_InventoryTransactions_Type CHECK (TransactionType IN ('Purchase', 'Sale', 'Adjustment', 'Return'))
    );

    PRINT 'InventoryTransactions 資料表建立成功';
END
ELSE
BEGIN
    PRINT 'InventoryTransactions 資料表已存在';
END
GO

-- =============================================
-- 建立視圖
-- =============================================

-- 訂單摘要視圖
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vw_OrderSummary')
BEGIN
    EXEC('
    CREATE VIEW vw_OrderSummary
    AS
    SELECT
        o.Id,
        o.OrderNumber,
        o.OrderDate,
        o.Status,
        c.Name AS CustomerName,
        c.Email AS CustomerEmail,
        o.TotalAmount,
        COUNT(oi.Id) AS ItemCount
    FROM Orders o
    INNER JOIN Customers c ON o.CustomerId = c.Id
    LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
    GROUP BY o.Id, o.OrderNumber, o.OrderDate, o.Status, c.Name, c.Email, o.TotalAmount
    ');

    PRINT 'vw_OrderSummary 視圖建立成功';
END
ELSE
BEGIN
    PRINT 'vw_OrderSummary 視圖已存在';
END
GO

-- 產品庫存視圖
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ProductInventory')
BEGIN
    EXEC('
    CREATE VIEW vw_ProductInventory
    AS
    SELECT
        p.Id,
        p.Name,
        p.Category,
        p.Price,
        p.Stock,
        p.MinStock,
        CASE
            WHEN p.Stock <= p.MinStock THEN ''Low Stock''
            WHEN p.Stock = 0 THEN ''Out of Stock''
            ELSE ''In Stock''
        END AS StockStatus
    FROM Products p
    WHERE p.IsActive = 1
    ');

    PRINT 'vw_ProductInventory 視圖建立成功';
END
ELSE
BEGIN
    PRINT 'vw_ProductInventory 視圖已存在';
END
GO

PRINT '所有資料表建立完成！';
GO
