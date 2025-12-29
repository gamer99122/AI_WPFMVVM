using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleCRUDExample.Models
{
    /// <summary>
    /// 產品模型
    /// </summary>
    public class Product : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _category;
        private decimal _price;
        private int _stock;

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

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public int Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 複製產品（用於編輯時避免直接修改原物件）
        /// </summary>
        public Product Clone()
        {
            return new Product
            {
                Id = this.Id,
                Name = this.Name,
                Category = this.Category,
                Price = this.Price,
                Stock = this.Stock
            };
        }
    }
}
