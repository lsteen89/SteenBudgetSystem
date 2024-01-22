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
using System.Diagnostics;

namespace SteenBudgetSystemLib.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _loginInfoMessage;
        private bool _isLoginError;
        private bool _debug;
        private SqlExecutor _sqlExecutor;
        private readonly IDialogService _dialogService;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OpenCreateUserCommand => new RelayCommand(_ => OpenCreateUser());
        public ICommand LoginCommand { get; }
        private readonly ISessionService _sessionService;
        private readonly AuthenticationService _authenticationService;


        public LoginViewModel(IDialogService dialogService, ISessionService sessionService, AuthenticationService authenticationService)
        {
            _dialogService = dialogService;
            _sessionService = sessionService;
            LoginCommand = new RelayCommand(ExecuteLogin);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
            _authenticationService = authenticationService;
            _authenticationService = authenticationService;
        }
        /*
         DEBUGGING
         */
        public ICommand ForgotPasswordCommand { get; }
        private void ExecuteForgotPassword(object parameter)
        {
            Email = "test@test.se";
            Password = "Smillan00";
            _debug = true;
            ExecuteLogin(null);
        }

        /*End debug*/
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
            string password = _debug ? Password : passwordBox?.Password;

            var (isSuccess, errorMessage) = _authenticationService.Login(Email, password);

            if (isSuccess)
            {
                IsLoginError = false;
                // Additional logic for successful login
            }
            else
            {
                LoginInfoMessage = errorMessage;
                IsLoginError = true;
            }
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
