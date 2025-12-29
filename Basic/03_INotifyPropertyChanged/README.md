# 03 - INotifyPropertyChanged

## 學習目標
- 理解 INotifyPropertyChanged 介面的重要性
- 學習如何實作屬性變更通知
- 了解 CallerMemberName 屬性的使用
- 建立可重用的 ViewModelBase 類別

## 核心概念
1. **INotifyPropertyChanged**: MVVM 的核心介面
2. **PropertyChanged 事件**: 通知 UI 屬性已變更
3. **OnPropertyChanged 方法**: 觸發屬性變更事件
4. **SetProperty 方法**: 簡化屬性設定程式碼

## 為什麼需要 INotifyPropertyChanged？
沒有它，UI 無法自動更新當 ViewModel 的屬性改變時。
這是 MVVM 雙向資料繫結的基礎。

## ViewModelBase 模式
建立一個基底類別，所有 ViewModel 都繼承它，
避免在每個 ViewModel 重複實作相同的程式碼。
