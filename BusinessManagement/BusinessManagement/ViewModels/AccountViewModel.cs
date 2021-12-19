using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessManagement.Models;
using BusinessManagement.Views;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessManagement.Resources.UserControls;
using Microsoft.Win32;
using System.Windows;
using System.Data.Entity.Migrations;

namespace BusinessManagement.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        //private bool isExisted;
        public HomeWindow HomeWindow { get; set; }
        private string imageFileName;
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand UpdateAccountCommand { get; set; }
        public ICommand LoadAccountOnWindowCommand { get; set; }
        //public ICommand DisplaynameCheckerCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ChooseImgAccountCommand { get; set; }
        //change password window
        public ICommand ChangePassword_SaveCommand { get; set; }
        public ICommand Close_ChangePasswordWindowCommand { get; set; }
        //info account window
        public ICommand InfoAcc_SaveCommand { get; set; }
        public ICommand InfoAcc_CloseWindowCommand { get; set; }
        //home window
        public ICommand ShowProfileCommand { get; set; }
        public ICommand ShowChangePasswordCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand ShutDownCommand { get; set; }

        public ICommand InitCommand { get; set; }
        public ICommand CreateAccountCommand { get; set; }
        public ICommand EditAccountCommand { get; set; }
        public ICommand AccountManagement_SaveCommand { get; set; }
        public ICommand AccountManagement_CloseCommand { get; set; }
        public ICommand SearchAccountCommand { get; set; }
        

        public AccountViewModel()
        {
            UpdateAccountCommand = new RelayCommand<HomeWindow>((para) => true, (para) => UpdateAccount(para));
            //DisplaynameCheckerCommand = new RelayCommand<HomeWindow>((para) => true, para => NameChecker(para));
            ChangePasswordCommand = new RelayCommand<HomeWindow>((para) => true, para => Account_ChangePassword(para));
            LoadAccountOnWindowCommand = new RelayCommand<HomeWindow>((para) => true, (para) => LoadAccount(para));
            ChooseImgAccountCommand = new RelayCommand<Grid>(para => true, para => ChooseImg(para));
            //change password window
            ChangePassword_SaveCommand = new RelayCommand<ChangePasswordWindow>((para) => true, para => ChangePassword(para));
            Close_ChangePasswordWindowCommand = new RelayCommand<ChangePasswordWindow>((para) => true, para => CloseChangePasswordWindow(para));
            //info account window
            InfoAcc_SaveCommand = new RelayCommand<InfoAccountWindow>((para) => true, para => InfoAcc_Save(para));
            InfoAcc_CloseWindowCommand = new RelayCommand<InfoAccountWindow>((para) => true, para => InfoAcc_CloseWindow(para));
            //home window
            ShowProfileCommand = new RelayCommand<HomeWindow>((para) => true, para => ShowProfileAccountWindow(para));
            ShowChangePasswordCommand = new RelayCommand<HomeWindow>((para) => true, para => ShowChangePasswordWindow(para));
            LogOutCommand = new RelayCommand<HomeWindow>((para) => true, para => LogOut(para));
            ShutDownCommand = new RelayCommand<HomeWindow>((para) => true, para => ShutDown(para));

            InitCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Init(para));
            CreateAccountCommand = new RelayCommand<HomeWindow>((para) => true, (para) => CreateAccount(para));
            EditAccountCommand = new RelayCommand<AccountControlUC>((para) => true, para => ShowEditAccountWindow(para));
            AccountManagement_SaveCommand = new RelayCommand<AccountManagementWindow>((para) => true, para => AccountMan_Save(para));
            AccountManagement_CloseCommand = new RelayCommand<AccountManagementWindow>((para) => true, para => AccountMan_CloseWindow(para));
            SearchAccountCommand = new RelayCommand<HomeWindow>((para) => true, (para) => SearchAccount(para));
            DeleteAccountCommand = new RelayCommand<AccountControlUC>((para) => true, (para) => DeleteAccount(para));

        }

        //home window
        private void LogOut(HomeWindow para)
        {
            para.Close();
        }
        private void ShutDown(HomeWindow para)
        {
            Environment.Exit(0);
        }

        private void ShowChangePasswordWindow(HomeWindow para)
        {
            this.HomeWindow = para;
            ChangePasswordWindow window = new ChangePasswordWindow();
            window.ShowDialog();
        }
        private void ShowProfileAccountWindow(HomeWindow para)
        {
            this.HomeWindow = para;
            InfoAccountWindow window = new InfoAccountWindow();
            ImageBrush imageBrush = new ImageBrush();

            window.txtUsername.Text = CurrentAccount.Username;
            window.txtDisplayName.Text = CurrentAccount.DisplayName;
            window.txtDisplayName.SelectionStart = window.txtDisplayName.Text.Length;

            window.txtLocation.Text = CurrentAccount.Location;
            window.txtLocation.SelectionStart = window.txtLocation.Text.Length;

            window.txtPhoneNumber.Text = CurrentAccount.PhoneNumber;
            window.txtPhoneNumber.SelectionStart = window.txtPhoneNumber.Text.Length;

            imageBrush.ImageSource = Converter.Instance.ConvertByteToBitmapImage(CurrentAccount.Image);
            window.grdImage.Background = imageBrush;

            if (window.grdImage.Children.Count != 0)
            {
                window.grdImage.Children.Remove(window.grdImage.Children[0]);
                window.grdImage.Children.Remove(window.grdImage.Children[0]);
            }

            window.ShowDialog();
            if (window.isSucceed)
            {
                imageBrush.ImageSource = Converter.Instance.ConvertByteToBitmapImage(CurrentAccount.Image);
                this.HomeWindow.grdAcc_Image.Background = imageBrush;
                this.HomeWindow.menu_Acc_DisplayName.Header = CurrentAccount.DisplayName;
            }
        }
        private void Init(HomeWindow para)
        {
            this.HomeWindow = para;
            this.HomeWindow.stkAccount.Children.Clear();
            List<Account> accounts = new List<Account>();
            accounts = DataProvider.Instance.DB.Accounts.ToList<Account>();
            foreach (Account account in accounts)
            {
                AccountControlUC accountUC = new AccountControlUC();
                accountUC.txtAccount.Text = account.Username.ToString();
                accountUC.textName.Text = account.DisplayName.ToString();
                if(account.Location != null)
                {
                    accountUC.txtAddress.Text = account.Location.ToString();
                }
                if(account.PhoneNumber != null)
                {
                    accountUC.txtPhone.Text = account.PhoneNumber.ToString();
                }
                this.HomeWindow.stkAccount.Children.Add(accountUC);
            }
            this.HomeWindow.ScrollAccount.Visibility = System.Windows.Visibility.Visible;
            this.HomeWindow.stkAccount.Visibility = System.Windows.Visibility.Visible;
        }
        private void SearchAccount(HomeWindow para)
        {
            this.HomeWindow = para;
            foreach (AccountControlUC control in HomeWindow.stkAccount.Children)
            {
                if (control.txtAccount.Text.ToLower().Contains(this.HomeWindow.txtSearchAccount.Text))
                {
                    control.Visibility = Visibility.Visible;
                }
                else if(control.textName.Text.ToLower().Contains(this.HomeWindow.txtSearchAccount.Text))
                {
                    control.Visibility = Visibility.Visible;
                }
                else if(control.txtPhone.Text.ToLower().Contains(this.HomeWindow.txtSearchAccount.Text))
                {
                    control.Visibility = Visibility.Visible;
                }
                else
                {
                    control.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void CreateAccount(HomeWindow para)
        {
            SignUpWindow window = new SignUpWindow();

            window.ShowDialog();
        }
        private void ShowEditAccountWindow(AccountControlUC para)
        {
            AccountManagementWindow window = new AccountManagementWindow();

            Account account = new Account();
            string username = para.txtAccount.Text;
            account = (Account)DataProvider.Instance.DB.Accounts.Where(x => x.Username == username).First();

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = Converter.Instance.ConvertByteToBitmapImage(account.Image);
            window.grdImage.Background = imageBrush;
            window.EImage.Visibility = Visibility.Hidden;
            window.IImage.Visibility = Visibility.Hidden;

            window.txtUsername.Text = account.Username;
            window.txtDisplayName.Text = account.DisplayName;
            if(account.Location != null)
            {
                window.txtLocation.Text = account.Location;
            }
            if (account.PhoneNumber != null)
            {
                window.txtPhoneNumber.Text = account.PhoneNumber;
            }
            if(window.txtUsername.Text == "admin")
            {
                window.chk_agency.IsEnabled = false;
                window.chk_product.IsEnabled = false;
                window.chk_business.IsEnabled = false;
                window.chk_receipt.IsEnabled = false;
                window.chk_report.IsEnabled = false;
                window.chk_account_management.IsEnabled = false;
                window.chk_setting.IsEnabled = false;
                window.chk_ban.IsEnabled = false;
            }

            //Role checkboxes
            if (account.Role.Contains("1"))
            {
                window.chk_agency.IsChecked = true;
            }
            if (account.Role.Contains("2"))
            {
                window.chk_product.IsChecked = true;
            }
            if (account.Role.Contains("3"))
            {
                window.chk_business.IsChecked = true;
            }
            if (account.Role.Contains("4"))
            {
                window.chk_receipt.IsChecked = true;
            }
            if (account.Role.Contains("5"))
            {
                window.chk_report.IsChecked = true;
            }
            if (account.Role.Contains("6"))
            {
                window.chk_account_management.IsChecked = true;
            }
            if (account.Role.Contains("7"))
            {
                window.chk_setting.IsChecked = true;
            }
            if (account.Ban == true)
            {
                window.chk_ban.IsChecked = true;
            }

            window.ShowDialog();
        }
        private void AccountMan_Save(AccountManagementWindow para)
        {
            string username = CurrentAccount.Username;
            if (para.txtUsername.Text == username && para.chk_ban.IsChecked == true)
            {
                CustomMessageBox.Show("Không thể tự khóa tài khoản của bản thân!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                para.chk_ban.IsChecked = false;
                return;
            }
            if (para.chk_agency.IsChecked != true && para.chk_product.IsChecked != true && para.chk_business.IsChecked != true && para.chk_receipt.IsChecked != true && para.chk_report.IsChecked != true && para.chk_account_management.IsChecked != true && para.chk_setting.IsChecked != true)
            {
                CustomMessageBox.Show("Tài khoản chưa có quyền hạn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Account account = new Account();
            account = (Account)DataProvider.Instance.DB.Accounts.Where(x => x.Username == username).First();

            string role = "";
            bool ban = false;
            if(para.chk_agency.IsChecked == true)
            {
                role += "1";
            }
            if (para.chk_product.IsChecked == true)
            {
                role += "2";
            }
            if (para.chk_business.IsChecked == true)
            {
                role += "3";
            }
            if (para.chk_receipt.IsChecked == true)
            {
                role += "4";
            }
            if (para.chk_report.IsChecked == true)
            {
                role += "5";
            }
            if (para.chk_account_management.IsChecked == true)
            {
                role += "6";
            }
            if (para.chk_setting.IsChecked == true)
            {
                role += "7";
            }
            if (para.chk_ban.IsChecked == true)
            {
                ban = true;
            }

            account.Role = role;
            account.Ban = ban;
            DataProvider.Instance.DB.SaveChanges();

            para.Close();
        }
        private void AccountMan_CloseWindow(AccountManagementWindow para)
        {
            para.Close();
        }
        private void DeleteAccount(AccountControlUC para)
        {
            MessageBoxResult res = CustomMessageBox.Show("Bạn có chắc không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                Account acc = new Account();
                string username = para.txtAccount.Text;
                acc = (Account)DataProvider.Instance.DB.Accounts.Where(x => x.Username == username).First();
                DataProvider.Instance.DB.Accounts.Remove(acc);
                DataProvider.Instance.DB.SaveChanges();

                this.HomeWindow.stkAccount.Children.Remove(para);
            }
        }
        //info account window
        private void InfoAcc_Save(InfoAccountWindow para)
        {
            if (string.IsNullOrEmpty(para.txtDisplayName.Text))
            {
                para.txtDisplayName.Focus();
                para.txtDisplayName.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(para.txtLocation.Text))
            {
                para.txtLocation.Focus();
                para.txtLocation.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(para.txtPhoneNumber.Text))
            {
                para.txtPhoneNumber.Focus();
                para.txtPhoneNumber.Text = "";
                return;
            }

            string username = para.txtUsername.Text;
            string displayname = para.txtDisplayName.Text;
            string location = para.txtLocation.Text;
            string phonenumber = para.txtPhoneNumber.Text;
            byte[] imgByteArr;

            if (phonenumber.Length != 10)
            {
                CustomMessageBox.Show("Số điện thoại không đúng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (imageFileName == null)
            {
                imgByteArr = CurrentAccount.Image;
                //imgByteArr = Converter.Instance.ConvertImageToBytes(@"..\..\Resources\Images\default.jpg");
            }
            else
            {
                imgByteArr = Converter.Instance.ConvertImageToBytes(imageFileName);
            }
            try
            {
                Account account = DataProvider.Instance.DB.Accounts.SingleOrDefault(p => p.Username == username);
                if (account != null)
                {
                    account.DisplayName = displayname;
                    account.Location = location;
                    account.PhoneNumber = phonenumber;
                    account.Image = imgByteArr;
                    DataProvider.Instance.DB.SaveChanges();

                    CurrentAccount.Instance.ConvertAccToCurrentAcc(account);
                    para.isSucceed = true;
                }
            }
            catch (Exception ex)
            {
                para.isSucceed = false;
                CustomMessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            para.Close();
        }
        private void InfoAcc_CloseWindow(InfoAccountWindow para)
        {
            para.Close();
        }
        //change password window
        private void ChangePassword(ChangePasswordWindow para)
        {
            if (string.IsNullOrEmpty(para.txtUsername.Text))
            {
                CustomMessageBox.Show("Hãy nhập tên tài khoản!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                para.txtUsername.Focus();
                return;
            }
            if (string.IsNullOrEmpty(para.pwbPassword.Password))
            {
                CustomMessageBox.Show("Hãy nhập mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                para.pwbPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(para.pwbNewPassword.Password))
            {
                CustomMessageBox.Show("Hãy nhập mật khẩu mới!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                para.pwbNewPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(para.pwbConfirmNewPassword.Password))
            {
                CustomMessageBox.Show("Hãy nhập lại mật khẩu mới!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                para.pwbConfirmNewPassword.Focus();
                return;
            }

            if (para.pwbNewPassword.Password != para.pwbConfirmNewPassword.Password)
            {
                CustomMessageBox.Show("Mật khẩu không trùng khớp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                para.pwbConfirmNewPassword.Focus();
                return;
            }

            string username = para.txtUsername.Text;
            string password = MD5Hash(para.pwbPassword.Password);

            var checkAcc = DataProvider.Instance.DB.Accounts.Where(p => p.Username == username && p.Password == password).Count();
            if(checkAcc <= 0)
            {
                CustomMessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                para.pwbPassword.Focus();
                return;
            }
            else
            {
                Account account = DataProvider.Instance.DB.Accounts.SingleOrDefault(p => p.Username == username);
                string newPassword = MD5Hash(para.pwbNewPassword.Password);
                if (account != null)
                {
                    account.Password = newPassword;
                    DataProvider.Instance.DB.SaveChanges();

                    CurrentAccount.Instance.ConvertAccToCurrentAcc(account);
                }
            }

            para.Close();
        }
        private void CloseChangePasswordWindow(ChangePasswordWindow para)
        {
            para.Close();
        }
        #region Commands_Logic     
        private void ChooseImg(Grid para)
        {
            //this.HomeWindow = parameter;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Chọn ảnh";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imageFileName = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imageFileName);
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                para.Background = imageBrush;
                if (para.Children.Count != 0)
                {
                    para.Children.Remove(para.Children[0]);
                    para.Children.Remove(para.Children[0]);
                }
            }
        }
        private void Account_ChangePassword(HomeWindow para)
        {
            //if (string.IsNullOrEmpty(para.txt_Account_Password.Text))
            //{
            //    para.txt_Account_Password.Focus();
            //    return;
            //}
            //if (string.IsNullOrEmpty(para.txt_Account_NewPassword.Text))
            //{
            //    para.txt_Account_NewPassword.Focus();
            //    return;
            //}
            //if (string.IsNullOrEmpty(para.txt_Account_RetypeNewPassword.Text))
            //{
            //    para.txt_Account_RetypeNewPassword.Focus();
            //    return;
            //}
            //if (para.txt_Account_NewPassword.Text == para.txt_Account_Password.Text)
            //{
            //    para.txt_Account_NewPassword.Focus();
            //    MessageBox.Show("Vui lòng nhập mật khẩu khác với mật khẩu hiện tại.");
            //    return;
            //}
            //if (para.txt_Account_NewPassword.Text != para.txt_Account_RetypeNewPassword.Text)
            //{
            //    para.txt_Account_NewPassword.Focus();
            //    MessageBox.Show("Nhập lại mật khẩu chưa trùng khớp vui lòng nhập lại.");
            //    return;
            //}
            //try
            //{
            //    Account acc = new Account();
            //    acc = DataProvider.Instance.DB.Accounts.Where(x => x.Username == tempUsername).First();

            //    if (para.txt_Account_Password.Text != acc.Password)
            //    {
            //        MessageBox.Show("Mật khẩu hiện tại chưa đúng vui lòng nhập lại.");
            //        return;
            //    }
            //    else
            //    {
            //        acc.Password = para.txt_Account_RetypeNewPassword.Text;
            //        DataProvider.Instance.DB.Accounts.AddOrUpdate(acc);
            //        DataProvider.Instance.DB.SaveChanges();
            //        MessageBox.Show("Thay đổi mật khẩu thành công.");
            //        para.txt_Account_Password.Clear();
            //        para.txt_Account_NewPassword.Clear();
            //        para.txt_Account_RetypeNewPassword.Clear();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        private void UpdateAccount(HomeWindow para)
        {
            //if (string.IsNullOrEmpty(para.txt_Account_Name.Text))
            //{
            //    para.txt_Account_Name.Focus();
            //    return;
            //}
            //if (string.IsNullOrEmpty(para.txt_Account_Location.Text))
            //{
            //    para.txt_Account_Location.Focus();
            //    return;
            //}
            //if (string.IsNullOrEmpty(para.txt_Account_PhoneNumber.Text))
            //{
            //    para.txt_Account_PhoneNumber.Focus();
            //    return;
            //}
            //para.Title = "Update info account";
            //try
            //{
            //    Account acc = new Account();
            //    acc = DataProvider.Instance.DB.Accounts.Where(x => x.Username == tempUsername).First();

            //    if (para.txt_Account_Name.Text == acc.DisplayName && para.txt_Account_Location.Text == acc.Location && para.txt_Account_PhoneNumber.Text == acc.PhoneNumber)
            //    {
            //        MessageBox.Show("Thông tin không thay đổi");
            //        return;
            //    }
            //    else
            //    {
            //        acc.DisplayName = para.txt_Account_Name.Text;
            //        acc.Location = para.txt_Account_Location.Text;
            //        acc.PhoneNumber = para.txt_Account_PhoneNumber.Text;
            //        DataProvider.Instance.DB.Accounts.AddOrUpdate(acc);
            //        DataProvider.Instance.DB.SaveChanges();
            //        MessageBox.Show("Cập nhật thông tin thành công.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        private void LoadAccount(HomeWindow para)
        {
            //this.HomeWindow = para;
            //tempUsername = para.txt_Account_Username.Text;
            ////string query = "SELECT " + username + " FROM Acount;

            //Account account = DataProvider.Instance.DB.Accounts.FirstOrDefault(x => x.Username == tempUsername);

            //para.txt_Account_Name.Text = account.DisplayName;
            //para.txt_Account_Location.Text = account.Location;
            //para.txt_Account_PhoneNumber.Text = account.PhoneNumber;

            //ImageBrush imageBrush = new ImageBrush();
            //imageBrush.ImageSource = Converter.Instance.ConvertByteToBitmapImage(account.Image);
            //if (para.grdImageAccount.Children.Count > 1)
            //{
            //    para.grdImageAccount.Children.Remove(para.grdImageAccount.Children[0]);
            //    para.grdImageAccount.Children.Remove(para.icoImageAccount);
            //}
            //para.grdImageAccount.Background = imageBrush;
        }
        #endregion
    }
}