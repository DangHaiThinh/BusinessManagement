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

namespace BusinessManagement.Views
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void grdWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentAccount.Role.Contains("1"))
            {
                grdMenu_Stores.Visibility = Visibility.Visible;
            }
            if (CurrentAccount.Role.Contains("2"))
            {
                grdMenu_Products.Visibility = Visibility.Visible;
            }
            if (CurrentAccount.Role.Contains("3"))
            {
                grdMenu_Business.Visibility = Visibility.Visible;
            }
            if (CurrentAccount.Role.Contains("4"))
            {
                grdMenu_Bills.Visibility = Visibility.Visible;
            }
            if (CurrentAccount.Role.Contains("5"))
            {
                grdMenu_Report.Visibility = Visibility.Visible;
            }
            if (CurrentAccount.Role.Contains("6"))
            {
                grdMenu_AccountManagement.Visibility = Visibility.Visible;
            }
            if (CurrentAccount.Role.Contains("7"))
            {
                grdMenu_Setting.Visibility = Visibility.Visible;
            }
        }

    }
}
