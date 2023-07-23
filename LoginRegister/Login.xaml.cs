using ProDiaryApplication.LoginRegister;
using ProDiaryApplication.Models;
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

namespace ProDiaryApplication
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnShowReset_Click(object sender, RoutedEventArgs e)
        {
            ForgetPassWord forgetPage = new ForgetPassWord();
            forgetPage.Show();
        }

        private void btnShowRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            Register register = new Register();
            register.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (DiaryNoteContext context = new DiaryNoteContext())
            {
                string username = txtUser.Text;
                string password = txtPassword.Password;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter all required information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Account account = context.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);
                if (account == null)
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MainWindow mainWindow = new MainWindow();
                mainWindow.CurrentUser = account;
                mainWindow.Show();
                this.Hide();
            }
        }
    }
}
