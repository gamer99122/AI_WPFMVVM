using System;

namespace HelloWorldMVVM.ViewModels
{
    /// <summary>
    /// 主視窗的 ViewModel
    /// 這是最簡單的 ViewModel，只包含一個屬性
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// 要顯示的訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 應用程式標題
        /// </summary>
        public string Title { get; set; }

        public MainViewModel()
        {
            // 初始化資料
            Title = "Hello World MVVM";
            Message = "歡迎來到 WPF MVVM 的世界！";
        }
    }
}
