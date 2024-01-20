using SteenBudgetSystemLib.Helpers;
using SteenBudgetSystemLib.Models;
using SteenBudgetSystemLib.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SteenBudgetSystemLib.ViewModel
{
    public class FirstTimeSetupViewModel : INotifyPropertyChanged
    {
        private bool _userHasPartner = false;

        private readonly ISessionService _sessionService;
        private UserSession _userSession;
        private string _salaryInput;
        private string _userMainIncome;
        private string _userOtherIncome;
        private string _partnerMainIncome;
        private string _partnerOtherIncome;

        public RelayCommand DebugButtonCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private Brush _textBlockColor = Brushes.Black;
        private Visibility _partnerDetailsVisibility = Visibility.Collapsed;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserSession UserSession
        {
            get { return _userSession; }
            set { _userSession = value; }
        }
        public FirstTimeSetupViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            UserSession = _sessionService.CurrentSession;
            DebugButtonCommand = new RelayCommand(ExecuteDebugButtonCommand);
            TextBlockColor = Brushes.Red;
        }

        public string UserMainIncome
        {
            get => _userMainIncome;
            set => SetIncomeValue(ref _userMainIncome, value);
        }

        public string UserOtherIncome
        {
            get => _userOtherIncome;
            set => SetIncomeValue(ref _userOtherIncome, value);
        }

        public string PartnerMainIncome
        {
            get => _partnerMainIncome;
            set => SetIncomeValue(ref _partnerMainIncome, value);
        }

        public string PartnerOtherIncome
        {
            get => _partnerOtherIncome;
            set => SetIncomeValue(ref _partnerOtherIncome, value);
        }

        public Visibility PartnerDetailsVisibility
        {
            get { return _partnerDetailsVisibility; }
            set
            {
                _partnerDetailsVisibility = value;
                OnPropertyChanged(nameof(PartnerDetailsVisibility));
            }
        }
        private void SetIncomeValue(ref string field, string value)
        {
            if (IsValidInput(value))
            {
                field = value;
                OnPropertyChanged();
            }
            else
            {
                MessageBox.Show("Invalid input! Only numbers and a single decimal point are allowed.");
            }
        }

        public bool UserHasPartner
        {
            get { return _userHasPartner; }
            set
            {
                if (_userHasPartner != value)
                {
                    _userHasPartner = value;
                    OnPropertyChanged(nameof(UserHasPartner));
                    PartnerDetailsVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        private bool IsValidInput(string input)
        {
            
            return string.IsNullOrWhiteSpace(input) || System.Text.RegularExpressions.Regex.IsMatch(input, @"^[0-9,]*$");
        }
        public Brush TextBlockColor
        {
            get { return _textBlockColor; }
            set
            {
                if (_textBlockColor != value)
                {
                    _textBlockColor = value;
                    OnPropertyChanged(nameof(TextBlockColor)); 
                }
            }
        }

        public string WelcomeText
        {
            get { return WelcomeTextGenerator(); }
        }
        private void ExecuteDebugButtonCommand(object parameter)
        {
            Debug.WriteLine(UserSession.Username);
        }

        public string WelcomeTextGenerator()
        {
            string welcome = "Welcome ";

            if (_sessionService.CurrentSession != null)
            {
                welcome += _sessionService.CurrentSession.Username + "!\n" +
                           "This is your first time, please fill in some information about yourself";
            }
            return welcome;
        }
    }   
}

