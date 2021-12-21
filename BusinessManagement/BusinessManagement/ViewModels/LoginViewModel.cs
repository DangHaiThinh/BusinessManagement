using BusinessManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using System.Windows.Input;
using BusinessManagement.Views;
using System.Windows.Controls;
using System.Windows.Media;
using System.Security.Cryptography;
using System.Windows;
using System.IO;

namespace BusinessManagement.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }
        
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<LoginWindow>((p) => { return true; }, (p) => { Login(p); });
        }


        void Login(LoginWindow parameter)
        {
            if (parameter == null)
            {
                return;
            }
            //check username
            if (String.IsNullOrEmpty(parameter.txtUser.Text))
            {
                CustomMessageBox.Show("Hãy nhập tên tài khoản!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtUser.Focus();
                return;
            }
            //check password
            if (String.IsNullOrEmpty(parameter.txtPassword.Password))
            {
                CustomMessageBox.Show("Hãy nhập mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPassword.Focus();
                return;
            }

            string codedPassword = SHA512Hash(parameter.txtPassword.Password);
            var checkACC = DataProvider.Instance.DB.Accounts.Where(x => x.Username == parameter.txtUser.Text && x.Password == codedPassword).Count();
            if (checkACC > 0)
            {
                CurrentAccount.Instance.ConvertAccToCurrentAcc(parameter.txtUser.Text);
                if (CurrentAccount.Ban == true)
                {
                    CustomMessageBox.Show("Tài khoản đã bị khóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                HomeWindow homeWindow = new HomeWindow();
                parameter.Hide();

                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = Converter.Instance.ConvertByteToBitmapImage(CurrentAccount.Image);
                homeWindow.grdAcc_Image.Background = imageBrush;
                homeWindow.menu_Acc_DisplayName.Header = CurrentAccount.DisplayName;


                if (homeWindow.grdAcc_Image.Children.Count != 0)
                {
                    homeWindow.grdAcc_Image.Children.Remove(homeWindow.grdAcc_Image.Children[0]);
                }

                //add auto login
                string[] autoLogin = new string[3];
                if (parameter.autoLogin.IsChecked == true)
                {
                    autoLogin[0] = "True";
                    autoLogin[1] = parameter.txtUser.Text;
                    autoLogin[2] = codedPassword;

                    File.WriteAllLines("cache.alg", autoLogin);
                }
                
                homeWindow.ShowDialog();

                parameter.autoLogin.IsChecked = false;
                parameter.txtPassword.Password = "";
                parameter.Show();
            }
            else
            {
                CustomMessageBox.Show("Tài khoản hoặc mật khẩu sai!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
