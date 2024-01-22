using SteenBudgetSystemLib.DataAccess;
using SteenBudgetSystemLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Services
{
    public class AuthenticationService
    {
        private readonly ISessionService _sessionService;
        private readonly IDialogService _dialogService;
        private readonly bool _debug;
        private SqlExecutor _sqlExecutor;

        public AuthenticationService(ISessionService sessionService, IDialogService dialogService, bool debug = false)
        {
            _sessionService = sessionService;
            _dialogService = dialogService;
            _debug = debug;
        }

        public (bool IsSuccess, string ErrorMessage) Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Email cannot be empty or contain whitespaces.");
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                return (false, "Password cannot be empty or contain whitespaces.");
            }

            User loggedInUser = ValidateLogin(email, password); // Implement your login validation logic here

            if (loggedInUser != null)
            {
                _sessionService.StartSession(loggedInUser);
                if (loggedInUser.FirstLogin)
                {
                    _dialogService.ShowWindow("FirstTime");
                }
                _dialogService.ShowWindow("MainWindow");
                return (true, "");
            }

            return (false, "Invalid email or password.");
        }

        private User ValidateLogin(string email, string password)
        {
            _sqlExecutor = new SqlExecutor();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null; // Invalid input
            }

            try
            {
                User user = _sqlExecutor.GetUserByEmail(email);
                if (user == null)
                {
                    return null; // User not found
                }

                string hashedInputPassword = PasswordHasher.HashPasswordWithSalt(password, Convert.FromBase64String(user.PasswordSalt));

                // Use constant-time comparison to compare hashed passwords
                bool isPasswordValid = SecurePasswordComparison(hashedInputPassword, user.Password);

                if (isPasswordValid)
                {
                    return user; // Successful authentication
                }
                else
                {
                    return null; // Invalid password
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle the error appropriately
                // For example, log the error and return null
                return null;
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
    }
}
