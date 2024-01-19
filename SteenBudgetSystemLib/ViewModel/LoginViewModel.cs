using SteenBudgetSystemLib.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SteenBudgetSystemLib.Helpers;

namespace SteenBudgetSystemLib.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private readonly IDialogService _dialogService;
        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
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

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            // Implement your login logic here
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ICommand implementation for opening the CreateUser window
        public ICommand OpenCreateUserCommand => new RelayCommand(_ => OpenCreateUser());

        private void OpenCreateUser()
        {
            _dialogService.ShowWindow("CreateUser");
        }
    }
}
