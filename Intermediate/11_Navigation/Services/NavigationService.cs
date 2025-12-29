using System;
using System.Windows.Controls;

namespace NavigationExample.Services
{
    /// <summary>
    /// 導航服務介面
    /// </summary>
    public interface INavigationService
    {
        void NavigateTo(string pageKey);
        void NavigateTo(string pageKey, object parameter);
        void GoBack();
        bool CanGoBack { get; }
    }

    /// <summary>
    /// 導航服務實作
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;
        private readonly Dictionary<string, Type> _pages;

        public NavigationService(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
            _pages = new Dictionary<string, Type>();
        }

        /// <summary>
        /// 註冊頁面
        /// </summary>
        public void RegisterPage(string key, Type pageType)
        {
            if (!typeof(Page).IsAssignableFrom(pageType))
            {
                throw new ArgumentException($"{pageType.Name} 必須繼承自 Page");
            }

            _pages[key] = pageType;
        }

        /// <summary>
        /// 導航到指定頁面
        /// </summary>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <summary>
        /// 導航到指定頁面，並傳遞參數
        /// </summary>
        public void NavigateTo(string pageKey, object parameter)
        {
            if (!_pages.ContainsKey(pageKey))
            {
                throw new ArgumentException($"找不到頁面: {pageKey}");
            }

            var pageType = _pages[pageKey];
            var page = Activator.CreateInstance(pageType) as Page;

            // 如果需要傳遞參數，可以透過 DataContext 或自訂屬性
            if (parameter != null && page != null)
            {
                // 這裡可以實作參數傳遞邏輯
                // page.DataContext = parameter; 或其他方式
            }

            _frame.Navigate(page);
        }

        /// <summary>
        /// 返回上一頁
        /// </summary>
        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }

        /// <summary>
        /// 是否可以返回
        /// </summary>
        public bool CanGoBack => _frame.CanGoBack;
    }
}
