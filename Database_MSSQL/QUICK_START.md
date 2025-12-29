# MSSQL 快速入門指南

## 🚀 5 分鐘快速開始

### 步驟 1: 安裝 SQL Server

#### 選項 A: SQL Server Express（推薦新手）
1. 下載 [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
2. 執行安裝程式，選擇「基本」安裝
3. 記下伺服器名稱（通常是 `localhost` 或 `.\SQLEXPRESS`）

#### 選項 B: LocalDB（最簡單）
- 隨 Visual Studio 一起安裝
- 連接字串: `Server=(localdb)\mssqllocaldb;...`

#### 選項 C: SQL Server Developer Edition（完整功能）
- 免費，但僅供開發和測試使用
- 包含所有企業版功能

### 步驟 2: 安裝管理工具（可選）

#### SQL Server Management Studio (SSMS)
- 下載: https://aka.ms/ssmsfullsetup
- 強大的圖形化管理工具
- 執行 SQL 腳本、查看資料、效能調整

#### Azure Data Studio
- 跨平台、現代化的資料庫工具
- 適合 macOS、Linux 使用者

### 步驟 3: 建立資料庫

#### 方法 1: 使用 SSMS
1. 開啟 SSMS，連線到伺服器
2. 開啟 `Scripts/01_create_database.sql`
3. 點擊「執行」或按 F5

#### 方法 2: 使用命令列
```bash
sqlcmd -S localhost -i Scripts\01_create_database.sql
sqlcmd -S localhost -i Scripts\02_create_tables.sql
sqlcmd -S localhost -i Scripts\03_seed_data.sql
```

#### 方法 3: 在程式碼中自動建立
範例程式碼會自動檢查並建立資料庫和資料表。

### 步驟 4: 修改連接字串

在範例程式碼中找到連接字串並修改：

#### Windows 驗證（推薦）
```csharp
string connectionString = "Server=localhost;Database=WpfMvvmDemo;Trusted_Connection=True;TrustServerCertificate=True;";
```

#### SQL Server 驗證
```csharp
string connectionString = "Server=localhost;Database=WpfMvvmDemo;User Id=sa;Password=YourPassword;TrustServerCertificate=True;";
```

#### LocalDB
```csharp
string connectionString = "Server=(localdb)\\mssqllocaldb;Database=WpfMvvmDemo;Trusted_Connection=True;";
```

### 步驟 5: 安裝 NuGet 套件

```bash
dotnet add package Dapper
dotnet add package Microsoft.Data.SqlClient
```

### 步驟 6: 執行範例

依序學習 5 個範例：
1. **MS01_BasicConnection** - 基本連線
2. **MS02_ParameterizedQuery** - 參數化查詢
3. **MS03_BasicCRUD** - CRUD 操作
4. **MS04_Transaction** - 事務處理
5. **MS05_StoredProcedure** - 預存程序

## 常見問題排除

### ❌ 無法連線到 SQL Server

**錯誤訊息**: "A network-related or instance-specific error occurred..."

**解決方法**:
1. 確認 SQL Server 服務已啟動
   - 按 Win+R，輸入 `services.msc`
   - 找到 `SQL Server (MSSQLSERVER)` 或 `SQL Server (SQLEXPRESS)`
   - 確認狀態為「執行中」

2. 啟用 TCP/IP 協定
   - 開啟「SQL Server Configuration Manager」
   - SQL Server 網路組態 → 協定
   - 啟用 TCP/IP

3. 檢查防火牆設定
   - 允許 SQL Server 通過防火牆
   - 預設埠號: 1433

### ❌ Login failed for user 'sa'

**解決方法**:
1. 確認 SQL Server 驗證模式
   - SSMS → 伺服器屬性 → 安全性
   - 選擇「SQL Server 及 Windows 驗證模式」
   - 重新啟動 SQL Server 服務

2. 重設 sa 密碼
   ```sql
   ALTER LOGIN sa WITH PASSWORD = 'YourNewPassword';
   ALTER LOGIN sa ENABLE;
   ```

### ❌ 資料庫不存在

**解決方法**:
執行 `Scripts/01_create_database.sql` 建立資料庫。

### ❌ Login failed: User not associated with trusted connection

**解決方法**:
改用 Windows 驗證，或設定 SQL Server 驗證。

## 連接字串參數說明

| 參數 | 說明 | 範例 |
|------|------|------|
| Server | 伺服器位址 | localhost, .\SQLEXPRESS, (localdb)\mssqllocaldb |
| Database | 資料庫名稱 | WpfMvvmDemo |
| Trusted_Connection | 使用 Windows 驗證 | True |
| User Id | SQL Server 帳號 | sa |
| Password | SQL Server 密碼 | YourPassword |
| TrustServerCertificate | 信任伺服器憑證 | True |
| Connection Timeout | 連線逾時（秒） | 30 |
| Encrypt | 加密連線 | False |

## 學習路徑

### 第 1 天: 環境設定
- ✅ 安裝 SQL Server
- ✅ 安裝 SSMS
- ✅ 建立資料庫
- ✅ 執行範例腳本

### 第 2 天: 基本連線
- ✅ 學習 MS01_BasicConnection
- ✅ 理解 SqlConnection、SqlCommand、SqlDataReader
- ✅ 掌握 using 語句的重要性

### 第 3 天: 安全查詢
- ✅ 學習 MS02_ParameterizedQuery
- ✅ 理解 SQL Injection 攻擊
- ✅ 掌握參數化查詢

### 第 4 天: CRUD 操作
- ✅ 學習 MS03_BasicCRUD
- ✅ 掌握 INSERT、UPDATE、DELETE
- ✅ 使用 Dapper 簡化程式碼

### 第 5 天: 事務處理
- ✅ 學習 MS04_Transaction
- ✅ 理解 ACID 特性
- ✅ 掌握 TransactionScope

### 第 6 天: 預存程序
- ✅ 學習 MS05_StoredProcedure
- ✅ 建立和呼叫預存程序
- ✅ 處理輸入/輸出參數

## 實用工具

### 查詢資料庫資訊
```sql
-- 查看所有資料表
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';

-- 查看資料表欄位
SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Customers';

-- 查看資料庫大小
EXEC sp_spaceused;
```

### 備份資料庫
```sql
BACKUP DATABASE WpfMvvmDemo TO DISK = 'C:\Backup\WpfMvvmDemo.bak';
```

### 還原資料庫
```sql
RESTORE DATABASE WpfMvvmDemo FROM DISK = 'C:\Backup\WpfMvvmDemo.bak';
```

## 效能最佳化建議

### 1. 使用索引
```sql
CREATE INDEX IX_Customers_Email ON Customers(Email);
```

### 2. 避免 SELECT *
```sql
-- ❌ 不好
SELECT * FROM Customers;

-- ✅ 好
SELECT Id, Name, Email FROM Customers;
```

### 3. 使用 WHERE 子句限制結果
```sql
SELECT * FROM Customers WHERE City = '台北市';
```

### 4. 批次操作
使用 Dapper 批次插入：
```csharp
connection.Execute(sql, customers); // 一次插入多筆
```

## 下一步

完成 5 個範例後，您可以：
1. 學習進階 Dapper 功能（範例 16）
2. 整合依賴注入（範例 18）
3. 建立完整應用程式（範例 20）
4. 學習 Entity Framework Core

## 資源連結

- [SQL Server 官方文件](https://docs.microsoft.com/sql/)
- [Dapper GitHub](https://github.com/DapperLib/Dapper)
- [SQL Server 下載](https://www.microsoft.com/sql-server/sql-server-downloads)
- [SSMS 下載](https://aka.ms/ssmsfullsetup)
- [SQL 教學](https://www.w3schools.com/sql/)

---

準備好了嗎？從 **MS01_BasicConnection** 開始你的 MSSQL 學習之旅！🎉
