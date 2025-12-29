using System;

namespace DataBindingExample.ViewModels
{
    /// <summary>
    /// 展示不同資料繫結模式的 ViewModel
    /// 注意：這個範例還沒有實作 INotifyPropertyChanged
    /// 所以屬性變更不會自動通知 UI
    /// </summary>
    public class MainViewModel
    {
        public string OneWayText { get; set; }
        public string TwoWayText { get; set; }
        public string OneTimeText { get; set; }

        public MainViewModel()
        {
            OneWayText = "這是 OneWay 繫結（單向）";
            TwoWayText = "這是 TwoWay 繫結（雙向）";
            OneTimeText = "這是 OneTime 繫結（一次性）";
        }
    }
}
