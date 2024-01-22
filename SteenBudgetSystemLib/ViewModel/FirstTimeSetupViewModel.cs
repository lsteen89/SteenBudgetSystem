using SteenBudgetSystemLib.Helpers;
using SteenBudgetSystemLib.Models;
using SteenBudgetSystemLib.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Wpf;
using LiveCharts;

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
        private double _sliderValue;
        private decimal _userRatio;
        private bool _isRatioConfirmed = true; // Checkbox starts as checked
        public bool IsControlsEnabled => !IsRatioConfirmed;
        public string RatioText => _financialCalculator.GetRatioText(SliderValue, IsRatioConfirmed);

        private UserSession _userSession;
        private readonly FinancialCalculator _financialCalculator = new FinancialCalculator();
        public RelayCommand DebugButtonCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private Brush _textBlockColor = Brushes.Black;
        private Visibility _partnerDetailsVisibility = Visibility.Collapsed;
        private readonly ISessionService _sessionService;
        private readonly IDialogService _dialogService;
        public ICommand SetupDoneCommand { get; private set; }
        public SeriesCollection FirstTimeSetupRatioPieChartData { get; set; }

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
        public string UserPartnerRatio
        {
            get
            {
                return GetFormattedUserPartnerRatio();
            }
        }

        public string UserMainIncome
        {
            get => _userMainIncome;
            set
            {
                if (SetIncomeValue(ref _userMainIncome, value, nameof(UserMainIncome)))
                {
                    OnPropertyChanged(nameof(IsSetupDoneEnabled));
                    OnPropertyChanged(nameof(UserPartnerRatio));
                }
            }
        }


        public string UserOtherIncome
        {
            get => _userOtherIncome;
            set
            {
                if (SetIncomeValue(ref _userOtherIncome, value, nameof(UserOtherIncome)))
                {
                    OnPropertyChanged(nameof(IsSetupDoneEnabled));
                    OnPropertyChanged(nameof(UserPartnerRatio));
                }
            }

        }

        public string PartnerMainIncome
        {
            get => _partnerMainIncome;
            set
            {
                if (SetIncomeValue(ref _partnerMainIncome, value, nameof(PartnerMainIncome)))
                {
                    OnPropertyChanged(nameof(IsSetupDoneEnabled));
                    OnPropertyChanged(nameof(UserPartnerRatio));
                }
            }
        }

        public string PartnerOtherIncome
        {
            get => _partnerOtherIncome;
            set
            {
                if (SetIncomeValue(ref _partnerOtherIncome, value, nameof(PartnerOtherIncome)))
                {
                    OnPropertyChanged(nameof(IsSetupDoneEnabled));
                    OnPropertyChanged(nameof(UserPartnerRatio));
                }
            }
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
                        ErrorMessagePartnerName = string.Empty; 
                        OnPropertyChanged(nameof(PartnerName));
                    }
                    else
                    {
                        // If validation fails, set the error message and do not update _partnerName
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
        public decimal UserRatio
        {
            get => _userRatio;
            set
            {
                _userRatio = value;
                OnPropertyChanged(nameof(UserRatio));
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
        private bool SetIncomeValue(ref string field, string value, string propertyName)
        {
            if (SteenBudgetSystemLib.Helpers.StringUtilities.IsDecimalFieldValidInput(value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            else
            {
                string error = "Invalid input! Only numbers and a single decimal point are allowed.";
                _dialogService.ShowMessage(error, "Validation Error");
                return false;
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
            var validationResult = StringUtilities.ValidatePartnerName(name);
            if (!validationResult.IsValid)
            {
                ErrorMessagePartnerName = validationResult.ErrorMessage;
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
            Debug.WriteLine(UserRatio);
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
        private void CalculateAndSetUserRatio()
        {
            FinancialCalculator calculator = new FinancialCalculator();

            decimal userMainIncome = SteenBudgetSystemLib.Helpers.ConverterHelpClass.ConvertToDecimal(UserMainIncome);
            decimal userOtherIncome = SteenBudgetSystemLib.Helpers.ConverterHelpClass.ConvertToDecimal(UserOtherIncome);
            decimal partnerMainIncome = SteenBudgetSystemLib.Helpers.ConverterHelpClass.ConvertToDecimal(PartnerMainIncome);
            decimal partnerOtherIncome = SteenBudgetSystemLib.Helpers.ConverterHelpClass.ConvertToDecimal(PartnerOtherIncome);

            UserRatio = calculator.CalculateUserRatio(userMainIncome, userOtherIncome, partnerMainIncome, partnerOtherIncome);
            UpdatePieChartData(userMainIncome + userOtherIncome, userMainIncome + userOtherIncome + partnerMainIncome + partnerOtherIncome);
        }

        private void UpdatePieChartData(decimal userIncome, decimal totalIncome)
        {
            decimal partnerIncome = totalIncome - userIncome;
            FirstTimeSetupRatioPieChartData = new SeriesCollection
        {
        new PieSeries
        {
            Title = "User Income",
            Values = new ChartValues<decimal> { userIncome },
            DataLabels = true
        },
        new PieSeries
        {
            Title = "Partner Income",
            Values = new ChartValues<decimal> { partnerIncome },
            DataLabels = true
        }
        };

            OnPropertyChanged(nameof(FirstTimeSetupRatioPieChartData));
        }
        public double SliderValue
        {
            get => _sliderValue;
            set
            {
                if (_sliderValue != value)
                {
                    _sliderValue = value;
                    OnPropertyChanged(nameof(SliderValue));
                    OnPropertyChanged(nameof(RatioText));

                    if (!IsRatioConfirmed)
                    {
                        UserRatio = (int)value;
                    }
                }
            }
        }

        public bool IsRatioConfirmed
        {
            get => _isRatioConfirmed;
            set
            {
                if (_isRatioConfirmed != value)
                {
                    _isRatioConfirmed = value;
                    OnPropertyChanged(nameof(IsRatioConfirmed));
                    OnPropertyChanged(nameof(IsControlsEnabled));
                    OnPropertyChanged(nameof(RatioText));

                    if (!value)
                    {
                        // When the checkbox is unchecked, use the slider value as the ratio
                        int xx = (int)SliderValue;
                        UserRatio = xx;
                    }
                    else
                    {
                        // When the checkbox is checked, calculate and update the ratio based on income
                        CalculateAndSetUserRatio();
                    }
                }
            }
        }
        public string GetFormattedUserPartnerRatio()
        {
            CalculateAndSetUserRatio(); // Call the common method

            if (UserRatio > 0)
            {
                return $"User makes {UserRatio:0.##}% of the total income.";
            }
            else
            {
                return "Income details not available.";
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
        private void ExecuteSetupDone(object parameter)
        {
            //Todo: Continue this method!!
            UserService userService = new UserService();

            if (_userHasPartner)
            {
                bool result = userService.CreateOrUpdatePartner(_userSession.Username, _partnerMainIncome, _partnerOtherIncome, _partnerName, _userRatio, _userSession.FirstLogin);
            }

           
        }
    }
}

