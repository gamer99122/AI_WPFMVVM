-- =============================================
-- 建立資料庫
-- =============================================

-- 檢查資料庫是否存在，如果存在則刪除（謹慎使用！）
-- 在生產環境中不要使用 DROP DATABASE
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'WpfMvvmDemo')
BEGIN
    ALTER DATABASE WpfMvvmDemo SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE WpfMvvmDemo;
END
GO

-- 建立新資料庫
CREATE DATABASE WpfMvvmDemo;
GO

-- 切換到新資料庫
USE WpfMvvmDemo;
GO

-- 設定資料庫選項
ALTER DATABASE WpfMvvmDemo SET RECOVERY SIMPLE;
GO

PRINT '資料庫 WpfMvvmDemo 建立成功！';
GO
