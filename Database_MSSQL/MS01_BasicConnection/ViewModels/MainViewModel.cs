using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MS01_BasicConnection.Data;
using MS01_BasicConnection.Models;

namespace MS01_BasicConnection.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseHelper _dbHelper;
        private string _connectionStatus;
        private string _serverVersion;
        private int _customerCount;
        private bool _isLoading;

        public ObservableCollection<Customer> Customers { get; set; }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        public string ServerVersion
        {
            get => _serverVersion;
            set
            {
                _serverVersion = value;
                OnPropertyChanged();
            }
        }

        public int CustomerCount
        {
            get => _customerCount;
            set
            {
                _customerCount = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        // 命令
        public ICommand TestConnectionCommand { get; }
        public ICommand LoadCustomersCommand { get; }
        public ICommand InitializeDatabaseCommand { get; }

        public MainViewModel()
        {
            // 初始化
            Customers = new ObservableCollection<Customer>();

            // 連接字串 - 請根據您的環境修改
            // 選項 1: Windows 驗證
            string connectionString = "Server=localhost;Database=WpfMvvmDemo;Trusted_Connection=True;TrustServerCertificate=True;";

            // 選項 2: SQL Server 驗證
            // string connectionString = "Server=localhost;Database=WpfMvvmDemo;User Id=sa;Password=YourPassword;TrustServerCertificate=True;";

            // 選項 3: LocalDB（最簡單）
            // string connectionString = "Server=(localdb)\\mssqllocaldb;Database=WpfMvvmDemo;Trusted_Connection=True;";

            _dbHelper = new DatabaseHelper(connectionString);

            // 初始化命令
            TestConnectionCommand = new RelayCommand(_ => TestConnection());
            LoadCustomersCommand = new RelayCommand(_ => LoadCustomers());
            InitializeDatabaseCommand = new RelayCommand(_ => InitializeDatabase());

            // 預設顯示訊息
            ConnectionStatus = "尚未連線";
            ServerVersion = "N/A";
        }

        /// <summary>
        /// 測試資料庫連線
        /// </summary>
        private void TestConnection()
        {
            try
            {
                IsLoading = true;

                bool isConnected = _dbHelper.TestConnection();

                if (isConnected)
                {
                    ConnectionStatus = "✅ 連線成功";
                    ServerVersion = _dbHelper.GetServerVersion();
                    MessageBox.Show("資料庫連線成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ConnectionStatus = "❌ 連線失敗";
                    ServerVersion = "N/A";
                    MessageBox.Show("無法連線到資料庫，請檢查連接字串和 SQL Server 服務。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                ConnectionStatus = "❌ 連線錯誤";
                MessageBox.Show($"連線錯誤: {ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 載入客戶資料
        /// </summary>
        private void LoadCustomers()
        {
            try
            {
                IsLoading = true;
                Customers.Clear();

                var customers = _dbHelper.GetAllCustomers();

                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }

                CustomerCount = _dbHelper.GetCustomerCount();

                MessageBox.Show($"成功載入 {customers.Count} 筆客戶資料", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入客戶資料失敗: {ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 初始化資料庫（建立資料表和範例資料）
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                var result = MessageBox.Show(
                    "這將建立資料表並插入範例資料。\n如果資料表已存在，將不會重複建立。\n\n是否繼續？",
                    "確認",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    IsLoading = true;
                    _dbHelper.InitializeDatabase();
                    MessageBox.Show("資料庫初始化成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);

                    // 自動載入資料
                    LoadCustomers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化資料庫失敗: {ex.Message}\n\n請確認：\n1. SQL Server 服務已啟動\n2. 資料庫已建立\n3. 有足夠的權限", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region RelayCommand

        private class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Func<object, bool> _canExecute;

            public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
            public void Execute(object parameter) => _execute(parameter);
        }

        #endregion
    }
}
