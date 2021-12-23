using BusinessManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace BusinessManagement.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var login = DataProvider.Instance.DB.AutoLogins.First();
            if (login.Checked == true)
            {
                autoLogin.IsChecked = true;
                AutoLogin(login.Username,login.Password);
            }
            
        }

        private void AutoLogin(string username, string codedPassword)
        {
            var checkACC = DataProvider.Instance.DB.Accounts.Where(x => x.Username == username && x.Password == codedPassword).Count();
            if (checkACC > 0)
            {
                HomeWindow homeWindow = new HomeWindow();
                CurrentAccount.Instance.ConvertAccToCurrentAcc(username);
                this.Hide();

                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = Converter.Instance.ConvertByteToBitmapImage(CurrentAccount.Image);
                homeWindow.grdAcc_Image.Background = imageBrush;
                homeWindow.menu_Acc_DisplayName.Header = CurrentAccount.DisplayName;

                if (homeWindow.grdAcc_Image.Children.Count != 0)
                {
                    homeWindow.grdAcc_Image.Children.Remove(homeWindow.grdAcc_Image.Children[0]);
                }

                homeWindow.ShowDialog();

                autoLogin.IsChecked = false;
                txtPassword.Password = "";
                this.Show();
            }
        }

        private void autoLogin_Unchecked(object sender, RoutedEventArgs e)
        {
            var login = DataProvider.Instance.DB.AutoLogins.First();

            login.Checked = false;
            login.Username = "";
            login.Password = "";

            DataProvider.Instance.DB.SaveChanges();
        }
    }
}
