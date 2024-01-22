using SteenBudgetSystemLib.DataAccess;
using SteenBudgetSystemLib.ViewModel;
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
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        public CreateUser()
        {
            InitializeComponent();

            var viewModel = new CreateUserViewModel();
            DataContext = viewModel;  

            viewModel.RequestClose += Close; 
        }
        /// <summary>
        /// Handles the PasswordChanged event of the CreateUserPasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// This method updates the UserPassword property in the ViewModel whenever the user changes the password in the PasswordBox.
        /// Since PasswordBox does not support direct data binding for security reasons, this approach is used to transfer the password data to the ViewModel.
        /// </remarks>
        private void CreateUserPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).UserPassword = ((PasswordBox)sender).Password;
            }
        }
        private void CreateUserPasswordVerifyBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlExecutor sqlExecutor = new SqlExecutor();
            string username = sqlExecutor.getUser();
            MessageBox.Show(username);
        }
    }
}
