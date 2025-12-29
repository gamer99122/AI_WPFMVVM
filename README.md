# WPF MVVM 完整學習指南

## 概述
本專案包含 20 個由淺入深的 WPF MVVM 範例，使用 C# + .NET 8 開發。
範例分為三個層級：基本（10個）、中階（5個）、進階（5個）。

## 技術棧
- .NET 8.0
- WPF (Windows Presentation Foundation)
- MVVM Pattern (Model-View-ViewModel)
- Dapper ORM (資料庫存取)
- SQLite (範例資料庫)

## 專案結構

### 基本範例 (Basic)
1. **01_HelloWorldMVVM** - 最簡單的 MVVM 架構
2. **02_DataBinding** - 基本資料繫結
3. **03_INotifyPropertyChanged** - 屬性變更通知
4. **04_RelayCommand** - 命令模式實作
5. **05_ListBinding** - 列表資料繫結
6. **06_TwoWayBinding** - 雙向繫結
7. **07_Converter** - 值轉換器
8. **08_CollectionView** - 集合視圖操作
9. **09_UserControl** - 自訂使用者控制項
10. **10_DependencyProperty** - 相依性屬性

### 中階範例 (Intermediate)
11. **11_Navigation** - 頁面導航
12. **12_Validation** - 資料驗證
13. **13_DialogService** - 對話框服務
14. **14_SimpleCRUD** - 簡單增刪改查
15. **15_ObservableCollection** - 可觀察集合進階應用

### 進階範例 (Advanced)
16. **16_DapperIntegration** - Dapper 完整整合
17. **17_AsyncMVVM** - 非同步 MVVM 模式
18. **18_DependencyInjection** - 依賴注入 (DI)
19. **19_EventAggregator** - 事件聚合器
20. **20_CompleteApp** - 完整應用程式範例

## 開始使用

### 前置需求
- Visual Studio 2022 或更新版本
- .NET 8 SDK
- Windows 10/11

### 建立專案
每個範例都是獨立的 WPF 應用程式專案。使用以下命令建立：

```bash
dotnet new wpf -n [專案名稱] -f net8.0
```

### 安裝必要套件
對於使用 Dapper 的範例：

```bash
dotnet add package Dapper
dotnet add package Microsoft.Data.Sqlite
dotnet add package Microsoft.Extensions.DependencyInjection
```

## 學習路徑建議
1. 依序從基本範例開始學習
2. 理解每個範例的核心概念
3. 嘗試修改範例程式碼
4. 進入中階範例前，確保掌握所有基本範例
5. 進階範例整合多個概念，建議最後學習

## MVVM 核心概念

### Model (模型)
- 代表應用程式的資料和業務邏輯
- 不依賴於 View 和 ViewModel
- 包含資料驗證邏輯

### View (視圖)
- XAML 定義的使用者介面
- 透過 DataBinding 與 ViewModel 互動
- 不包含業務邏輯

### ViewModel (視圖模型)
- View 和 Model 之間的橋樑
- 實作 INotifyPropertyChanged 介面
- 提供 Command 供 View 綁定
- 包含 UI 邏輯

## 常用輔助類別

### RelayCommand
```csharp
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
    public void Execute(object parameter) => _execute(parameter);
}
```

### ViewModelBase
```csharp
public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
```

## 資料庫設定 (Dapper 範例)

請參考 `Database` 資料夾中的 SQL 腳本來建立範例資料庫。

## 貢獻
歡迎提出問題或改進建議！

## 授權
本專案僅供學習使用。
