using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using RelayCommandExample.Commands;

namespace RelayCommandExample.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _inputText;
        private string _displayText;
        private int _clickCount;

        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
                // 當輸入文字改變時，通知 ShowMessageCommand 重新檢查 CanExecute
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string DisplayText
        {
            get => _displayText;
            set
            {
                _displayText = value;
                OnPropertyChanged();
            }
        }

        public int ClickCount
        {
            get => _clickCount;
            set
            {
                _clickCount = value;
                OnPropertyChanged();
            }
        }

        // 命令屬性
        public ICommand ShowMessageCommand { get; }
        public ICommand IncrementCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ClearCommand { get; }

        public MainViewModel()
        {
            // 初始化
            InputText = "";
            DisplayText = "請輸入文字並點擊按鈕";
            ClickCount = 0;

            // 初始化命令
            // ShowMessageCommand: 只有當 InputText 不是空白時才能執行
            ShowMessageCommand = new RelayCommand(
                execute: _ => ShowMessage(),
                canExecute: _ => !string.IsNullOrWhiteSpace(InputText)
            );

            // IncrementCommand: 總是可以執行
            IncrementCommand = new RelayCommand(
                execute: _ => ClickCount++
            );

            // ResetCommand: 只有當 ClickCount > 0 時才能執行
            ResetCommand = new RelayCommand(
                execute: _ => ResetCounter(),
                canExecute: _ => ClickCount > 0
            );

            // ClearCommand: 清除所有資料
            ClearCommand = new RelayCommand(
                execute: _ => ClearAll()
            );
        }

        private void ShowMessage()
        {
            DisplayText = $"您輸入了: {InputText}";
            MessageBox.Show(InputText, "訊息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetCounter()
        {
            ClickCount = 0;
            DisplayText = "計數器已重置";
        }

        private void ClearAll()
        {
            InputText = "";
            DisplayText = "已清除所有資料";
            ClickCount = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
