-- =============================================
-- WPF MVVM Dapper 範例 - 資料庫架構
-- 資料庫: SQLite
-- =============================================

-- 啟用外鍵約束
PRAGMA foreign_keys = ON;

-- =============================================
-- 1. Customers 資料表 (客戶)
-- =============================================
CREATE TABLE IF NOT EXISTS Customers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    Phone TEXT,
    Address TEXT,
    City TEXT,
    Country TEXT DEFAULT 'Taiwan',
    PostalCode TEXT,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    UpdatedAt TEXT,
    IsActive INTEGER NOT NULL DEFAULT 1,
    Notes TEXT
);

-- 建立索引以提升查詢效能
CREATE INDEX IF NOT EXISTS idx_customers_email ON Customers(Email);
CREATE INDEX IF NOT EXISTS idx_customers_name ON Customers(Name);
CREATE INDEX IF NOT EXISTS idx_customers_created ON Customers(CreatedAt);

-- =============================================
-- 2. Products 資料表 (產品)
-- =============================================
CREATE TABLE IF NOT EXISTS Products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    Category TEXT NOT NULL,
    Price REAL NOT NULL CHECK(Price >= 0),
    Stock INTEGER NOT NULL DEFAULT 0 CHECK(Stock >= 0),
    MinStock INTEGER DEFAULT 10,
    SKU TEXT UNIQUE,
    Barcode TEXT,
    ImageUrl TEXT,
    IsActive INTEGER NOT NULL DEFAULT 1,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    UpdatedAt TEXT
);

-- 建立索引
CREATE INDEX IF NOT EXISTS idx_products_category ON Products(Category);
CREATE INDEX IF NOT EXISTS idx_products_sku ON Products(SKU);
CREATE INDEX IF NOT EXISTS idx_products_name ON Products(Name);

-- =============================================
-- 3. Orders 資料表 (訂單)
-- =============================================
CREATE TABLE IF NOT EXISTS Orders (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderNumber TEXT NOT NULL UNIQUE,
    CustomerId INTEGER NOT NULL,
    OrderDate TEXT NOT NULL DEFAULT (datetime('now')),
    RequiredDate TEXT,
    ShippedDate TEXT,
    Status TEXT NOT NULL DEFAULT 'Pending',
    -- Status: Pending, Processing, Shipped, Delivered, Cancelled
    TotalAmount REAL NOT NULL DEFAULT 0,
    ShippingAddress TEXT,
    Notes TEXT,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    UpdatedAt TEXT,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE RESTRICT
);

-- 建立索引
CREATE INDEX IF NOT EXISTS idx_orders_customer ON Orders(CustomerId);
CREATE INDEX IF NOT EXISTS idx_orders_number ON Orders(OrderNumber);
CREATE INDEX IF NOT EXISTS idx_orders_date ON Orders(OrderDate);
CREATE INDEX IF NOT EXISTS idx_orders_status ON Orders(Status);

-- =============================================
-- 4. OrderItems 資料表 (訂單明細)
-- =============================================
CREATE TABLE IF NOT EXISTS OrderItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    ProductName TEXT NOT NULL,
    Quantity INTEGER NOT NULL CHECK(Quantity > 0),
    UnitPrice REAL NOT NULL CHECK(UnitPrice >= 0),
    Discount REAL DEFAULT 0 CHECK(Discount >= 0 AND Discount <= 1),
    Subtotal REAL GENERATED ALWAYS AS (Quantity * UnitPrice * (1 - Discount)) STORED,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE RESTRICT
);

-- 建立索引
CREATE INDEX IF NOT EXISTS idx_orderitems_order ON OrderItems(OrderId);
CREATE INDEX IF NOT EXISTS idx_orderitems_product ON OrderItems(ProductId);

-- =============================================
-- 5. Categories 資料表 (產品類別)
-- =============================================
CREATE TABLE IF NOT EXISTS Categories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    Description TEXT,
    ParentId INTEGER,
    DisplayOrder INTEGER DEFAULT 0,
    IsActive INTEGER NOT NULL DEFAULT 1,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (ParentId) REFERENCES Categories(Id) ON DELETE SET NULL
);

-- =============================================
-- 6. Suppliers 資料表 (供應商)
-- =============================================
CREATE TABLE IF NOT EXISTS Suppliers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyName TEXT NOT NULL,
    ContactName TEXT,
    Email TEXT,
    Phone TEXT,
    Address TEXT,
    City TEXT,
    Country TEXT DEFAULT 'Taiwan',
    Website TEXT,
    IsActive INTEGER NOT NULL DEFAULT 1,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    UpdatedAt TEXT
);

-- =============================================
-- 7. ProductSuppliers 資料表 (產品-供應商關聯)
-- =============================================
CREATE TABLE IF NOT EXISTS ProductSuppliers (
    ProductId INTEGER NOT NULL,
    SupplierId INTEGER NOT NULL,
    SupplyPrice REAL NOT NULL CHECK(SupplyPrice >= 0),
    LeadTimeDays INTEGER DEFAULT 7,
    IsPreferred INTEGER DEFAULT 0,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    PRIMARY KEY (ProductId, SupplierId),
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id) ON DELETE CASCADE
);

-- =============================================
-- 8. Inventory 資料表 (庫存異動記錄)
-- =============================================
CREATE TABLE IF NOT EXISTS InventoryTransactions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductId INTEGER NOT NULL,
    TransactionType TEXT NOT NULL,
    -- TransactionType: Purchase, Sale, Adjustment, Return
    Quantity INTEGER NOT NULL,
    UnitPrice REAL,
    Reference TEXT,
    Notes TEXT,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    CreatedBy TEXT,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS idx_inventory_product ON InventoryTransactions(ProductId);
CREATE INDEX IF NOT EXISTS idx_inventory_date ON InventoryTransactions(CreatedAt);

-- =============================================
-- 建立視圖 (Views)
-- =============================================

-- 訂單摘要視圖
CREATE VIEW IF NOT EXISTS vw_OrderSummary AS
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
GROUP BY o.Id, o.OrderNumber, o.OrderDate, o.Status, c.Name, c.Email, o.TotalAmount;

-- 產品庫存視圖
CREATE VIEW IF NOT EXISTS vw_ProductInventory AS
SELECT
    p.Id,
    p.Name,
    p.Category,
    p.Price,
    p.Stock,
    p.MinStock,
    CASE
        WHEN p.Stock <= p.MinStock THEN 'Low Stock'
        WHEN p.Stock = 0 THEN 'Out of Stock'
        ELSE 'In Stock'
    END AS StockStatus
FROM Products p
WHERE p.IsActive = 1;

-- =============================================
-- 完成
-- =============================================
