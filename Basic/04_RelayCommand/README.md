# 04 - RelayCommand

## 學習目標
- 理解 ICommand 介面
- 學習如何實作 RelayCommand
- 了解如何在 MVVM 中處理使用者操作
- 掌握 CanExecute 的使用

## 核心概念
1. **ICommand 介面**: WPF 命令模式的基礎
2. **RelayCommand**: 通用的命令實作類別
3. **Execute**: 執行命令的邏輯
4. **CanExecute**: 判斷命令是否可執行

## 為什麼需要 Command？
在 MVVM 中，我們不應該在 Code-Behind 寫事件處理程式。
Command 讓我們可以在 ViewModel 中處理使用者的互動。

## 使用場景
- Button 點擊
- MenuItem 選擇
- 鍵盤快捷鍵
- 任何需要執行動作的地方
