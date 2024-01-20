using SteenBudgetSystemLib.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SteenBudgetSystemLib.Helpers;
using SteenBudgetSystemLib.DataAccess;
using SteenBudgetSystemLib.Models;
using System.Windows.Controls;

namespace SteenBudgetSystemLib.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _loginInfoMessage;
        private bool _isLoginError;
        private SqlExecutor _sqlExecutor;
        private readonly IDialogService _dialogService;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OpenCreateUserCommand => new RelayCommand(_ => OpenCreateUser());
        public ICommand LoginCommand { get; }
        

        public LoginViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            LoginCommand = new RelayCommand(ExecuteLogin);
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public string LoginInfoMessage
        {
            get => _loginInfoMessage;
            set
            {
                _loginInfoMessage = value;
                OnPropertyChanged(nameof(LoginInfoMessage));
            }
        }
        public bool IsLoginError
        {
            get => _isLoginError;
            set
            {
                _isLoginError = value;
                OnPropertyChanged(nameof(IsLoginError));
            }
        }

        private void ExecuteLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox != null)
            {
                string password = passwordBox.Password;

                // Check if the Email is null, empty, or contains only whitespaces
                if (string.IsNullOrWhiteSpace(Email))
                {
                    LoginInfoMessage = "Email cannot be empty or contain whitespaces.";
                    IsLoginError = true;
                    return;
                }

                // Check if the Password is null, empty, or contains only whitespaces
                else if (string.IsNullOrWhiteSpace(password))
                {
                    LoginInfoMessage = "Password cannot be empty or contain whitespaces.";
                    IsLoginError = true;
                    return;
                }

                // If both Email and Password are valid, proceed with the login logic
                if (ValidateLogin(Email, password))
                {
                    // Handle successful login
                    IsLoginError = false;
                    _dialogService.ShowWindow("MainWindow");
                }
                else
                {
                    // Handle failed login
                    LoginInfoMessage = "Invalid email or password.";
                    IsLoginError = true;
                }
            }
        }


        private bool ValidateLogin(string email, string password)
        {
            _sqlExecutor = new SqlExecutor();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                // Show an error message or update a status property
                return false;
            }

            try
            {
                User user = _sqlExecutor.GetUserByEmail(email);
                if (user == null)
                {
                    // Return false without specifying the reason to avoid giving hints to attackers
                    return false;
                }

                string hashedInputPassword = PasswordHasher.HashPasswordWithSalt(password, Convert.FromBase64String(user.PasswordSalt));

                // Use constant-time comparison to compare hashed passwords
                return SecurePasswordComparison(hashedInputPassword, user.Password);
            }
            catch (Exception ex)
            {
                // Log the exception and handle the error appropriately
                // For example, return false or rethrow the exception
                return false;
            }
        }
        private bool SecurePasswordComparison(string hashedPassword1, string hashedPassword2)
        {
            if (hashedPassword1.Length != hashedPassword2.Length)
            {
                return false;
            }

            int result = 0;
            for (int i = 0; i < hashedPassword1.Length; i++)
            {
                result |= hashedPassword1[i] ^ hashedPassword2[i];
            }

            return result == 0;
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OpenCreateUser()
        {
            _dialogService.ShowWindow("CreateUser");
        }
    }
}
