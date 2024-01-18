using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using SteenBudgetSystemUI.Views;
using SteenBudgetSystemLib.Services;
using SteenBudgetSystemLib.ViewModel;


namespace SteenBudgetSystemUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create and show the login page
            LoginPage loginPage = new LoginPage();
            bool? dialogResult = loginPage.ShowDialog();

            // Check the dialog result to see if login was successful
            if (dialogResult == true)
            {
                // If login successful, create and show the main window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                // If login failed or was canceled, shut down the application
                this.Shutdown();
            }
        }
        public App()
        {
            ConfigureServices();

            // Create the main window and set its DataContext
            MainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<LoginViewModel>()
            };
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register your services and view models here
            services.AddTransient<LoginViewModel>();
            services.AddTransient<IDialogService, DialogService>();

            // Build the service provider
            _serviceProvider = services.BuildServiceProvider();
        }
    }


}
