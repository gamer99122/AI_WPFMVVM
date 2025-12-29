using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ListBindingExample.Models;

namespace ListBindingExample.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Employee _selectedEmployee;

        /// <summary>
        /// 員工集合 - 使用 ObservableCollection 以支援自動 UI 更新
        /// </summary>
        public ObservableCollection<Employee> Employees { get; set; }

        /// <summary>
        /// 當前選中的員工
        /// </summary>
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            // 初始化範例資料
            Employees = new ObservableCollection<Employee>
            {
                new Employee { Id = 1, Name = "張小明", Department = "資訊部", Position = "軟體工程師", Salary = 60000 },
                new Employee { Id = 2, Name = "李美美", Department = "人資部", Position = "人資專員", Salary = 45000 },
                new Employee { Id = 3, Name = "王大明", Department = "業務部", Position = "業務經理", Salary = 75000 },
                new Employee { Id = 4, Name = "陳小華", Department = "資訊部", Position = "系統分析師", Salary = 70000 },
                new Employee { Id = 5, Name = "林雅婷", Department = "財務部", Position = "會計師", Salary = 55000 },
                new Employee { Id = 6, Name = "黃志明", Department = "資訊部", Position = "專案經理", Salary = 80000 }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
