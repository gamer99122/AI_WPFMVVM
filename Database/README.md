# 資料庫腳本說明

## 概述
本目錄包含用於 Dapper 範例的資料庫架構和範例資料腳本。

## 資料庫選擇
範例使用 **SQLite**，因為：
- 輕量級，無需安裝資料庫伺服器
- 跨平台支援
- 適合學習和開發
- 易於分發和備份

## 檔案說明

### 1. create_schema.sql
建立資料庫架構（資料表、索引、外鍵等）

### 2. seed_data.sql
插入範例資料，用於測試和展示

### 3. queries.sql
常用 SQL 查詢範例

## 使用方式

### 在程式碼中初始化
大多數範例會在程式碼中自動建立資料庫：

```csharp
var dbContext = new AppDbContext("Data Source=app.db");
dbContext.Initialize();
```

### 手動執行 SQL
如果需要手動執行 SQL 腳本，可以使用：

#### 方法 1: 使用 SQLite CLI
```bash
sqlite3 app.db < create_schema.sql
sqlite3 app.db < seed_data.sql
```

#### 方法 2: 使用 DB Browser for SQLite
1. 下載並安裝 [DB Browser for SQLite](https://sqlitebrowser.org/)
2. 開啟資料庫檔案
3. 執行 SQL 腳本

#### 方法 3: 使用 Visual Studio
- 安裝 SQLite/SQL Server Compact Toolbox 擴充功能
- 使用內建的 SQL 編輯器執行腳本

## 資料表架構

### Customers (客戶)
| 欄位 | 型別 | 說明 |
|------|------|------|
| Id | INTEGER | 主鍵，自動遞增 |
| Name | TEXT | 客戶姓名 |
| Email | TEXT | 電子郵件 |
| Phone | TEXT | 電話號碼 |
| Address | TEXT | 地址 |
| CreatedAt | TEXT | 建立時間 |
| UpdatedAt | TEXT | 更新時間 |

### Products (產品)
| 欄位 | 型別 | 說明 |
|------|------|------|
| Id | INTEGER | 主鍵，自動遞增 |
| Name | TEXT | 產品名稱 |
| Description | TEXT | 產品描述 |
| Category | TEXT | 產品類別 |
| Price | REAL | 價格 |
| Stock | INTEGER | 庫存數量 |
| CreatedAt | TEXT | 建立時間 |

### Orders (訂單)
| 欄位 | 型別 | 說明 |
|------|------|------|
| Id | INTEGER | 主鍵，自動遞增 |
| CustomerId | INTEGER | 客戶 ID（外鍵） |
| OrderDate | TEXT | 訂單日期 |
| TotalAmount | REAL | 總金額 |
| Status | TEXT | 訂單狀態 |

## 連接字串

### SQLite
```csharp
"Data Source=app.db"
```

### 記憶體資料庫（測試用）
```csharp
"Data Source=:memory:"
```

### 完整連接字串選項
```csharp
"Data Source=app.db;Cache=Shared;Mode=ReadWriteCreate"
```

## 注意事項

1. **日期時間處理**
   - SQLite 沒有專門的 DateTime 型別
   - 使用 TEXT 儲存 ISO 8601 格式
   - 範例: `"2024-01-15T10:30:00"`

2. **外鍵約束**
   - SQLite 預設不啟用外鍵約束
   - 需要在連線時執行: `PRAGMA foreign_keys = ON;`

3. **自動遞增**
   - 使用 `AUTOINCREMENT` 關鍵字
   - 或使用 `INTEGER PRIMARY KEY` 自動遞增

4. **資料型別**
   - INTEGER: 整數
   - REAL: 浮點數
   - TEXT: 字串
   - BLOB: 二進位資料

## 從其他資料庫遷移

### SQL Server
如果要改用 SQL Server：
```bash
dotnet add package Microsoft.Data.SqlClient
```

連接字串：
```csharp
"Server=localhost;Database=MyApp;User Id=sa;Password=yourPassword;"
```

### PostgreSQL
如果要改用 PostgreSQL：
```bash
dotnet add package Npgsql
```

連接字串：
```csharp
"Host=localhost;Database=myapp;Username=postgres;Password=yourPassword"
```

## 效能優化建議

1. **建立索引**
```sql
CREATE INDEX idx_customers_email ON Customers(Email);
CREATE INDEX idx_orders_customer ON Orders(CustomerId);
```

2. **使用事務**
```csharp
using (var transaction = connection.BeginTransaction())
{
    // 執行多個操作
    transaction.Commit();
}
```

3. **參數化查詢**
```csharp
// 使用 Dapper 自動參數化
var result = connection.Query<Customer>(
    "SELECT * FROM Customers WHERE Email = @Email",
    new { Email = email }
);
```
