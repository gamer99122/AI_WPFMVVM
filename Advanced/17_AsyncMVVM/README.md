# 17 - Async MVVM (非同步 MVVM 模式)

## 學習目標
- 理解非同步操作在 MVVM 中的應用
- 學習 async/await 模式
- 掌握非同步命令的實作
- 處理長時間執行的操作

## 核心概念
1. **AsyncRelayCommand**: 支援非同步的命令
2. **IsBusy Pattern**: 忙碌狀態管理
3. **CancellationToken**: 取消操作
4. **Progress Reporting**: 進度回報

## 常見場景
- 資料庫查詢
- API 呼叫
- 檔案讀寫
- 長時間運算

## 注意事項
- UI 執行緒安全
- 異常處理
- 避免阻塞 UI
- 取消操作的支援
