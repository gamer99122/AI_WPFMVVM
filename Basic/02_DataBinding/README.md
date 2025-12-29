# 02 - Data Binding (資料繫結)

## 學習目標
- 理解單向繫結 (OneWay)
- 理解雙向繫結 (TwoWay)
- 了解繫結模式的差異

## 核心概念
1. **OneWay Binding**: 資料從 ViewModel 流向 View
2. **TwoWay Binding**: 資料可以雙向流動
3. **OneTime Binding**: 只在初始化時繫結一次
4. **Binding Mode** 的選擇時機

## 範例說明
展示不同 Binding Mode 的行為差異：
- TextBlock 使用 OneWay (只讀顯示)
- TextBox 使用 TwoWay (可編輯並同步)

## 重點提示
大部分控制項有預設的 Binding Mode：
- TextBox.Text -> TwoWay
- TextBlock.Text -> OneWay
- Button.Content -> OneWay
