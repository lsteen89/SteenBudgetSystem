using SteenBudgetSystemLib.Helpers;
using SteenBudgetSystemLib.Models;
using SteenBudgetSystemLib.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.ViewModel
{
    public class MainWindowViewModel
    {
        private readonly ISessionService _sessionService;


        private UserSession _userSession;

        public RelayCommand DebugButtonCommand { get; private set; }

        public UserSession UserSession
        {
            get { return _userSession; }
            set
            {
                _userSession = value;
                
            }
        }

        public MainWindowViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
            UserSession = _sessionService.CurrentSession;
            DebugButtonCommand = new RelayCommand(param => ExecuteDebugButton());
        }

        private void ExecuteDebugButton()
        {
            
            Debug.WriteLine(UserSession.Username);
        }
    }

}
