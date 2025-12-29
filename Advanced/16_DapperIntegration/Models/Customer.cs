using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DapperIntegrationExample.Models
{
    /// <summary>
    /// 客戶資料模型
    /// </summary>
    public class Customer : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _email;
        private string _phone;
        private string _address;
        private DateTime _createdAt;
        private DateTime? _updatedAt;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged();
            }
        }

        public DateTime? UpdatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
