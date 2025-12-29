# 16 - Dapper Integration (Dapper 整合)

## 學習目標
- 理解 Dapper ORM 的使用
- 學習在 MVVM 中整合資料庫操作
- 掌握 Repository 模式

## 技術棧
- **Dapper**: 輕量級 ORM
- **SQLite**: 嵌入式資料庫
- **Repository Pattern**: 資料存取層抽象

## 專案結構
```
16_DapperIntegration/
├── Models/          # 資料模型
├── Data/            # 資料存取層
│   ├── DbContext.cs
│   └── Repositories/
├── ViewModels/      # 視圖模型
└── Views/           # 視圖
```

## NuGet 套件
```bash
dotnet add package Dapper
dotnet add package Microsoft.Data.Sqlite
```

## 核心概念
1. **DbContext**: 資料庫連線管理
2. **Repository Pattern**: 封裝資料存取邏輯
3. **Unit of Work**: 事務管理
4. **Connection Management**: 連線管理
