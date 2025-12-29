# WPF MVVM 快速入門指南

## 歡迎！

恭喜你開始學習 WPF MVVM！本指南將幫助你快速上手。

## 學習路徑

### 第一階段：基礎知識（第 1-10 天）

從基本範例開始，建議依照順序學習：

1. **Day 1-2**: `01_HelloWorldMVVM` - 理解 MVVM 基本架構
2. **Day 3**: `02_DataBinding` - 學習資料繫結
3. **Day 4-5**: `03_INotifyPropertyChanged` - 掌握屬性通知（最重要！）
4. **Day 6-7**: `04_RelayCommand` - 學習命令模式
5. **Day 8**: `05_ListBinding` - 列表資料繫結
6. **Day 9**: `06_TwoWayBinding` - 雙向繫結深入
7. **Day 10**: `07_Converter` - 值轉換器

剩餘的基本範例（08-10）可以根據需求學習。

### 第二階段：進階應用（第 11-20 天）

中階範例整合多個概念：

11. **Navigation** - 頁面導航
12. **Validation** - 資料驗證（重要！）
13. **DialogService** - 對話框服務
14. **SimpleCRUD** - 完整的增刪改查
15. **ObservableCollection** - 集合操作

### 第三階段：專業開發（第 21-30 天）

進階範例涵蓋企業級開發：

16. **DapperIntegration** - 資料庫整合（必學！）
17. **AsyncMVVM** - 非同步操作
18. **DependencyInjection** - 依賴注入
19. **EventAggregator** - 事件聚合器
20. **CompleteApp** - 完整應用程式

## 建立你的第一個專案

### 步驟 1: 安裝工具

確保你已安裝：
- Visual Studio 2022 或更新版本
- .NET 8 SDK

### 步驟 2: 建立專案

```bash
# 使用 .NET CLI
dotnet new wpf -n MyFirstMVVMApp -f net8.0
cd MyFirstMVVMApp
```

或在 Visual Studio 中：
1. 檔案 → 新增 → 專案
2. 選擇「WPF 應用程式」
3. 選擇 .NET 8.0

### 步驟 3: 建立 MVVM 結構

建立以下資料夾：
```
MyFirstMVVMApp/
├── Models/
├── ViewModels/
├── Views/
├── Commands/
└── Services/
```

### 步驟 4: 複製基礎類別

從範例中複製這些必要類別：

1. **ViewModelBase.cs** (從 `03_INotifyPropertyChanged`)
   - 放在 `ViewModels/` 資料夾

2. **RelayCommand.cs** (從 `04_RelayCommand`)
   - 放在 `Commands/` 資料夾

### 步驟 5: 開始開發

參考 `01_HelloWorldMVVM` 範例開始你的第一個 ViewModel！

## 使用 Dapper 的專案

如果你要使用資料庫：

### 步驟 1: 安裝套件

```bash
dotnet add package Dapper
dotnet add package Microsoft.Data.Sqlite
```

### 步驟 2: 初始化資料庫

使用 `Database/create_schema.sql` 建立資料表：

```csharp
var dbContext = new AppDbContext("Data Source=myapp.db");
dbContext.Initialize();
```

### 步驟 3: 參考範例

查看 `Advanced/16_DapperIntegration` 的完整實作。

## 常見問題

### Q1: 為什麼我的 UI 沒有更新？

**A**: 最常見的原因是沒有實作 `INotifyPropertyChanged`。
確保你的 ViewModel 繼承自 `ViewModelBase` 並使用 `SetProperty` 方法。

```csharp
private string _name;
public string Name
{
    get => _name;
    set => SetProperty(ref _name, value);  // ✅ 正確
    // set { _name = value; }              // ❌ 錯誤
}
```

### Q2: 如何在 ViewModel 中顯示對話框？

**A**: 不要直接在 ViewModel 中使用 `MessageBox.Show()`！
使用 DialogService：

```csharp
// ViewModel 中
private readonly IDialogService _dialogService;

public void ShowMessage()
{
    _dialogService.ShowMessage("Hello!");
}
```

參考 `13_DialogService` 範例。

### Q3: Command 什麼時候執行？

**A**: Command 透過 XAML 綁定到 UI 控制項：

```xml
<Button Content="點我" Command="{Binding MyCommand}" />
```

當使用者點擊按鈕時，`MyCommand` 的 `Execute` 方法會執行。

### Q4: 如何在 ViewModel 之間傳遞資料？

**A**: 有三種方式：

1. **導航參數** (推薦) - 參考 `11_Navigation`
2. **事件聚合器** (解耦) - 參考 `19_EventAggregator`
3. **共享服務** (簡單) - 使用依賴注入

## 學習資源

### 核心概念優先順序

1. ⭐⭐⭐ **INotifyPropertyChanged** - 最重要！
2. ⭐⭐⭐ **Data Binding** - MVVM 的基礎
3. ⭐⭐⭐ **ICommand** - 處理使用者操作
4. ⭐⭐ **ObservableCollection** - 集合操作
5. ⭐⭐ **Validation** - 確保資料正確
6. ⭐ **Converter** - 資料轉換
7. ⭐ **DependencyProperty** - 自訂控制項

### 推薦學習順序

```
基礎 → 中階 → 進階
 ↓       ↓       ↓
理論 → 實作 → 整合
```

## 實用程式碼片段

### ViewModelBase 範本

```csharp
public class MyViewModel : ViewModelBase
{
    private string _property;

    public string Property
    {
        get => _property;
        set => SetProperty(ref _property, value);
    }

    public ICommand MyCommand { get; }

    public MyViewModel()
    {
        MyCommand = new RelayCommand(
            execute: _ => ExecuteMyCommand(),
            canExecute: _ => CanExecuteMyCommand()
        );
    }

    private void ExecuteMyCommand()
    {
        // 命令邏輯
    }

    private bool CanExecuteMyCommand()
    {
        return !string.IsNullOrEmpty(Property);
    }
}
```

### XAML 綁定範本

```xml
<Window x:Class="MyApp.Views.MainWindow"
        xmlns:vm="clr-namespace:MyApp.ViewModels">

    <Window.DataContext>
        <vm:MainViewModel />
    </Window.DataContext>

    <StackPanel>
        <!-- 文字綁定 -->
        <TextBlock Text="{Binding MyProperty}" />

        <!-- 雙向綁定 -->
        <TextBox Text="{Binding MyProperty, UpdateSourceTrigger=PropertyChanged}" />

        <!-- 命令綁定 -->
        <Button Content="執行" Command="{Binding MyCommand}" />
    </StackPanel>
</Window>
```

## 除錯技巧

### 1. 綁定錯誤

在 Visual Studio 輸出視窗查看綁定錯誤：
```
System.Windows.Data Error: 40 : BindingExpression path error...
```

### 2. 使用 PresentationTraceSources

在 XAML 中啟用追蹤：
```xml
<TextBlock Text="{Binding MyProperty,
    diag:PresentationTraceSources.TraceLevel=High}" />
```

### 3. 中斷點

在 ViewModel 的屬性 setter 設定中斷點：
```csharp
public string Name
{
    get => _name;
    set
    {
        SetProperty(ref _name, value);  // 在這裡設中斷點
    }
}
```

## 下一步

1. ✅ 完成基本範例 1-10
2. ✅ 建立你的第一個 MVVM 專案
3. ✅ 嘗試實作簡單的 CRUD 功能
4. ✅ 學習 Dapper 整合
5. ✅ 研究依賴注入和非同步操作

## 取得協助

遇到問題時：

1. 查看範例的 README.md
2. 檢查程式碼註解
3. 對照完整範例程式碼
4. 使用除錯工具追蹤問題

祝你學習愉快！🎉
