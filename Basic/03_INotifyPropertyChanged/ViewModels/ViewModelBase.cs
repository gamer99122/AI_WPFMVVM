using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace INotifyPropertyChangedExample.ViewModels
{
    /// <summary>
    /// ViewModel 的基底類別
    /// 實作 INotifyPropertyChanged 介面，提供屬性變更通知功能
    /// 所有 ViewModel 都應該繼承此類別
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 屬性變更事件
        /// 當屬性值改變時觸發，通知 UI 更新
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 觸發屬性變更事件
        /// </summary>
        /// <param name="propertyName">屬性名稱（自動取得）</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // 使用 ?. 運算子安全地觸發事件
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 設定屬性值並觸發變更通知
        /// 只有當新值與舊值不同時才會更新
        /// </summary>
        /// <typeparam name="T">屬性型別</typeparam>
        /// <param name="field">欄位的參考</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">屬性名稱（自動取得）</param>
        /// <returns>如果值有變更回傳 true，否則回傳 false</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            // 比較新舊值
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            // 更新欄位值
            field = value;

            // 觸發屬性變更通知
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
