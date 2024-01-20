using SteenBudgetSystemLib.Models;
using SteenBudgetSystemLib.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Services
{
    public class SessionService : ISessionService
    {
        public UserSession CurrentSession { get; private set; }
        public FirstTimeSetupViewModel FirstTimeSetupModel { get; private set; }

        public void StartSession(User user)
        {
            CurrentSession = new UserSession { Username = user.Email, Roles = user.Roles, FirstLogin = user.FirstLogin };
            // Initialize other session data
        }

        public void EndSession()
        {
            CurrentSession = null;
        }
        public FirstTimeSetupViewModel GetFirstTimeSetupViewModel()
        {
            return FirstTimeSetupModel;
        }
        // Additional implementation
    }
}
