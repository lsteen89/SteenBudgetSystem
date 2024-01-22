using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using SteenBudgetSystemUI.Views;
using SteenBudgetSystemLib.Services;
using SteenBudgetSystemLib.ViewModel;
using SteenBudgetSystemLib.Models;


namespace SteenBudgetSystemUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<AuthenticationService>();

            // Register ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<FirstTimeSetupViewModel>();

            //Pages
            services.AddTransient<LoginPage>();
            services.AddTransient<CreateUser>();
            services.AddTransient<MainWindow>();
            services.AddSingleton<FirstTimeSetup>();
            

            // Build the service provider
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Resolve the LoginViewModel
            var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();

            // Create the login page and set its DataContext
            var loginPage = _serviceProvider.GetRequiredService<LoginPage>();

            // Show the login page
            bool? dialogResult = loginPage.ShowDialog();

            // Proceed based on the dialog result
            if (dialogResult == true)
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }



}
