# 12 - Data Validation (資料驗證)

## 學習目標
- 理解 WPF 資料驗證機制
- 學習使用 IDataErrorInfo
- 掌握 ValidationRule 的使用
- 了解 Annotation 驗證

## 驗證方式
1. **IDataErrorInfo**: 最常用的驗證介面
2. **INotifyDataErrorInfo**: 支援非同步驗證
3. **ValidationRule**: 自訂驗證規則
4. **Data Annotations**: 使用屬性標註驗證

## 視覺回饋
- ValidatesOnDataErrors
- ValidatesOnExceptions
- NotifyOnValidationError
- 錯誤樣板 (Validation.ErrorTemplate)
