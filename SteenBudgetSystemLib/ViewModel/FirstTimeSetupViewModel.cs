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
using SteenBudgetSystemLib.DataAccess;
using System.Text.RegularExpressions;

namespace SteenBudgetSystemLib.ViewModel
{
    public class FirstTimeSetupViewModel : INotifyPropertyChanged
    {
        private bool _userHasPartner = false;
        private string _salaryInput;
        private string _userMainIncome;
        private string _userOtherIncome;
        private string _partnerMainIncome;
        private string _partnerOtherIncome;
        private string _partnerName;
        private string _errorMessagePartnerName;

        private UserSession _userSession;
        public RelayCommand DebugButtonCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private Brush _textBlockColor = Brushes.Black;
        private Visibility _partnerDetailsVisibility = Visibility.Collapsed;
        private readonly ISessionService _sessionService;
        private readonly IDialogService _dialogService;
        public ICommand SetupDoneCommand { get; private set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserSession UserSession
        {
            get { return _userSession; }
            set { _userSession = value; }
        }
        public FirstTimeSetupViewModel(ISessionService sessionService, IDialogService dialogService)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            UserSession = _sessionService.CurrentSession;
            DebugButtonCommand = new RelayCommand(ExecuteDebugButtonCommand);
            TextBlockColor = Brushes.Red;
            SetupDoneCommand = new RelayCommand(ExecuteSetupDone);
            _dialogService = dialogService;
        }

        public string UserMainIncome
        {
            get => _userMainIncome;
            set
            {
                _userMainIncome = value;
                OnPropertyChanged(nameof(UserMainIncome));
                OnPropertyChanged(nameof(IsSetupDoneEnabled));
            }
        }


        public string UserOtherIncome
        {
            get => _userOtherIncome;
            set => SetIncomeValue(ref _userOtherIncome, value);
        }

        public string PartnerMainIncome
        {
            get => _partnerMainIncome;
            set
            {
                _partnerMainIncome = value;
                OnPropertyChanged(nameof(PartnerMainIncome));
                OnPropertyChanged(nameof(IsSetupDoneEnabled));
            }
        }

        public string PartnerOtherIncome
        {
            get => _partnerOtherIncome;
            set => SetIncomeValue(ref _partnerOtherIncome, value);
        }
        public string PartnerName
        {
            get => _partnerName;
            set
            {
                if (_partnerName != value)
                {
                    if (IsValidPartnerName(value))
                    {
                        _partnerName = value;
                        ErrorMessagePartnerName = string.Empty; // Clear any existing error message
                        OnPropertyChanged(nameof(PartnerName));
                    }
                    else
                    {
                        // If validation fails, set the error message and do not update _partnerName
                        // Error message is set within IsValidPartnerName method
                    }
                }
            }
        }

        public string ErrorMessagePartnerName
        {
            get => _errorMessagePartnerName;
            set
            {
                _errorMessagePartnerName = value;
                OnPropertyChanged(nameof(ErrorMessagePartnerName));
            }
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
                string error = "Invalid input! Only numbers and a single decimal point are allowed.";
                _dialogService.ShowMessage(error, "Validation Error");
            }
        }

        public bool UserHasPartner
        {
            get => _userHasPartner;
            set
            {
                if (_userHasPartner != value)
                {
                    _userHasPartner = value;
                    OnPropertyChanged(nameof(UserHasPartner));
                    OnPropertyChanged(nameof(IsSetupDoneEnabled));

                    PartnerDetailsVisibility = value ? Visibility.Visible : Visibility.Collapsed;

                    if (!value)
                    {
                        PartnerMainIncome = string.Empty;
                        PartnerOtherIncome = string.Empty;
                    }
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
        private bool IsValidPartnerName(string name)
        {
            string pattern = @"^\s*[A-Za-z]+(?: [A-Za-z]+)*\s*$";

            if (!Regex.IsMatch(name, pattern))
            {
                ErrorMessagePartnerName = "Name can only contain alphabetical characters and spaces.";
                _dialogService.ShowMessage(ErrorMessagePartnerName, "Validation Error");
                return false;
            }

            ErrorMessagePartnerName = string.Empty;
            return true;
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
                           "This is your first time, please fill in some information about yourself\n" +
                           "Fields marked with red asterix are mandatory";
            }
            return welcome;
        }
        private void ExecuteSetupDone(object parameter)
        {
            SqlExecutor sqlExecutor = new SqlExecutor();
            //User has partner, have to create partner table
            if(_userHasPartner)
            {
                sqlExecutor.CreatePartner(_userSession.Username, _partnerMainIncome, _partnerOtherIncome, _partnerName);
            }

        }
        public bool IsSetupDoneEnabled
        {
            get
            {
                if (_userHasPartner)
                {
                    return !string.IsNullOrWhiteSpace(UserMainIncome) && !string.IsNullOrWhiteSpace(PartnerMainIncome) && !string.IsNullOrEmpty(_partnerName);
                }
                else
                {
                    return !string.IsNullOrWhiteSpace(UserMainIncome);
                }
            }
        }

    }
}

