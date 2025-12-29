# MSSQL 資料庫連線範例（初階）

## 概述
本系列包含 5 個由淺入深的 MSSQL 資料庫連線範例，使用 Dapper 和 ADO.NET。
適合初學者學習如何在 WPF MVVM 應用程式中操作 MSSQL 資料庫。

## 前置需求

### 1. MSSQL Server
- SQL Server 2019 或更新版本
- SQL Server Express（免費版本）即可
- 或使用 LocalDB

### 2. NuGet 套件
```bash
dotnet add package Dapper
dotnet add package Microsoft.Data.SqlClient
```

### 3. 連接字串範例
```csharp
// SQL Server（Windows 驗證）
"Server=localhost;Database=WpfMvvmDemo;Trusted_Connection=True;TrustServerCertificate=True;"

// SQL Server（帳號密碼驗證）
"Server=localhost;Database=WpfMvvmDemo;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"

// LocalDB
"Server=(localdb)\\mssqllocaldb;Database=WpfMvvmDemo;Trusted_Connection=True;"
```

## 範例列表

### MS01 - Basic Connection（基本連線與查詢）
- 建立資料庫連線
- 執行簡單查詢
- 顯示查詢結果
- 理解 Connection、Command、DataReader

**學習重點**：
- SqlConnection 的使用
- 基本 SELECT 查詢
- using 語句管理資源

### MS02 - Parameterized Query（參數化查詢）
- 使用參數化查詢
- 防止 SQL Injection 攻擊
- 安全的資料查詢方式

**學習重點**：
- SqlParameter 的使用
- 為什麼要使用參數化查詢
- 安全性最佳實踐

### MS03 - Basic CRUD（基本增刪改查）
- 完整的 CRUD 操作
- INSERT、UPDATE、DELETE 語句
- 使用 Dapper 簡化程式碼

**學習重點**：
- Dapper 的基本使用
- ExecuteScalar、Execute 方法
- 資料模型映射

### MS04 - Transaction（事務處理）
- 理解資料庫事務
- Commit 和 Rollback
- 確保資料一致性

**學習重點**：
- TransactionScope 的使用
- ACID 特性
- 錯誤處理與回滾

### MS05 - Stored Procedure（預存程序）
- 建立和呼叫預存程序
- 傳遞參數
- 接收輸出參數和返回值

**學習重點**：
- 預存程序的優勢
- 輸入/輸出參數
- 返回值處理

## 學習路徑

```
MS01 (基本連線)
    ↓
MS02 (參數化查詢)
    ↓
MS03 (CRUD 操作)
    ↓
MS04 (事務處理)
    ↓
MS05 (預存程序)
```

## 資料庫設定

### 方法 1: 使用 SQL Server Management Studio (SSMS)
1. 開啟 SSMS
2. 執行 `create_database.sql`
3. 執行 `create_tables.sql`
4. 執行 `seed_data.sql`

### 方法 2: 在程式碼中初始化
範例中包含自動建立資料庫的程式碼。

## 安全性注意事項

### ❌ 不安全的做法
```csharp
// 永遠不要這樣做！
string sql = $"SELECT * FROM Users WHERE Username = '{username}'";
```

### ✅ 安全的做法
```csharp
// 使用參數化查詢
string sql = "SELECT * FROM Users WHERE Username = @Username";
var result = connection.Query<User>(sql, new { Username = username });
```

## 連線管理最佳實踐

### 1. 使用 using 語句
```csharp
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    // 執行查詢
}
// 連線自動關閉和釋放
```

### 2. 連線字串管理
- 不要硬編碼在程式碼中
- 使用 `appsettings.json` 或 `app.config`
- 敏感資訊使用加密

### 3. 連線池
- SQL Server 自動管理連線池
- 確保正確關閉連線以回收資源
- 避免連線洩漏

## 錯誤處理

```csharp
try
{
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        // 資料庫操作
    }
}
catch (SqlException ex)
{
    // 處理 SQL 錯誤
    MessageBox.Show($"資料庫錯誤: {ex.Message}");
}
catch (Exception ex)
{
    // 處理其他錯誤
    MessageBox.Show($"發生錯誤: {ex.Message}");
}
```

## Dapper vs ADO.NET

### ADO.NET（原生）
- 更細緻的控制
- 程式碼較多
- 適合複雜場景

### Dapper（微 ORM）
- 程式碼簡潔
- 效能優異
- 適合大多數場景

## 效能建議

1. **使用 async/await** - 避免阻塞 UI
2. **批次操作** - 減少往返次數
3. **適當的索引** - 提升查詢速度
4. **避免 N+1 查詢** - 使用 JOIN 或批次查詢
5. **連線池設定** - 依需求調整

## 偵錯技巧

### 1. 查看 SQL 語句
```csharp
Console.WriteLine($"SQL: {sql}");
Console.WriteLine($"Parameters: {JsonSerializer.Serialize(parameters)}");
```

### 2. 使用 SQL Profiler
- 監控實際執行的 SQL
- 查看執行時間
- 找出效能瓶頸

### 3. 例外處理
- 記錄詳細的錯誤訊息
- 包含 SQL 語句和參數
- 使用日誌框架

## 常見問題

### Q1: 無法連線到資料庫
**A**: 檢查：
- SQL Server 服務是否執行
- 防火牆設定
- 連接字串是否正確
- TCP/IP 協定是否啟用

### Q2: Login failed for user 'xxx'
**A**: 檢查：
- 使用者帳號密碼
- SQL Server 驗證模式
- 使用者權限

### Q3: 資料庫不存在
**A**: 先執行 `create_database.sql` 建立資料庫

### Q4: 連線逾時
**A**:
- 增加連線逾時時間
- 檢查網路連線
- 檢查資料庫負載

## 下一步

完成這 5 個範例後，您可以：
1. 學習進階的 Repository Pattern（範例 16）
2. 整合依賴注入（範例 18）
3. 實作完整的應用程式（範例 20）

## 資源連結

- [Dapper 官方文件](https://github.com/DapperLib/Dapper)
- [SQL Server 文件](https://docs.microsoft.com/sql/)
- [MSSQL 最佳實踐](https://docs.microsoft.com/sql/relational-databases/best-practices-database-engine)
