# MS02 - Parameterized Query（參數化查詢）

## 學習目標
- 理解 SQL Injection 攻擊的危險性
- 學習使用參數化查詢保護資料庫安全
- 掌握 SqlParameter 的使用方法
- 了解安全的資料查詢最佳實踐

## 核心概念

### 1. SQL Injection 攻擊
當直接將使用者輸入拼接到 SQL 語句中時，可能被惡意利用。

### 2. 參數化查詢
使用參數代替直接拼接，由資料庫引擎處理，確保安全。

### 3. SqlParameter
ADO.NET 提供的參數類別，支援型別安全。

## 為什麼需要參數化查詢？

### ❌ 危險的做法（永遠不要這樣做！）
```csharp
// SQL Injection 風險！
string username = txtUsername.Text;
string sql = $"SELECT * FROM Users WHERE Username = '{username}'";
// 如果使用者輸入: admin' OR '1'='1
// 實際執行: SELECT * FROM Users WHERE Username = 'admin' OR '1'='1'
// 結果: 返回所有使用者！
```

### ✅ 安全的做法（參數化查詢）
```csharp
string sql = "SELECT * FROM Users WHERE Username = @Username";
var parameters = new { Username = username };
// SQL Server 會正確處理參數，防止注入攻擊
```

## 使用方式

### 方法 1: 使用 Dapper（推薦）
```csharp
var result = connection.Query<Customer>(
    "SELECT * FROM Customers WHERE City = @City",
    new { City = city }
);
```

### 方法 2: 使用 ADO.NET
```csharp
var command = new SqlCommand("SELECT * FROM Customers WHERE City = @City", connection);
command.Parameters.AddWithValue("@City", city);
```

## 重要提示

### ⚠️ 永遠使用參數化
即使是簡單的查詢，也要使用參數化，養成良好習慣。

### ⚠️ 不要信任使用者輸入
所有來自使用者的輸入都要當作不可信任的資料處理。

### ⚠️ 型別安全
使用 SqlParameter 時指定正確的資料型別。

## 執行結果
- 可以根據不同條件安全地查詢客戶
- 展示參數化查詢的正確用法
- 比較不同查詢方式

## 下一步
學習 MS03 - 基本 CRUD 操作，了解完整的資料庫操作。
