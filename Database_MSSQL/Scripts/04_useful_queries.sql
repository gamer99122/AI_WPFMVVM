-- =============================================
-- 常用查詢範例
-- 用於測試和學習 MSSQL 查詢
-- =============================================
USE WpfMvvmDemo;
GO

-- =============================================
-- 1. 基本查詢
-- =============================================

-- 查詢所有客戶
SELECT * FROM Customers ORDER BY CreatedAt DESC;

-- 查詢特定城市的客戶
SELECT * FROM Customers WHERE City = N'台北市';

-- 模糊搜尋客戶名稱
SELECT * FROM Customers WHERE Name LIKE N'%王%';

-- =============================================
-- 2. 聚合查詢
-- =============================================

-- 計算客戶總數
SELECT COUNT(*) AS TotalCustomers FROM Customers WHERE IsActive = 1;

-- 計算訂單總金額
SELECT SUM(TotalAmount) AS TotalSales FROM Orders WHERE Status = 'Delivered';

-- 每個客戶的訂單數量和總消費
SELECT
    c.Id,
    c.Name,
    COUNT(o.Id) AS OrderCount,
    ISNULL(SUM(o.TotalAmount), 0) AS TotalSpent
FROM Customers c
LEFT JOIN Orders o ON c.Id = o.CustomerId
GROUP BY c.Id, c.Name
ORDER BY TotalSpent DESC;

-- =============================================
-- 3. JOIN 查詢
-- =============================================

-- 查詢訂單及客戶資訊
SELECT
    o.Id,
    o.OrderNumber,
    o.OrderDate,
    o.Status,
    o.TotalAmount,
    c.Name AS CustomerName,
    c.Email AS CustomerEmail,
    c.Phone AS CustomerPhone
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.Id
ORDER BY o.OrderDate DESC;

-- 查詢訂單明細（含產品資訊）
SELECT
    o.OrderNumber,
    oi.ProductName,
    oi.Quantity,
    oi.UnitPrice,
    oi.Discount,
    oi.Subtotal,
    p.Stock AS CurrentStock
FROM OrderItems oi
INNER JOIN Orders o ON oi.OrderId = o.Id
INNER JOIN Products p ON oi.ProductId = p.Id
WHERE o.Id = 1;

-- =============================================
-- 4. 子查詢
-- =============================================

-- 查詢購買金額超過平均值的客戶
SELECT
    c.Name,
    SUM(o.TotalAmount) AS TotalSpent
FROM Customers c
INNER JOIN Orders o ON c.Id = o.CustomerId
WHERE o.Status = 'Delivered'
GROUP BY c.Id, c.Name
HAVING SUM(o.TotalAmount) > (
    SELECT AVG(TotalAmount) FROM Orders WHERE Status = 'Delivered'
);

-- 查詢從未下過訂單的客戶
SELECT * FROM Customers
WHERE Id NOT IN (SELECT DISTINCT CustomerId FROM Orders);

-- =============================================
-- 5. 統計查詢
-- =============================================

-- 產品銷售排行榜
SELECT TOP 10
    p.Name,
    p.Category,
    SUM(oi.Quantity) AS TotalSold,
    SUM(oi.Subtotal) AS TotalRevenue
FROM Products p
INNER JOIN OrderItems oi ON p.Id = oi.ProductId
INNER JOIN Orders o ON oi.OrderId = o.Id
WHERE o.Status IN ('Delivered', 'Shipped')
GROUP BY p.Id, p.Name, p.Category
ORDER BY TotalRevenue DESC;

-- 月度銷售統計
SELECT
    FORMAT(OrderDate, 'yyyy-MM') AS Month,
    COUNT(*) AS OrderCount,
    SUM(TotalAmount) AS TotalSales
FROM Orders
WHERE Status = 'Delivered'
GROUP BY FORMAT(OrderDate, 'yyyy-MM')
ORDER BY Month DESC;

-- 城市銷售統計
SELECT
    c.City,
    COUNT(DISTINCT c.Id) AS CustomerCount,
    COUNT(o.Id) AS OrderCount,
    SUM(o.TotalAmount) AS TotalSales
FROM Customers c
LEFT JOIN Orders o ON c.Id = o.CustomerId AND o.Status = 'Delivered'
WHERE c.City IS NOT NULL
GROUP BY c.City
ORDER BY TotalSales DESC;

-- =============================================
-- 6. 庫存管理查詢
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

-- 庫存異動歷史
SELECT
    t.CreatedAt,
    t.TransactionType,
    t.Quantity,
    t.Reference,
    p.Name AS ProductName,
    p.Stock AS CurrentStock
FROM InventoryTransactions t
INNER JOIN Products p ON t.ProductId = p.Id
WHERE t.ProductId = 1
ORDER BY t.CreatedAt DESC;

-- =============================================
-- 7. 日期查詢
-- =============================================

-- 查詢最近 7 天的訂單
SELECT * FROM Orders
WHERE OrderDate >= DATEADD(DAY, -7, GETDATE())
ORDER BY OrderDate DESC;

-- 查詢本月訂單
SELECT * FROM Orders
WHERE YEAR(OrderDate) = YEAR(GETDATE())
  AND MONTH(OrderDate) = MONTH(GETDATE());

-- 查詢本年度訂單統計
SELECT
    MONTH(OrderDate) AS Month,
    COUNT(*) AS OrderCount,
    SUM(TotalAmount) AS MonthlyTotal
FROM Orders
WHERE YEAR(OrderDate) = YEAR(GETDATE())
  AND Status = 'Delivered'
GROUP BY MONTH(OrderDate)
ORDER BY Month;

-- =============================================
-- 8. 進階查詢
-- =============================================

-- 客戶價值分析（RFM 簡化版）
SELECT
    c.Id,
    c.Name,
    c.Email,
    COUNT(o.Id) AS Frequency,
    MAX(o.OrderDate) AS Recency,
    SUM(o.TotalAmount) AS Monetary,
    CASE
        WHEN SUM(o.TotalAmount) >= 50000 THEN 'VIP'
        WHEN SUM(o.TotalAmount) >= 20000 THEN '高價值'
        WHEN SUM(o.TotalAmount) >= 5000 THEN '中價值'
        ELSE '一般'
    END AS CustomerTier
FROM Customers c
LEFT JOIN Orders o ON c.Id = o.CustomerId AND o.Status = 'Delivered'
WHERE c.IsActive = 1
GROUP BY c.Id, c.Name, c.Email
ORDER BY Monetary DESC;

-- 產品組合分析（哪些產品經常一起購買）
SELECT
    oi1.ProductName AS Product1,
    oi2.ProductName AS Product2,
    COUNT(*) AS Frequency
FROM OrderItems oi1
INNER JOIN OrderItems oi2 ON oi1.OrderId = oi2.OrderId AND oi1.ProductId < oi2.ProductId
GROUP BY oi1.ProductName, oi2.ProductName
HAVING COUNT(*) > 1
ORDER BY Frequency DESC;

-- =============================================
-- 9. 視圖查詢
-- =============================================

-- 使用訂單摘要視圖
SELECT * FROM vw_OrderSummary WHERE Status = 'Pending';

-- 使用產品庫存視圖
SELECT * FROM vw_ProductInventory WHERE StockStatus = 'Low Stock';

-- =============================================
-- 10. 分頁查詢
-- =============================================

-- 使用 OFFSET/FETCH 分頁（SQL Server 2012+）
DECLARE @PageNumber INT = 1;
DECLARE @PageSize INT = 10;

SELECT * FROM Customers
ORDER BY Id
OFFSET (@PageNumber - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;

-- =============================================
-- 11. CTE（Common Table Expression）範例
-- =============================================

-- 使用 CTE 計算客戶統計
WITH CustomerStats AS (
    SELECT
        c.Id,
        c.Name,
        COUNT(o.Id) AS OrderCount,
        SUM(o.TotalAmount) AS TotalSpent
    FROM Customers c
    LEFT JOIN Orders o ON c.Id = o.CustomerId AND o.Status = 'Delivered'
    GROUP BY c.Id, c.Name
)
SELECT
    Name,
    OrderCount,
    TotalSpent,
    CASE
        WHEN OrderCount = 0 THEN '未消費'
        WHEN OrderCount = 1 THEN '新客戶'
        WHEN OrderCount >= 5 THEN '忠實客戶'
        ELSE '一般客戶'
    END AS CustomerType
FROM CustomerStats
ORDER BY TotalSpent DESC;

-- =============================================
-- 12. 窗口函數（Window Function）範例
-- =============================================

-- 為每個城市的客戶編號
SELECT
    Name,
    City,
    Email,
    ROW_NUMBER() OVER (PARTITION BY City ORDER BY CreatedAt) AS CityRank
FROM Customers
WHERE City IS NOT NULL
ORDER BY City, CityRank;

-- 計算累計銷售額
SELECT
    OrderDate,
    OrderNumber,
    TotalAmount,
    SUM(TotalAmount) OVER (ORDER BY OrderDate) AS CumulativeSales
FROM Orders
WHERE Status = 'Delivered'
ORDER BY OrderDate;

-- =============================================
-- 完成
-- =============================================

PRINT '常用查詢範例載入完成！';
GO
