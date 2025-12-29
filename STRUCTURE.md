# 專案完整結構

```
AI_WPFMVVM/
│
├── README.md                          # 專案總覽與介紹
├── GETTING_STARTED.md                 # 快速入門指南
├── PROJECT_INDEX.md                   # 範例索引與快速查找
└── STRUCTURE.md                       # 本檔案 - 專案結構說明
│
├── Basic/                             # 基本範例 (10個)
│   │
│   ├── 01_HelloWorldMVVM/             # ⭐ MVVM 入門
│   │   ├── README.md
│   │   ├── ViewModels/
│   │   │   └── MainViewModel.cs
│   │   └── Views/
│   │       ├── MainWindow.xaml
│   │       └── MainWindow.xaml.cs
│   │
│   ├── 02_DataBinding/                # ⭐ 資料繫結基礎
│   │   ├── README.md
│   │   ├── ViewModels/
│   │   │   └── MainViewModel.cs
│   │   └── Views/
│   │       └── MainWindow.xaml
│   │
│   ├── 03_INotifyPropertyChanged/     # ⭐⭐⭐ 屬性變更通知 (核心!)
│   │   ├── README.md
│   │   ├── ViewModels/
│   │   │   ├── ViewModelBase.cs       # 重要：所有專案都需要
│   │   │   └── MainViewModel.cs
│   │   └── Views/
│   │       └── MainWindow.xaml
│   │
│   ├── 04_RelayCommand/               # ⭐⭐⭐ 命令模式 (核心!)
│   │   ├── README.md
│   │   ├── Commands/
│   │   │   └── RelayCommand.cs        # 重要：命令基礎類別
│   │   ├── ViewModels/
│   │   │   └── MainViewModel.cs
│   │   └── Views/
│   │       └── MainWindow.xaml
│   │
│   ├── 05_ListBinding/                # ⭐⭐ 列表資料繫結
│   │   ├── README.md
│   │   ├── Models/
│   │   │   └── Employee.cs
│   │   ├── ViewModels/
│   │   │   └── MainViewModel.cs
│   │   └── Views/
│   │       └── MainWindow.xaml
│   │
│   ├── 06_TwoWayBinding/              # ⭐ 雙向繫結進階
│   │   └── README.md
│   │
│   ├── 07_Converter/                  # ⭐⭐ 值轉換器
│   │   ├── README.md
│   │   └── Converters/
│   │       └── BoolToVisibilityConverter.cs
│   │
│   ├── 08_CollectionView/             # ⭐ 集合視圖
│   │   └── README.md
│   │
│   ├── 09_UserControl/                # ⭐ 自訂控制項
│   │   └── README.md
│   │
│   └── 10_DependencyProperty/         # ⭐ 相依性屬性
│       └── README.md
│
├── Intermediate/                      # 中階範例 (5個)
│   │
│   ├── 11_Navigation/                 # ⭐⭐ 頁面導航
│   │   ├── README.md
│   │   └── Services/
│   │       └── NavigationService.cs   # 導航服務實作
│   │
│   ├── 12_Validation/                 # ⭐⭐⭐ 資料驗證 (重要!)
│   │   ├── README.md
│   │   └── ViewModels/
│   │       └── UserFormViewModel.cs   # IDataErrorInfo 範例
│   │
│   ├── 13_DialogService/              # ⭐⭐ 對話框服務
│   │   ├── README.md
│   │   └── Services/
│   │       └── DialogService.cs       # 對話框服務實作
│   │
│   ├── 14_SimpleCRUD/                 # ⭐⭐ 簡單 CRUD
│   │   ├── README.md
│   │   └── Models/
│   │       └── Product.cs
│   │
│   └── 15_ObservableCollection/       # ⭐⭐ 集合進階應用
│       └── README.md
│
├── Advanced/                          # 進階範例 (5個)
│   │
│   ├── 16_DapperIntegration/          # ⭐⭐⭐ Dapper 資料庫整合 (重要!)
│   │   ├── README.md
│   │   ├── Data/
│   │   │   ├── DbContext.cs           # 資料庫上下文
│   │   │   └── Repositories/
│   │   │       └── CustomerRepository.cs  # Repository 模式
│   │   └── Models/
│   │       └── Customer.cs
│   │
│   ├── 17_AsyncMVVM/                  # ⭐⭐⭐ 非同步 MVVM
│   │   ├── README.md
│   │   └── Commands/
│   │       └── AsyncRelayCommand.cs   # 非同步命令
│   │
│   ├── 18_DependencyInjection/        # ⭐⭐⭐ 依賴注入 (重要!)
│   │   ├── README.md
│   │   └── App.xaml.cs               # DI 容器設定
│   │
│   ├── 19_EventAggregator/            # ⭐⭐ 事件聚合器
│   │   ├── README.md
│   │   └── Services/
│   │       └── EventAggregator.cs     # 發布-訂閱模式
│   │
│   └── 20_CompleteApp/                # ⭐⭐⭐ 完整應用程式 (整合所有概念)
│       └── README.md
│           ├── Models/                # 所有資料模型
│           ├── ViewModels/            # 所有 ViewModels
│           ├── Views/                 # 所有 Views
│           ├── Services/              # 所有服務
│           ├── Data/                  # 資料存取層
│           │   └── Repositories/
│           └── Commands/              # 所有命令
│
└── Database/                          # 資料庫相關
    ├── README.md                      # 資料庫使用說明
    ├── create_schema.sql              # 建立資料表架構
    ├── seed_data.sql                  # 範例資料
    └── queries.sql                    # 常用查詢範例

```

## 檔案類型說明

### 📄 README.md
每個範例都有自己的 README 文件，包含：
- 學習目標
- 核心概念
- 使用說明
- 重點提示

### 🔧 核心類別檔案

#### ViewModelBase.cs
- 位置：`Basic/03_INotifyPropertyChanged/ViewModels/`
- 用途：所有 ViewModel 的基底類別
- 必要性：⭐⭐⭐ 每個專案都需要

#### RelayCommand.cs
- 位置：`Basic/04_RelayCommand/Commands/`
- 用途：通用命令實作
- 必要性：⭐⭐⭐ 處理使用者操作必備

#### AsyncRelayCommand.cs
- 位置：`Advanced/17_AsyncMVVM/Commands/`
- 用途：非同步命令實作
- 必要性：⭐⭐ 處理長時間操作時需要

### 🎨 UI 檔案

#### .xaml
- XAML 視圖定義
- 包含 UI 佈局和資料繫結

#### .xaml.cs
- Code-Behind 檔案
- 在 MVVM 中應該保持簡潔

### 📦 資料模型 (Models/)
- 純資料類別
- 代表業務實體
- 不包含業務邏輯

### 🎯 視圖模型 (ViewModels/)
- View 和 Model 之間的橋樑
- 包含 UI 邏輯
- 實作命令和屬性

### 🔌 服務 (Services/)
- DialogService：對話框
- NavigationService：導航
- EventAggregator：事件通訊

### 💾 資料存取 (Data/)
- DbContext：資料庫上下文
- Repositories：資料存取抽象

## 學習建議路徑

### 階段 1：建立基礎 (第 1-2 週)
```
01_HelloWorldMVVM
    ↓
02_DataBinding
    ↓
03_INotifyPropertyChanged ⭐ 重點
    ↓
04_RelayCommand ⭐ 重點
    ↓
05_ListBinding
```

### 階段 2：進階功能 (第 3-4 週)
```
12_Validation ⭐ 重點
    ↓
13_DialogService
    ↓
14_SimpleCRUD
    ↓
11_Navigation
```

### 階段 3：專業開發 (第 5-8 週)
```
16_DapperIntegration ⭐ 重點
    ↓
17_AsyncMVVM
    ↓
18_DependencyInjection ⭐ 重點
    ↓
19_EventAggregator
    ↓
20_CompleteApp ⭐ 整合
```

## 每個資料夾的用途

### Basic/
初學者友善的範例，每個專注於單一概念。
適合循序漸進學習 MVVM 基礎。

### Intermediate/
整合多個基本概念的中階範例。
涵蓋實際應用中常見的需求。

### Advanced/
企業級開發模式和最佳實踐。
包含資料庫、非同步、DI 等進階主題。

### Database/
所有資料庫相關的腳本和說明。
支援 Dapper 範例的資料庫需求。

## 開始使用

### 1. 瀏覽 README.md
了解整個專案的概述和技術棧

### 2. 閱讀 GETTING_STARTED.md
快速入門指南，建立第一個專案

### 3. 查看 PROJECT_INDEX.md
根據需求快速找到對應範例

### 4. 依序學習範例
從 Basic 開始，逐步進階

## 複製可重用的程式碼

建立新專案時，這些檔案可以直接複製：

1. ✅ `ViewModelBase.cs` - 從 03_INotifyPropertyChanged
2. ✅ `RelayCommand.cs` - 從 04_RelayCommand
3. ✅ `DialogService.cs` - 從 13_DialogService
4. ✅ `NavigationService.cs` - 從 11_Navigation
5. ✅ `AsyncRelayCommand.cs` - 從 17_AsyncMVVM
6. ✅ `EventAggregator.cs` - 從 19_EventAggregator

## 資料庫初始化

使用 Dapper 範例時：

```bash
# 1. 查看 Database/create_schema.sql
# 2. 在程式碼中初始化：
var dbContext = new AppDbContext("Data Source=app.db");
dbContext.Initialize();

# 3. (可選) 插入範例資料：
# 執行 Database/seed_data.sql
```

## 建議的專案結構

當你建立自己的專案時，推薦使用這個結構：

```
MyWPFApp/
├── Models/              # 資料模型
├── ViewModels/          # 視圖模型
│   └── Base/            # 基底類別
├── Views/               # XAML 視圖
├── Commands/            # 命令類別
├── Services/            # 服務介面與實作
├── Data/                # 資料存取
│   └── Repositories/    # Repository
├── Converters/          # 值轉換器
├── Helpers/             # 輔助類別
└── Resources/           # 資源檔案
```

---

📚 開始探索這些範例，成為 WPF MVVM 專家！
