using SteenBudgetSystemLib.ViewModel;
using SteenBudgetSystemUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SteenBudgetSystemUI
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(new DialogService());
        }

        private void LoginPageLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LoginViewModel;
            if (viewModel != null)
            {
                viewModel.Password = LoginPagePasswordBox.Password;
                viewModel.LoginCommand.Execute(null);
            }
        }
    }
}
