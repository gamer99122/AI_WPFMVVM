using System;

namespace INotifyPropertyChangedExample.ViewModels
{
    /// <summary>
    /// 主視窗的 ViewModel
    /// 展示如何使用 INotifyPropertyChanged 實現屬性變更通知
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private string _firstName;
        private string _lastName;
        private int _age;

        /// <summary>
        /// 名字屬性
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                {
                    // 當名字改變時，全名也會改變，所以需要通知 UI
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        /// <summary>
        /// 姓氏屬性
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                {
                    // 當姓氏改變時，全名也會改變
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        /// <summary>
        /// 年齡屬性
        /// </summary>
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        /// <summary>
        /// 計算屬性：全名
        /// 這是一個只讀屬性，由 FirstName 和 LastName 組成
        /// </summary>
        public string FullName => $"{LastName} {FirstName}";

        public MainViewModel()
        {
            // 初始化預設值
            FirstName = "小明";
            LastName = "王";
            Age = 25;
        }
    }
}
