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
        string workingDirectory = Environment.CurrentDirectory;
        string codedPassword;
        public LoginWindow()
        {
            InitializeComponent();
            string cache = workingDirectory + "/cache.alg";
            if (File.Exists(cache))
            {
                string[] autoL = File.ReadAllLines(workingDirectory + "/cache.alg");
                txtUser.Text = autoL[1];
                if (autoL[0] == "True")
                {
                    autoLogin.IsChecked = true;
                    codedPassword = autoL[2];
                    AutoLogin();
                }
            }
            
        }

        private void AutoLogin()
        {
            var checkACC = DataProvider.Instance.DB.Accounts.Where(x => x.Username == txtUser.Text && x.Password == codedPassword).Count();
            if (checkACC > 0)
            {
                HomeWindow homeWindow = new HomeWindow();
                CurrentAccount.Instance.ConvertAccToCurrentAcc(txtUser.Text);
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
            string[] autoL = new string[3];
            autoL[0] = "False";
            autoL[1] = txtUser.Text;
            autoL[2] = "";

            File.WriteAllLines("cache.alg", autoL);
        }
    }
}
