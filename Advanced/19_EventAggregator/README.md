# 19 - Event Aggregator (事件聚合器)

## 學習目標
- 理解事件聚合器模式
- 學習 ViewModel 之間的通訊
- 實作發布-訂閱模式
- 掌握弱引用避免記憶體洩漏

## 核心概念
1. **Publish-Subscribe Pattern**: 發布-訂閱模式
2. **Weak References**: 弱引用避免記憶體洩漏
3. **Message Types**: 訊息類型設計
4. **Thread Safety**: 執行緒安全

## 使用場景
- ViewModel 之間的通訊
- 跨模組的事件通知
- 解耦合的訊息傳遞
- 廣播式通知

## 實作方式
1. 簡單的事件聚合器
2. 使用泛型訊息
3. 支援篩選器
4. 非同步訊息處理
