using System.Windows;
using Microsoft.Win32;

namespace DialogServiceExample.Services
{
    /// <summary>
    /// 對話框服務介面
    /// </summary>
    public interface IDialogService
    {
        void ShowMessage(string message);
        void ShowMessage(string message, string title);
        bool ShowConfirmation(string message, string title);
        void ShowError(string message, string title);
        string ShowOpenFileDialog(string filter = "All files (*.*)|*.*");
        string ShowSaveFileDialog(string filter = "All files (*.*)|*.*");
    }

    /// <summary>
    /// 對話框服務實作
    /// 封裝所有與對話框相關的操作，讓 ViewModel 保持純粹
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <summary>
        /// 顯示訊息對話框
        /// </summary>
        public void ShowMessage(string message)
        {
            ShowMessage(message, "訊息");
        }

        /// <summary>
        /// 顯示訊息對話框（含標題）
        /// </summary>
        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 顯示確認對話框
        /// </summary>
        /// <returns>使用者點擊確定返回 true，否則返回 false</returns>
        public bool ShowConfirmation(string message, string title)
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// 顯示錯誤對話框
        /// </summary>
        public void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// 顯示開啟檔案對話框
        /// </summary>
        /// <returns>選擇的檔案路徑，若取消則返回 null</returns>
        public string ShowOpenFileDialog(string filter = "All files (*.*)|*.*")
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter,
                Title = "選擇檔案"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        /// <summary>
        /// 顯示儲存檔案對話框
        /// </summary>
        /// <returns>儲存的檔案路徑，若取消則返回 null</returns>
        public string ShowSaveFileDialog(string filter = "All files (*.*)|*.*")
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter,
                Title = "儲存檔案"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}
