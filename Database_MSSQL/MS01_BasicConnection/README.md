# MS01 - Basic Connection（基本連線與查詢）

## 學習目標
- 理解如何連線到 MSSQL 資料庫
- 學習執行基本 SELECT 查詢
- 掌握 SqlConnection 的使用
- 理解資源管理的重要性

## 核心概念

### 1. SqlConnection
負責建立與資料庫的連線。

### 2. SqlCommand
用於執行 SQL 語句。

### 3. SqlDataReader
讀取查詢結果。

### 4. using 語句
自動釋放資源，確保連線正確關閉。

## 專案結構
```
MS01_BasicConnection/
├── Models/
│   └── Customer.cs
├── ViewModels/
│   ├── ViewModelBase.cs
│   └── MainViewModel.cs
├── Views/
│   └── MainWindow.xaml
└── Data/
    └── DatabaseHelper.cs
```

## 連接字串說明

### Windows 驗證（推薦用於開發）
```csharp
"Server=localhost;Database=WpfMvvmDemo;Trusted_Connection=True;TrustServerCertificate=True;"
```

### SQL Server 驗證
```csharp
"Server=localhost;Database=WpfMvvmDemo;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
```

### LocalDB（最簡單）
```csharp
"Server=(localdb)\\mssqllocaldb;Database=WpfMvvmDemo;Trusted_Connection=True;"
```

## 重要提示

### ⚠️ 資源管理
務必使用 `using` 語句確保連線被正確釋放：

```csharp
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    // 執行查詢
} // 連線自動關閉
```

### ⚠️ 錯誤處理
適當處理 SQL 例外：

```csharp
try
{
    // 資料庫操作
}
catch (SqlException ex)
{
    // 處理 SQL 錯誤
}
```

## 執行結果
- 顯示資料庫中的客戶列表
- 可以測試連線狀態
- 查看查詢結果

## 下一步
學習 MS02 - 參數化查詢，了解如何安全地傳遞參數。
