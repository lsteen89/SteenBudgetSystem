using SteenBudgetSystemLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemUI.Views
{
    public class DialogService : IDialogService
    {
        public void ShowWindow(string windowName)
        {
            switch (windowName)
            {
                case "CreateUser":
                    new CreateUser().Show();
                    break;
                case "MainWindow":
                    new MainWindow().Show();
                    break;
            }
        }
    }
}
