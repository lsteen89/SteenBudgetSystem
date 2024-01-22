using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using SteenBudgetSystemLib.Helpers;
using System.Windows;
using System.Text.RegularExpressions;
using SteenBudgetSystemLib.Models;
using SteenBudgetSystemLib.DataAccess;
using SteenBudgetSystemLib.Services;

namespace SteenBudgetSystemLib.ViewModel
{
    public class CreateUserViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _userPassword;
        private string _confirmPassword;
        private bool _passwordsMatch;
        private bool _isPasswordMatchInfoVisible;
        private string _validationMessage;
        private string _emailValidationMessage;
        private string _passwordValidationMessage;
        private string _firstNameValidationMessage;
        private User _user;

        
        private Visibility _emailValidationVisibility = Visibility.Collapsed;
        private Visibility _passwordValidationVisibility = Visibility.Collapsed;
        public ICommand CreateUserCommand { get; }

        public event Action RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        public CreateUserViewModel()
        {
            CreateUserCommand = new RelayCommand(ExecuteCreateUser, CanExecuteCreateUser);
        }

        private Visibility _validationMessageVisibility = Visibility.Collapsed;

        public string ValidationMessage
        {
            get => _validationMessage;
            set
            {
                _validationMessage = value;
                OnPropertyChanged(nameof(ValidationMessage));
            }
        }

        public Visibility ValidationMessageVisibility
        {
            get => _validationMessageVisibility;
            set
            {
                _validationMessageVisibility = value;
                OnPropertyChanged(nameof(ValidationMessageVisibility));
            }
        }
        public string PasswordValidationMessage
        {
            get { return _passwordValidationMessage; }
            set
            {
                _passwordValidationMessage = value;
                OnPropertyChanged(nameof(PasswordValidationMessage));
            }
        }
        public Visibility PasswordValidationVisibility
        {
            get { return _passwordValidationVisibility; }
            set
            {
                _passwordValidationVisibility = value;
                OnPropertyChanged(nameof(PasswordValidationVisibility));
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    ValidateField(_firstName, "FirstName");
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    ValidateField(_lastName, "LastName");
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                ValidateField(_email, "Email");
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                _userPassword = value;
                OnPropertyChanged(nameof(UserPassword));
                ValidatePassword(value);
                CommandManager.InvalidateRequerySuggested();

            }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                UpdatePasswordMatchStatus();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool PasswordsMatch
        {
            get { return _passwordsMatch; }
            set
            {
                _passwordsMatch = value;
                OnPropertyChanged(nameof(PasswordsMatch));
            }
        }
        public bool IsPasswordMatchInfoVisible
        {
            get { return _isPasswordMatchInfoVisible; }
            set
            {
                _isPasswordMatchInfoVisible = value;
                OnPropertyChanged(nameof(IsPasswordMatchInfoVisible));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void UpdatePasswordMatchStatus()
        {
            PasswordsMatch = string.Equals(UserPassword, ConfirmPassword);
            IsPasswordMatchInfoVisible = ConfirmPassword.Length > 0;
        }
        private bool CanExecuteCreateUser(object parameter)
        {
            return StringUtilities.IsValidName(FirstName) &&
                   StringUtilities.IsValidName(LastName) &&
                   StringUtilities.IsValidEmail(Email) &&
                   PasswordsMatch &&
                   PasswordValidationMessage.Equals("Password is valid");
        }
        
        public string FirstNameValidationMessage
        {
            get { return _firstNameValidationMessage; }
            set
            {
                _firstNameValidationMessage = value;
                OnPropertyChanged(nameof(FirstNameValidationMessage));
            }
        }

        private string _lastNameValidationMessage;
        public string LastNameValidationMessage
        {
            get { return _lastNameValidationMessage; }
            set
            {
                _lastNameValidationMessage = value;
                OnPropertyChanged(nameof(LastNameValidationMessage));
            }
        }
        public string EmailValidationMessage
        {
            get { return _emailValidationMessage; }
            set
            {
                _emailValidationMessage = value;
                OnPropertyChanged(nameof(EmailValidationMessage));
            }
        }
        public Visibility EmailValidationVisibility
        {
            get { return _emailValidationVisibility; }
            set
            {
                _emailValidationVisibility = value;
                OnPropertyChanged(nameof(EmailValidationVisibility));
            }
        }

        private Visibility _firstNameValidationVisibility = Visibility.Collapsed;
        public Visibility FirstNameValidationVisibility
        {
            get { return _firstNameValidationVisibility; }
            set
            {
                _firstNameValidationVisibility = value;
                OnPropertyChanged(nameof(FirstNameValidationVisibility));
            }
        }

        private Visibility _lastNameValidationVisibility = Visibility.Collapsed;
        public Visibility LastNameValidationVisibility
        {
            get { return _lastNameValidationVisibility; }
            set
            {
                _lastNameValidationVisibility = value;
                OnPropertyChanged(nameof(LastNameValidationVisibility));
            }
        }
        private void ValidateField(string fieldValue, string fieldName)
        {
            switch (fieldName)
            {
                case "FirstName":
                    FirstNameValidationMessage = StringUtilities.IsValidName(fieldValue) ?
                                                  "Valid First Name" : "Invalid characters in First Name";
                    FirstNameValidationVisibility = Visibility.Visible;
                    break;

                case "LastName":
                    LastNameValidationMessage = StringUtilities.IsValidName(fieldValue) ?
                                                "Valid Last Name" : "Invalid characters in Last Name";
                    LastNameValidationVisibility = Visibility.Visible;
                    break;

                case "Email":
                    EmailValidationMessage = StringUtilities.IsValidEmail(fieldValue) ?
                                             "Valid Email" : "Invalid Email";
                    EmailValidationVisibility = Visibility.Visible;
                    break;
            }
        }
        private void ValidatePassword(string password)
        {
            var validationResult = StringUtilities.ValidatePassword(password);

            PasswordValidationMessage = validationResult.ErrorMessage;
            PasswordValidationVisibility = Visibility.Visible;
        }
        private void ExecuteCreateUser(object parameter)
        {
            UserService userService = new UserService();

            bool isUserCreated = userService.CreateUser(FirstName, LastName, Email, ConfirmPassword);

            if (isUserCreated)
            {
                MessageBox.Show("User was created successfully!\nLoginname is your e-mail.\nPlease verify account via email before logging in");
                RequestClose?.Invoke();
            }
            else
            {
                MessageBox.Show("An error occurred during user creation.");
            }
        }
    }
}
