# MS05 - Stored Procedure（預存程序）

## 學習目標
- 理解預存程序的概念和優勢
- 學習建立和呼叫預存程序
- 掌握參數傳遞（輸入/輸出）
- 了解返回值的處理

## 什麼是預存程序？

預存程序（Stored Procedure）是預先編譯並儲存在資料庫中的 SQL 程式碼集合。

## 預存程序的優勢

### 1. 效能提升
- 預先編譯，執行更快
- 減少網路流量

### 2. 安全性
- 控制資料存取權限
- 防止 SQL Injection

### 3. 可維護性
- 集中管理業務邏輯
- 易於修改和版本控制

### 4. 重用性
- 多個應用程式共用
- 減少程式碼重複

## 參數類型

### 輸入參數（Input）
```sql
CREATE PROCEDURE GetCustomerById
    @CustomerId INT
AS
BEGIN
    SELECT * FROM Customers WHERE Id = @CustomerId;
END
```

### 輸出參數（Output）
```sql
CREATE PROCEDURE AddCustomer
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @NewId INT OUTPUT
AS
BEGIN
    INSERT INTO Customers (Name, Email)
    VALUES (@Name, @Email);

    SET @NewId = SCOPE_IDENTITY();
END
```

### 返回值（Return）
```sql
CREATE PROCEDURE CheckEmailExists
    @Email NVARCHAR(100)
AS
BEGIN
    IF EXISTS(SELECT 1 FROM Customers WHERE Email = @Email)
        RETURN 1;
    ELSE
        RETURN 0;
END
```

## 使用 Dapper 呼叫預存程序

### 基本呼叫
```csharp
var customer = connection.QueryFirstOrDefault<Customer>(
    "GetCustomerById",
    new { CustomerId = 1 },
    commandType: CommandType.StoredProcedure
);
```

### 輸出參數
```csharp
var parameters = new DynamicParameters();
parameters.Add("@Name", "王小明");
parameters.Add("@Email", "wang@example.com");
parameters.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

connection.Execute("AddCustomer", parameters, commandType: CommandType.StoredProcedure);

int newId = parameters.Get<int>("@NewId");
```

### 返回值
```csharp
var parameters = new DynamicParameters();
parameters.Add("@Email", "test@example.com");
parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

connection.Execute("CheckEmailExists", parameters, commandType: CommandType.StoredProcedure);

int returnValue = parameters.Get<int>("@ReturnValue");
```

## 最佳實踐

### ✅ 建議使用
- 複雜的業務邏輯
- 需要高效能的查詢
- 需要事務處理的操作
- 批次資料處理

### ❌ 避免使用
- 簡單的 CRUD 操作
- 頻繁變動的查詢
- 需要動態 SQL 的場景

## 執行結果
- 展示各種預存程序的呼叫方式
- 處理輸入/輸出參數
- 取得返回值
