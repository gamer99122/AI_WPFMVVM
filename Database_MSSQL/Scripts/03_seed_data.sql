-- =============================================
-- 插入範例資料
-- =============================================
USE WpfMvvmDemo;
GO

PRINT '開始插入範例資料...';
GO

-- =============================================
-- 1. 插入客戶資料
-- =============================================
IF NOT EXISTS (SELECT * FROM Customers)
BEGIN
    INSERT INTO Customers (Name, Email, Phone, City, Address) VALUES
    ('王小明', 'wang.xiaoming@example.com', '0912-345-678', '台北市', '信義區信義路五段7號'),
    ('李美美', 'li.meimei@example.com', '0923-456-789', '新北市', '板橋區中山路一段152號'),
    ('張大力', 'zhang.dali@example.com', '0934-567-890', '台中市', '西屯區台灣大道三段99號'),
    ('陳小華', 'chen.xiaohua@example.com', '0945-678-901', '高雄市', '前鎮區中山二路5號'),
    ('林雅婷', 'lin.yating@example.com', '0956-789-012', '台南市', '東區中華東路三段269號'),
    ('黃志明', 'huang.zhiming@example.com', '0967-890-123', '桃園市', '中壢區中央西路二段141號'),
    ('吳建國', 'wu.jianguo@example.com', '0978-901-234', '新竹市', '東區光復路二段101號'),
    ('劉雅琪', 'liu.yaqi@example.com', '0989-012-345', '台北市', '大安區敦化南路二段105號'),
    ('鄭文彬', 'zheng.wenbin@example.com', '0911-234-567', '台中市', '北區三民路三段161號'),
    ('謝佳玲', 'xie.jialing@example.com', '0922-345-678', '高雄市', '左營區博愛二路777號');

    PRINT '✓ 客戶資料插入成功 (10 筆)';
END
ELSE
BEGIN
    PRINT '⚠ 客戶資料已存在，跳過插入';
END
GO

-- =============================================
-- 2. 插入類別資料
-- =============================================
IF NOT EXISTS (SELECT * FROM Categories)
BEGIN
    INSERT INTO Categories (Name, Description, DisplayOrder) VALUES
    ('電子產品', '各類電子產品', 1),
    ('電腦週邊', '電腦相關週邊設備', 2),
    ('辦公用品', '辦公室文具用品', 3),
    ('家電產品', '家用電器', 4),
    ('手機配件', '手機相關配件', 5);

    PRINT '✓ 類別資料插入成功 (5 筆)';
END
ELSE
BEGIN
    PRINT '⚠ 類別資料已存在，跳過插入';
END
GO

-- =============================================
-- 3. 插入產品資料
-- =============================================
IF NOT EXISTS (SELECT * FROM Products)
BEGIN
    INSERT INTO Products (Name, Description, Category, Price, Stock, MinStock, SKU) VALUES
    ('無線滑鼠', 'Logitech MX Master 3 無線滑鼠', '電腦週邊', 2990, 50, 10, 'MOUSE-001'),
    ('機械鍵盤', 'Corsair K95 RGB 機械式鍵盤', '電腦週邊', 5990, 30, 5, 'KB-001'),
    ('27吋顯示器', 'Dell UltraSharp U2720Q 27吋 4K 顯示器', '電子產品', 18900, 15, 3, 'MON-001'),
    ('筆記型電腦', 'ASUS ZenBook 14 筆記型電腦', '電子產品', 35900, 20, 5, 'LAPTOP-001'),
    ('USB-C Hub', '7合1 USB-C 多功能轉接器', '電腦週邊', 890, 100, 20, 'HUB-001'),
    ('辦公椅', 'Herman Miller Aeron 人體工學椅', '辦公用品', 38900, 10, 2, 'CHAIR-001'),
    ('站立式桌', '電動升降桌 120x60cm', '辦公用品', 12900, 8, 2, 'DESK-001'),
    ('無線耳機', 'Sony WH-1000XM5 降噪耳機', '電子產品', 9990, 25, 5, 'AUDIO-001'),
    ('網路攝影機', 'Logitech C920 HD Pro 網路攝影機', '電腦週邊', 2490, 40, 10, 'CAM-001'),
    ('智能手錶', 'Apple Watch Series 9', '電子產品', 13900, 30, 8, 'WATCH-001'),
    ('行動電源', '20000mAh 快充行動電源', '手機配件', 990, 80, 20, 'PWR-001'),
    ('手機支架', '桌面手機平板支架', '手機配件', 290, 150, 30, 'STAND-001'),
    ('藍牙喇叭', 'JBL Flip 6 防水藍牙喇叭', '電子產品', 3990, 35, 10, 'SPEAKER-001'),
    ('檯燈', 'BenQ WiT ScreenBar 螢幕智能掛燈', '辦公用品', 3590, 25, 5, 'LAMP-001'),
    ('滑鼠墊', '大型遊戲滑鼠墊 80x30cm', '電腦週邊', 390, 120, 30, 'PAD-001');

    PRINT '✓ 產品資料插入成功 (15 筆)';
END
ELSE
BEGIN
    PRINT '⚠ 產品資料已存在，跳過插入';
END
GO

-- =============================================
-- 4. 插入訂單資料
-- =============================================
IF NOT EXISTS (SELECT * FROM Orders)
BEGIN
    DECLARE @Today DATETIME2 = GETDATE();

    INSERT INTO Orders (OrderNumber, CustomerId, OrderDate, Status, TotalAmount, ShippingAddress) VALUES
    ('ORD-2024-001', 1, DATEADD(DAY, -10, @Today), 'Delivered', 8970, '台北市信義區信義路五段7號'),
    ('ORD-2024-002', 2, DATEADD(DAY, -8, @Today), 'Delivered', 35900, '新北市板橋區中山路一段152號'),
    ('ORD-2024-003', 3, DATEADD(DAY, -6, @Today), 'Shipped', 41890, '台中市西屯區台灣大道三段99號'),
    ('ORD-2024-004', 4, DATEADD(DAY, -5, @Today), 'Processing', 6381, '高雄市前鎮區中山二路5號'),
    ('ORD-2024-005', 1, DATEADD(DAY, -3, @Today), 'Processing', 13900, '台北市信義區信義路五段7號'),
    ('ORD-2024-006', 5, DATEADD(DAY, -2, @Today), 'Pending', 2840.5, '台南市東區中華東路三段269號'),
    ('ORD-2024-007', 6, DATEADD(DAY, -1, @Today), 'Pending', 18900, '桃園市中壢區中央西路二段141號'),
    ('ORD-2024-008', 7, @Today, 'Pending', 5990, '新竹市東區光復路二段101號');

    PRINT '✓ 訂單資料插入成功 (8 筆)';
END
ELSE
BEGIN
    PRINT '⚠ 訂單資料已存在，跳過插入';
END
GO

-- =============================================
-- 5. 插入訂單明細資料
-- =============================================
IF NOT EXISTS (SELECT * FROM OrderItems)
BEGIN
    -- ORD-2024-001 的明細
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount) VALUES
    (1, 1, '無線滑鼠', 3, 2990, 0);

    -- ORD-2024-002 的明細
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount) VALUES
    (2, 4, '筆記型電腦', 1, 35900, 0);

    -- ORD-2024-003 的明細
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount) VALUES
    (3, 4, '筆記型電腦', 1, 35900, 0),
    (3, 2, '機械鍵盤', 1, 5990, 0);

    -- ORD-2024-004 的明細
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount) VALUES
    (4, 9, '網路攝影機', 1, 2490, 0),
    (4, 8, '無線耳機', 1, 3990, 0.10);

    -- ORD-2024-005 的明細
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount) VALUES
    (5, 10, '智能手錶', 1, 13900, 0);

    -- ORD-2024-006 的明細
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount) VALUES
    (6, 1, '無線滑鼠', 1, 2990, 0.05);

    -- ORD-2024-007 的明細
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount) VALUES
    (7, 3, '27吋顯示器', 1, 18900, 0);

    -- ORD-2024-008 的明細
    INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount) VALUES
    (8, 2, '機械鍵盤', 1, 5990, 0);

    PRINT '✓ 訂單明細資料插入成功';
END
ELSE
BEGIN
    PRINT '⚠ 訂單明細資料已存在，跳過插入';
END
GO

-- =============================================
-- 6. 插入庫存異動記錄
-- =============================================
IF NOT EXISTS (SELECT * FROM InventoryTransactions)
BEGIN
    INSERT INTO InventoryTransactions (ProductId, TransactionType, Quantity, Reference, Notes) VALUES
    (1, 'Purchase', 100, 'PO-2024-001', '進貨'),
    (2, 'Purchase', 50, 'PO-2024-002', '進貨'),
    (1, 'Sale', -3, 'ORD-2024-001', '銷售'),
    (4, 'Sale', -2, 'ORD-2024-002', '銷售');

    PRINT '✓ 庫存異動記錄插入成功';
END
ELSE
BEGIN
    PRINT '⚠ 庫存異動記錄已存在，跳過插入';
END
GO

-- =============================================
-- 顯示插入結果統計
-- =============================================
PRINT '';
PRINT '========================================';
PRINT '資料插入完成！統計資訊：';
PRINT '========================================';

SELECT '客戶' AS [資料表], COUNT(*) AS [筆數] FROM Customers
UNION ALL
SELECT '類別', COUNT(*) FROM Categories
UNION ALL
SELECT '產品', COUNT(*) FROM Products
UNION ALL
SELECT '訂單', COUNT(*) FROM Orders
UNION ALL
SELECT '訂單明細', COUNT(*) FROM OrderItems
UNION ALL
SELECT '庫存異動', COUNT(*) FROM InventoryTransactions;

PRINT '';
PRINT '========================================';
PRINT '範例資料插入完成！';
PRINT '========================================';
GO
