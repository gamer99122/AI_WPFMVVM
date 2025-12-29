# 18 - Dependency Injection (依賴注入)

## 學習目標
- 理解依賴注入的概念和優勢
- 學習使用 Microsoft.Extensions.DependencyInjection
- 掌握 MVVM 中的 DI 模式
- 了解服務生命週期

## 核心概念
1. **IoC Container**: 控制反轉容器
2. **Service Lifetime**: 服務生命週期
   - Singleton: 單例
   - Scoped: 範圍
   - Transient: 暫時性
3. **Constructor Injection**: 建構子注入
4. **Service Locator**: 服務定位器

## NuGet 套件
```bash
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Hosting
```

## 優勢
- 降低耦合度
- 提高可測試性
- 更好的程式碼組織
- 易於維護和擴展
