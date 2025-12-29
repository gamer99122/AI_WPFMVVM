-- =============================================
-- 常用 Dapper 查詢範例
-- =============================================

-- =============================================
-- 1. 基本查詢
-- =============================================

-- 查詢所有客戶
SELECT * FROM Customers WHERE IsActive = 1 ORDER BY CreatedAt DESC;

-- 根據 ID 查詢客戶
SELECT * FROM Customers WHERE Id = @Id;

-- 模糊搜尋客戶名稱
SELECT * FROM Customers WHERE Name LIKE '%' || @SearchText || '%';

-- =============================================
-- 2. 聚合查詢
-- =============================================

-- 計算客戶總數
SELECT COUNT(*) AS TotalCustomers FROM Customers WHERE IsActive = 1;

-- 計算訂單總金額
SELECT SUM(TotalAmount) AS TotalSales FROM Orders WHERE Status = 'Delivered';

-- 每個客戶的訂單數量
SELECT
    c.Id,
    c.Name,
    COUNT(o.Id) AS OrderCount,
    COALESCE(SUM(o.TotalAmount), 0) AS TotalSpent
FROM Customers c
LEFT JOIN Orders o ON c.Id = o.CustomerId
GROUP BY c.Id, c.Name
ORDER BY TotalSpent DESC;

-- =============================================
-- 3. 關聯查詢
-- =============================================

-- 查詢訂單及客戶資訊
SELECT
    o.Id,
    o.OrderNumber,
    o.OrderDate,
    o.Status,
    o.TotalAmount,
    c.Id AS CustomerId,
    c.Name AS CustomerName,
    c.Email AS CustomerEmail
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.Id
WHERE o.Id = @OrderId;

-- 查詢訂單明細
SELECT
    oi.Id,
    oi.ProductId,
    oi.ProductName,
    oi.Quantity,
    oi.UnitPrice,
    oi.Discount,
    oi.Subtotal,
    p.Stock AS CurrentStock
FROM OrderItems oi
INNER JOIN Products p ON oi.ProductId = p.Id
WHERE oi.OrderId = @OrderId;

-- =============================================
-- 4. 統計查詢
-- =============================================

-- 銷售排行榜（按產品）
SELECT
    p.Id,
    p.Name,
    p.Category,
    SUM(oi.Quantity) AS TotalSold,
    SUM(oi.Subtotal) AS TotalRevenue
FROM Products p
INNER JOIN OrderItems oi ON p.Id = oi.ProductId
INNER JOIN Orders o ON oi.OrderId = o.Id
WHERE o.Status IN ('Delivered', 'Shipped')
GROUP BY p.Id, p.Name, p.Category
ORDER BY TotalRevenue DESC
LIMIT 10;

-- 月度銷售統計
SELECT
    strftime('%Y-%m', OrderDate) AS Month,
    COUNT(*) AS OrderCount,
    SUM(TotalAmount) AS TotalSales
FROM Orders
WHERE Status = 'Delivered'
GROUP BY strftime('%Y-%m', OrderDate)
ORDER BY Month DESC;

-- 客戶價值分析
SELECT
    c.Id,
    c.Name,
    c.Email,
    COUNT(o.Id) AS OrderCount,
    SUM(o.TotalAmount) AS TotalSpent,
    MAX(o.OrderDate) AS LastOrderDate,
    CASE
        WHEN SUM(o.TotalAmount) >= 50000 THEN 'VIP'
        WHEN SUM(o.TotalAmount) >= 20000 THEN '高價值'
        ELSE '一般'
    END AS CustomerTier
FROM Customers c
LEFT JOIN Orders o ON c.Id = o.CustomerId AND o.Status = 'Delivered'
GROUP BY c.Id, c.Name, c.Email
HAVING OrderCount > 0
ORDER BY TotalSpent DESC;

-- =============================================
-- 5. 庫存管理查詢
-- =============================================

-- 低庫存警示
SELECT
    Id,
    Name,
    Category,
    Stock,
    MinStock,
    (MinStock - Stock) AS NeedRestock
FROM Products
WHERE Stock <= MinStock AND IsActive = 1
ORDER BY NeedRestock DESC;

-- 產品庫存異動歷史
SELECT
    t.Id,
    t.TransactionType,
    t.Quantity,
    t.Reference,
    t.CreatedAt,
    p.Name AS ProductName,
    p.Stock AS CurrentStock
FROM InventoryTransactions t
INNER JOIN Products p ON t.ProductId = p.Id
WHERE t.ProductId = @ProductId
ORDER BY t.CreatedAt DESC;

-- =============================================
-- 6. 訂單管理查詢
-- =============================================

-- 待處理訂單
SELECT
    o.Id,
    o.OrderNumber,
    o.OrderDate,
    c.Name AS CustomerName,
    o.TotalAmount,
    COUNT(oi.Id) AS ItemCount
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.Id
LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
WHERE o.Status IN ('Pending', 'Processing')
GROUP BY o.Id, o.OrderNumber, o.OrderDate, c.Name, o.TotalAmount
ORDER BY o.OrderDate ASC;

-- 訂單完整資訊（含明細）
-- 這個查詢適合用 Dapper Multi-Mapping
SELECT
    o.*,
    c.Name AS CustomerName,
    c.Email AS CustomerEmail,
    c.Phone AS CustomerPhone
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.Id
WHERE o.Id = @OrderId;

-- 同時查詢訂單明細
SELECT * FROM OrderItems WHERE OrderId = @OrderId;

-- =============================================
-- 7. 搜尋與篩選
-- =============================================

-- 產品搜尋（多條件）
SELECT * FROM Products
WHERE IsActive = 1
    AND (@Category IS NULL OR Category = @Category)
    AND (@MinPrice IS NULL OR Price >= @MinPrice)
    AND (@MaxPrice IS NULL OR Price <= @MaxPrice)
    AND (@SearchText IS NULL OR Name LIKE '%' || @SearchText || '%')
ORDER BY Name;

-- 訂單搜尋（日期範圍）
SELECT
    o.*,
    c.Name AS CustomerName
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.Id
WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
    AND (@Status IS NULL OR o.Status = @Status)
ORDER BY o.OrderDate DESC;

-- =============================================
-- 8. Dapper 特殊查詢範例
-- =============================================

-- 批次插入範例（程式碼中使用）
/*
var customers = new List<Customer> { ... };
connection.Execute(@"
    INSERT INTO Customers (Name, Email, Phone, Address, City, Country)
    VALUES (@Name, @Email, @Phone, @Address, @City, @Country)
", customers);
*/

-- 更新庫存（事務）
/*
using (var transaction = connection.BeginTransaction())
{
    // 減少庫存
    connection.Execute(@"
        UPDATE Products
        SET Stock = Stock - @Quantity, UpdatedAt = datetime('now')
        WHERE Id = @ProductId AND Stock >= @Quantity
    ", new { ProductId, Quantity }, transaction);

    // 記錄異動
    connection.Execute(@"
        INSERT INTO InventoryTransactions (ProductId, TransactionType, Quantity, Reference)
        VALUES (@ProductId, 'Sale', @Quantity, @OrderNumber)
    ", new { ProductId, Quantity, OrderNumber }, transaction);

    transaction.Commit();
}
*/

-- =============================================
-- 9. 效能優化查詢
-- =============================================

-- 使用索引的查詢
SELECT * FROM Customers WHERE Email = @Email;  -- 使用 idx_customers_email

-- 避免 SELECT *，只查詢需要的欄位
SELECT Id, Name, Email FROM Customers WHERE IsActive = 1;

-- 分頁查詢
SELECT * FROM Products
WHERE IsActive = 1
ORDER BY CreatedAt DESC
LIMIT @PageSize OFFSET @Offset;

-- =============================================
-- 10. 報表查詢
-- =============================================

-- 每日銷售報表
SELECT
    DATE(OrderDate) AS SaleDate,
    COUNT(*) AS OrderCount,
    SUM(TotalAmount) AS DailySales
FROM Orders
WHERE Status = 'Delivered'
    AND OrderDate >= datetime('now', '-30 days')
GROUP BY DATE(OrderDate)
ORDER BY SaleDate DESC;

-- 產品類別銷售分析
SELECT
    p.Category,
    COUNT(DISTINCT p.Id) AS ProductCount,
    SUM(oi.Quantity) AS TotalQuantitySold,
    SUM(oi.Subtotal) AS TotalRevenue,
    AVG(oi.UnitPrice) AS AveragePrice
FROM Products p
INNER JOIN OrderItems oi ON p.Id = oi.ProductId
INNER JOIN Orders o ON oi.OrderId = o.Id
WHERE o.Status = 'Delivered'
GROUP BY p.Category
ORDER BY TotalRevenue DESC;

-- =============================================
-- 完成
-- =============================================
