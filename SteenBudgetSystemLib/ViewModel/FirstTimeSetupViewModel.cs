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

        private UserSession _userSession;
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
            if (IsValidInput(value))
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
        private bool IsValidInput(string input)
        {
            // Allow numbers, spaces, and commas anywhere in the string
            return string.IsNullOrWhiteSpace(input) || Regex.IsMatch(input, @"^[\d\s,]*$");
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
            if (string.IsNullOrEmpty(name))
            {
                ErrorMessagePartnerName = string.Empty;
                return true;
            }
            string pattern = @"^\s*\p{L}+(?:\s+\p{L}+)*\s*$";

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
        public string UserPartnerRatio
        {
            get
            {
                decimal userMainIncome = ConvertToDecimal(UserMainIncome);
                decimal userOtherIncome = ConvertToDecimal(UserOtherIncome);
                decimal partnerMainIncome = ConvertToDecimal(PartnerMainIncome);
                decimal partnerOtherIncome = ConvertToDecimal(PartnerOtherIncome);

                decimal totalIncome = userMainIncome + userOtherIncome + partnerMainIncome + partnerOtherIncome;

                if (totalIncome > 0)
                {
                    decimal userTotalIncome = userMainIncome + userOtherIncome;
                    decimal ratio = userTotalIncome / totalIncome * 100;
                    UserRatio = ratio;
                    UpdatePieChartData(userTotalIncome, totalIncome);
                    return $"User makes {ratio:0.##}% of the total income.";
                }
                else
                {
                    return "Income details not available.";
                }
            }
        }
        private decimal ConvertToDecimal(string incomeString)
        {
            if (decimal.TryParse(incomeString, out decimal result))
            {
                return result;
            }
            return 0;
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
        public string RatioText
        {
            get
            {
                if (IsRatioConfirmed)
                {
                    return "Using calculated ratio";
                }
                else
                {
                    int xx = (int)SliderValue;
                    int yy = 100 - xx;
                    return $"Other ratio: {xx:00}/{yy:00}";
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

        private void CalculateAndSetUserRatio()
        {
            decimal userMainIncome = ConvertToDecimal(UserMainIncome);
            decimal userOtherIncome = ConvertToDecimal(UserOtherIncome);
            decimal partnerMainIncome = ConvertToDecimal(PartnerMainIncome);
            decimal partnerOtherIncome = ConvertToDecimal(PartnerOtherIncome);

            decimal totalIncome = userMainIncome + userOtherIncome + partnerMainIncome + partnerOtherIncome;

            if (totalIncome > 0)
            {
                decimal userTotalIncome = userMainIncome + userOtherIncome;
                UserRatio = userTotalIncome / totalIncome * 100;
            }
            else
            {
                UserRatio = 0; // Or handle this case as appropriate
            }
        }
        public bool IsControlsEnabled => !IsRatioConfirmed;
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
            _userMainIncome = RemoveCommas(_userMainIncome);
            _userOtherIncome = RemoveCommas(_userOtherIncome);
            _partnerMainIncome = RemoveCommas(_partnerMainIncome);
            _partnerOtherIncome = RemoveCommas(_partnerOtherIncome);
            SqlExecutor sqlExecutor = new SqlExecutor();
            //User has partner, have to create partner table
            if(_userHasPartner)
            {
                sqlExecutor.CreatePartner(_userSession.Username, _partnerMainIncome, _partnerOtherIncome, _partnerName, _userRatio, _userSession.FirstLogin);
            }

        }
        private string RemoveCommas(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // Remove commas from the start and end of the string
            input = Regex.Replace(input, @"^,*|,*$", "");

            // Replace two or more consecutive commas with a single comma
            input = Regex.Replace(input, @",{2,}", ",");

            // Keep only the first comma and remove the rest
            int firstCommaIndex = input.IndexOf(',');
            if (firstCommaIndex != -1)
            {
                input = input.Substring(0, firstCommaIndex + 1) + input.Substring(firstCommaIndex + 1).Replace(",", "");
            }

            return input;
        }

    }
}

