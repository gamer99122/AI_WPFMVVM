using System;
using System.Windows.Input;

namespace RelayCommandExample.Commands
{
    /// <summary>
    /// 通用的命令實作類別
    /// 實作 ICommand 介面，讓 ViewModel 可以綁定命令到 UI 控制項
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="execute">要執行的動作</param>
        /// <param name="canExecute">判斷是否可執行的函式（可選）</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// 當命令的可執行狀態可能改變時觸發
        /// 綁定到 CommandManager.RequerySuggested，讓 WPF 自動管理
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// 判斷命令是否可執行
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// 執行命令
        /// </summary>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    /// <summary>
    /// 泛型版本的 RelayCommand
    /// 提供強型別的參數
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
