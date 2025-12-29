# MS03 - Basic CRUD（基本增刪改查）

## 學習目標
- 掌握完整的 CRUD 操作
- 學習使用 Dapper 簡化資料存取
- 理解 INSERT、UPDATE、DELETE 的正確用法
- 了解資料驗證的重要性

## CRUD 操作

### Create (新增)
- 插入新資料到資料表
- 取得自動產生的 ID
- 驗證資料完整性

### Read (讀取)
- 查詢單筆或多筆資料
- 使用條件篩選
- 排序和分頁

### Update (更新)
- 修改現有資料
- 驗證資料存在性
- 處理並行更新

### Delete (刪除)
- 刪除資料
- 確認刪除操作
- 處理外鍵約束

## Dapper 方法對照

| 操作 | Dapper 方法 | 用途 |
|------|------------|------|
| SELECT (單筆) | QueryFirstOrDefault<T> | 查詢單一物件 |
| SELECT (多筆) | Query<T> | 查詢列表 |
| INSERT | Execute + SCOPE_IDENTITY() | 新增並返回 ID |
| UPDATE | Execute | 更新資料 |
| DELETE | Execute | 刪除資料 |
| COUNT | ExecuteScalar<int> | 計數 |

## 重要概念

### 1. SCOPE_IDENTITY()
取得最後插入的自動編號 ID。

### 2. 受影響的資料列數
Execute 方法返回受影響的資料列數，用於驗證操作成功。

### 3. 樂觀並行控制
使用時間戳記或版本號避免並行更新問題。

## 執行結果
- 完整的客戶管理功能
- 新增、編輯、刪除客戶
- 即時反映資料庫變更
