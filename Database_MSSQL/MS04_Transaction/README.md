# MS04 - Transaction（事務處理）

## 學習目標
- 理解資料庫事務的概念
- 學習 ACID 特性
- 掌握 Commit 和 Rollback
- 了解事務的使用場景

## ACID 特性

### Atomicity（原子性）
事務中的所有操作要麼全部成功，要麼全部失敗。

### Consistency（一致性）
事務執行前後，資料庫都處於一致的狀態。

### Isolation（隔離性）
並行事務之間互不干擾。

### Durability（持久性）
事務提交後，變更永久保存。

## 為什麼需要事務？

### 場景 1: 轉帳操作
```
1. 從 A 帳戶扣款 1000 元
2. 向 B 帳戶存款 1000 元
```
兩個操作必須同時成功或同時失敗，否則會造成資料不一致。

### 場景 2: 訂單建立
```
1. 插入訂單主檔
2. 插入訂單明細
3. 更新庫存數量
```
任何一步失敗，都應該回滾所有變更。

## 使用方式

### 方法 1: TransactionScope（推薦）
```csharp
using (var scope = new TransactionScope())
{
    // 執行多個資料庫操作
    repository.AddOrder(order);
    repository.UpdateStock(productId, quantity);

    // 提交事務
    scope.Complete();
}
```

### 方法 2: SqlTransaction
```csharp
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    using (var transaction = connection.BeginTransaction())
    {
        try
        {
            // 執行操作
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
```

## 重要提示

### ⚠️ 事務範圍要小
避免在事務中執行長時間操作，影響並行性能。

### ⚠️ 錯誤處理
務必處理例外並回滾事務。

### ⚠️ 鎖定問題
注意事務隔離層級，避免死鎖。
