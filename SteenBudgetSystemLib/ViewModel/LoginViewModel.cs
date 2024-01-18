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
        private readonly IDialogService _dialogService;

        public LoginViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
