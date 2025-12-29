# 20 - Complete Application (完整應用程式範例)

## 專案概述
這是一個完整的 WPF MVVM 應用程式範例，整合了前面所有的概念和技術。

## 功能特點
- 客戶管理（CRUD）
- 產品管理（CRUD）
- 訂單管理
- 資料搜尋與篩選
- 資料驗證
- 多頁面導航
- 非同步操作
- 依賴注入
- 事件聚合器

## 技術棧整合
- ✅ MVVM 架構
- ✅ Dapper ORM
- ✅ SQLite 資料庫
- ✅ 依賴注入 (DI)
- ✅ 非同步操作
- ✅ Repository 模式
- ✅ 事件聚合器
- ✅ 資料驗證
- ✅ 導航服務
- ✅ 對話框服務

## 專案結構
```
20_CompleteApp/
├── Models/              # 資料模型
│   ├── Customer.cs
│   ├── Product.cs
│   └── Order.cs
├── Data/                # 資料存取層
│   ├── DbContext.cs
│   └── Repositories/
│       ├── CustomerRepository.cs
│       ├── ProductRepository.cs
│       └── OrderRepository.cs
├── Services/            # 服務層
│   ├── NavigationService.cs
│   ├── DialogService.cs
│   └── EventAggregator.cs
├── ViewModels/          # 視圖模型
│   ├── Base/
│   │   └── ViewModelBase.cs
│   ├── MainViewModel.cs
│   ├── CustomerViewModel.cs
│   ├── ProductViewModel.cs
│   └── OrderViewModel.cs
├── Views/               # 視圖
│   ├── MainWindow.xaml
│   ├── CustomerView.xaml
│   ├── ProductView.xaml
│   └── OrderView.xaml
├── Commands/            # 命令
│   ├── RelayCommand.cs
│   └── AsyncRelayCommand.cs
├── Converters/          # 轉換器
│   └── ValueConverters.cs
└── App.xaml            # 應用程式入口
```

## 資料庫架構
資料庫包含三個主要資料表：
- **Customers**: 客戶資料
- **Products**: 產品資料
- **Orders**: 訂單資料

## 開發指南

### 1. 建立專案
```bash
dotnet new wpf -n CompleteApp -f net8.0
```

### 2. 安裝 NuGet 套件
```bash
dotnet add package Dapper
dotnet add package Microsoft.Data.Sqlite
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Hosting
```

### 3. 執行應用程式
```bash
dotnet run
```

## 學習重點

### 1. 分層架構
- **Presentation Layer**: Views + ViewModels
- **Business Logic Layer**: Services
- **Data Access Layer**: Repositories
- **Data Layer**: Models + DbContext

### 2. 設計模式
- MVVM Pattern
- Repository Pattern
- Service Pattern
- Observer Pattern (Event Aggregator)
- Command Pattern

### 3. 最佳實踐
- 依賴注入降低耦合
- 非同步操作避免阻塞 UI
- 資料驗證確保資料正確性
- 使用介面提高可測試性
- 適當的錯誤處理

## 擴展建議
1. 新增報表功能
2. 實作匯出/匯入功能
3. 加入使用者權限管理
4. 實作資料同步功能
5. 加入日誌記錄
6. 實作多語系支援

## 效能優化
- 虛擬化長列表
- 延遲載入資料
- 快取常用資料
- 批次處理資料庫操作
- 使用非同步操作

這個範例展示了一個生產級別的 WPF MVVM 應用程式應該如何組織和實作。
