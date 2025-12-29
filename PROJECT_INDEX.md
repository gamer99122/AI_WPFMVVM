# 專案範例索引

快速查找你需要的範例！

## 按主題分類

### 資料繫結 (Data Binding)
- `02_DataBinding` - 基本資料繫結與模式
- `03_INotifyPropertyChanged` - 屬性變更通知
- `06_TwoWayBinding` - 雙向繫結進階
- `05_ListBinding` - 列表與集合繫結
- `08_CollectionView` - 集合視圖操作

### 命令模式 (Commands)
- `04_RelayCommand` - 基本命令實作
- `17_AsyncMVVM` - 非同步命令

### UI 元件
- `07_Converter` - 值轉換器
- `09_UserControl` - 自訂控制項
- `10_DependencyProperty` - 相依性屬性

### 導航與對話框
- `11_Navigation` - 頁面導航
- `13_DialogService` - 對話框服務

### 資料驗證
- `12_Validation` - 完整資料驗證

### 資料庫操作
- `14_SimpleCRUD` - 基本 CRUD（無資料庫）
- `16_DapperIntegration` - Dapper + SQLite 整合
- `20_CompleteApp` - 完整資料庫應用

### 進階模式
- `15_ObservableCollection` - 集合進階應用
- `17_AsyncMVVM` - 非同步操作
- `18_DependencyInjection` - 依賴注入
- `19_EventAggregator` - 事件聚合器

## 按難度分類

### ⭐ 初學者必看
1. `01_HelloWorldMVVM` - MVVM 入門
2. `02_DataBinding` - 資料繫結基礎
3. `03_INotifyPropertyChanged` - 核心概念
4. `04_RelayCommand` - 命令模式

### ⭐⭐ 進階學習
5. `05_ListBinding` - 列表操作
6. `12_Validation` - 資料驗證
7. `13_DialogService` - 服務模式
8. `14_SimpleCRUD` - CRUD 操作

### ⭐⭐⭐ 專業開發
9. `16_DapperIntegration` - 資料庫整合
10. `17_AsyncMVVM` - 非同步模式
11. `18_DependencyInjection` - DI 模式
12. `20_CompleteApp` - 完整應用

## 按功能查找

### 我想學習...

#### 如何更新 UI？
→ `03_INotifyPropertyChanged`

#### 如何處理按鈕點擊？
→ `04_RelayCommand`

#### 如何顯示列表資料？
→ `05_ListBinding`

#### 如何驗證使用者輸入？
→ `12_Validation`

#### 如何在頁面間導航？
→ `11_Navigation`

#### 如何操作資料庫？
→ `16_DapperIntegration`

#### 如何處理長時間操作？
→ `17_AsyncMVVM`

#### 如何組織大型專案？
→ `18_DependencyInjection`
→ `20_CompleteApp`

#### 如何在 ViewModel 間通訊？
→ `19_EventAggregator`

## 核心類別參考

### ViewModelBase
📁 位置: `Basic/03_INotifyPropertyChanged/ViewModels/ViewModelBase.cs`

所有 ViewModel 的基底類別，提供 `INotifyPropertyChanged` 實作。

```csharp
public abstract class ViewModelBase : INotifyPropertyChanged
{
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null);
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null);
}
```

### RelayCommand
📁 位置: `Basic/04_RelayCommand/Commands/RelayCommand.cs`

通用命令實作，支援 `Execute` 和 `CanExecute`。

```csharp
public class RelayCommand : ICommand
{
    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null);
}
```

### AsyncRelayCommand
📁 位置: `Advanced/17_AsyncMVVM/Commands/AsyncRelayCommand.cs`

支援非同步操作的命令。

```csharp
public class AsyncRelayCommand : ICommand
{
    public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null);
}
```

### DialogService
📁 位置: `Intermediate/13_DialogService/Services/DialogService.cs`

對話框服務，封裝 MessageBox 和檔案對話框。

```csharp
public interface IDialogService
{
    void ShowMessage(string message, string title);
    bool ShowConfirmation(string message, string title);
    string ShowOpenFileDialog(string filter);
}
```

### NavigationService
📁 位置: `Intermediate/11_Navigation/Services/NavigationService.cs`

頁面導航服務。

```csharp
public interface INavigationService
{
    void NavigateTo(string pageKey);
    void GoBack();
}
```

### EventAggregator
📁 位置: `Advanced/19_EventAggregator/Services/EventAggregator.cs`

事件聚合器，實作發布-訂閱模式。

```csharp
public interface IEventAggregator
{
    void Subscribe<TMessage>(Action<TMessage> action);
    void Publish<TMessage>(TMessage message);
}
```

### Repository Pattern
📁 位置: `Advanced/16_DapperIntegration/Data/Repositories/`

資料存取層抽象。

```csharp
public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> GetByIdAsync(int id);
    Task<int> AddAsync(Customer customer);
    Task<bool> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(int id);
}
```

## 資料庫相關

### 架構腳本
📁 `Database/create_schema.sql` - 建立資料表

### 範例資料
📁 `Database/seed_data.sql` - 插入測試資料

### 查詢範例
📁 `Database/queries.sql` - 常用 SQL 查詢

## 使用場景對照表

| 需求 | 推薦範例 | 難度 |
|------|---------|------|
| 建立第一個 MVVM 應用 | 01_HelloWorldMVVM | ⭐ |
| 顯示和編輯資料 | 03_INotifyPropertyChanged | ⭐ |
| 處理使用者操作 | 04_RelayCommand | ⭐ |
| 顯示資料列表 | 05_ListBinding | ⭐ |
| 格式化顯示資料 | 07_Converter | ⭐⭐ |
| 表單驗證 | 12_Validation | ⭐⭐ |
| 多頁面應用 | 11_Navigation | ⭐⭐ |
| 資料庫 CRUD | 16_DapperIntegration | ⭐⭐⭐ |
| 非同步操作 | 17_AsyncMVVM | ⭐⭐⭐ |
| 依賴注入 | 18_DependencyInjection | ⭐⭐⭐ |
| 模組間通訊 | 19_EventAggregator | ⭐⭐⭐ |
| 企業級應用 | 20_CompleteApp | ⭐⭐⭐ |

## NuGet 套件參考

### 基本範例不需要額外套件

### 進階範例需要的套件

#### Dapper 相關 (範例 16, 20)
```bash
dotnet add package Dapper
dotnet add package Microsoft.Data.Sqlite
```

#### 依賴注入 (範例 18, 20)
```bash
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Hosting
```

## 學習檢查清單

### 基礎階段 ✅
- [ ] 理解 MVVM 模式的三個組件
- [ ] 掌握 INotifyPropertyChanged
- [ ] 能夠實作 RelayCommand
- [ ] 熟悉資料繫結語法
- [ ] 了解 ObservableCollection

### 中階階段 ✅
- [ ] 實作完整的 CRUD 功能
- [ ] 能夠進行資料驗證
- [ ] 掌握頁面導航
- [ ] 使用服務模式解耦
- [ ] 理解 ValueConverter

### 進階階段 ✅
- [ ] 整合 Dapper 操作資料庫
- [ ] 實作非同步操作
- [ ] 使用依賴注入
- [ ] 實作事件聚合器
- [ ] 能夠組織大型專案

## 快速參考

### 建立新專案
```bash
dotnet new wpf -n MyApp -f net8.0
```

### 執行專案
```bash
dotnet run
```

### 發佈專案
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## 相關文件

- 📖 [README.md](README.md) - 專案總覽
- 🚀 [GETTING_STARTED.md](GETTING_STARTED.md) - 快速入門
- 💾 [Database/README.md](Database/README.md) - 資料庫說明

---

找到你需要的範例了嗎？開始學習吧！💪
