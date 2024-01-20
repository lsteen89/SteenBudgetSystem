using Microsoft.Extensions.DependencyInjection;
using SteenBudgetSystemLib.Services;
using SteenBudgetSystemLib.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SteenBudgetSystemUI.Views
{
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void ShowWindow(string windowName)
        {
            try
            {
                if (windowName == "CreateUser")
                {
                    var createUserWindow = _serviceProvider.GetService<CreateUser>();
                    createUserWindow?.Show();
                }
                else if (windowName == "MainWindow")
                {
                    var mainWindowViewModel = _serviceProvider.GetService<MainWindowViewModel>();
                    var mainWindow = new MainWindow(mainWindowViewModel);
                    mainWindow?.Show();
                }
                else if (windowName == "FirstTime")
                {
                    var FirstTimeSetupWindowviewModel = _serviceProvider.GetService<FirstTimeSetupViewModel>();
                    if (FirstTimeSetupWindowviewModel != null)
                    {
                        var FirstTimeSetupWindow = new FirstTimeSetup(FirstTimeSetupWindowviewModel);
                        FirstTimeSetupWindow.ShowDialog();
                    }
                    else
                    {
                        // Log an error here to track the issue
                        Debug.WriteLine("FirstTimeSetupViewModel instance not obtained from the service provider.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                MessageBox.Show($"Error opening window: {ex.Message}");
            }
        }

    }

}
