using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ValidationExample.ViewModels
{
    /// <summary>
    /// 使用者表單 ViewModel，實作 IDataErrorInfo 進行驗證
    /// </summary>
    public class UserFormViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _username;
        private string _email;
        private int _age;
        private string _password;
        private string _confirmPassword;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
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

        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ConfirmPassword)); // 密碼改變時，重新驗證確認密碼
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        #region IDataErrorInfo 實作

        /// <summary>
        /// 整體錯誤訊息（較少使用）
        /// </summary>
        public string Error => null;

        /// <summary>
        /// 欄位驗證邏輯
        /// </summary>
        public string this[string propertyName]
        {
            get
            {
                string error = null;

                switch (propertyName)
                {
                    case nameof(Username):
                        if (string.IsNullOrWhiteSpace(Username))
                            error = "使用者名稱不能為空";
                        else if (Username.Length < 3)
                            error = "使用者名稱至少需要 3 個字元";
                        else if (Username.Length > 20)
                            error = "使用者名稱不能超過 20 個字元";
                        break;

                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            error = "Email 不能為空";
                        else if (!IsValidEmail(Email))
                            error = "Email 格式不正確";
                        break;

                    case nameof(Age):
                        if (Age < 18)
                            error = "年齡必須大於或等於 18 歲";
                        else if (Age > 120)
                            error = "請輸入有效的年齡";
                        break;

                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password))
                            error = "密碼不能為空";
                        else if (Password.Length < 6)
                            error = "密碼至少需要 6 個字元";
                        break;

                    case nameof(ConfirmPassword):
                        if (ConfirmPassword != Password)
                            error = "兩次密碼輸入不一致";
                        break;
                }

                return error;
            }
        }

        #endregion

        /// <summary>
        /// 驗證 Email 格式
        /// </summary>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 驗證整個表單
        /// </summary>
        public bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(Username)]) &&
                   string.IsNullOrEmpty(this[nameof(Email)]) &&
                   string.IsNullOrEmpty(this[nameof(Age)]) &&
                   string.IsNullOrEmpty(this[nameof(Password)]) &&
                   string.IsNullOrEmpty(this[nameof(ConfirmPassword)]);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
