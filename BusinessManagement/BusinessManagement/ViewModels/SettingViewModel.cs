using BusinessManagement.Models;
using BusinessManagement.Resources.UserControls;
using BusinessManagement.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BusinessManagement.ViewModels
{
    class SettingViewModel : BaseViewModel
    {
        public HomeWindow HomeWindow { get; set; }
        public int PageNumber { get; set; }
        public List<TypeOfAgency> ListType
        {
            get; set;
        }
        public string cache;
        public ICommand LoadSettingWindowCommand { get; set; }
        public ICommand SaveRulesType_SettingCommand { get; set; }
        public ICommand SaveRulesProduct_SettingCommand { get; set; }
        public ICommand EditTypeCommand { get; set; }
        public ICommand DeleteTypeCommand { get; set; }
        public ICommand SaveStoreCommand { get; set; }
        public ICommand CloseStoreWindowCommand { get; set; }
        public ICommand OpenAddTypeCommand { get; set; }
        public ICommand SeparateThousandsCommand { get; set; }

        public SettingViewModel()
        {
            this.ListType = DataProvider.Instance.DB.TypeOfAgencies.ToList<TypeOfAgency>();
            cache = "";

            LoadSettingWindowCommand = new RelayCommand<HomeWindow>((para) => true, (para) => LoadSettingWindow(para));
            SaveRulesType_SettingCommand = new RelayCommand<HomeWindow>((para) => true, (para) => SaveRulesType_Setting(para));
            SaveRulesProduct_SettingCommand = new RelayCommand<HomeWindow>((para) => true, (para) => SaveRulesProduct_Setting(para));
            EditTypeCommand = new RelayCommand<TypeOfAgencyUC>((para) => true, (para) => EditType(para));
            DeleteTypeCommand = new RelayCommand<TypeOfAgencyUC>((para) => true, (para) => DeleteType(para));
            SaveStoreCommand = new RelayCommand<AddTypeOfAgencyWindow>((para) => true, (para) => SaveStore(para));
            CloseStoreWindowCommand = new RelayCommand<AddTypeOfAgencyWindow>((para) => true, (para) => para.Close());
            OpenAddTypeCommand = new RelayCommand<Object>((para) => true, (para) => OpenAddType());
            SeparateThousandsCommand = new RelayCommand<TextBox>((para) => true, (para) => SeparateThousands(para));
        }

        private void OpenAddType()
        {
            AddTypeOfAgencyWindow wd = new AddTypeOfAgencyWindow();
            this.ListType = DataProvider.Instance.DB.TypeOfAgencies.ToList<TypeOfAgency>();

            try
            {
                wd.txtID.Text = (this.ListType.Last().ID + 1).ToString();
            }
            catch
            {
                wd.txtID.Text = "1";
            }
            finally
            {
                wd.txtName.Text = null;
                wd.txtDebt.Text = null;
                wd.ShowDialog();
            }
        }

        private void SaveStore(AddTypeOfAgencyWindow para)
        {
            if (String.IsNullOrEmpty(para.txtName.Text))
            {
                para.txtName.Focus();
                para.txtName.Text = "";
                return;
            }
            if (String.IsNullOrEmpty(para.txtDebt.Text))
            {
                para.txtDebt.Focus();
                para.txtDebt.Text = "";
                return;
            }

            int id = int.Parse(para.txtID.Text);
            TypeOfAgency item = new TypeOfAgency();
            item.ID = id;
            item.Name = para.txtName.Text;
            item.MaxOfDebt = ConvertToNumber(para.txtDebt.Text);

            DataProvider.Instance.DB.TypeOfAgencies.AddOrUpdate(item);
            DataProvider.Instance.DB.SaveChanges();

            int count = this.HomeWindow.stkListType_Setting.Children.Count;

            for (int i = 0; i < count - 1; i++)
            {
                this.HomeWindow.stkListType_Setting.Children.RemoveAt(0);
            }

            para.Close();
            LoadSettingWindow(this.HomeWindow);
        }

        private void DeleteType(TypeOfAgencyUC para)
        {
            MessageBoxResult mes = CustomMessageBox.Show("Bạn có chắc không?", "Xác nhận", MessageBoxButton.YesNo);

            if (mes != MessageBoxResult.Yes)
            {
                return;
            }

            List<Agency> list = DataProvider.Instance.DB.Agencies.Where(x => x.IsDelete == false).ToList();

            int stt = int.Parse(para.txbSTT.Text);

            foreach (Agency item in list)
            {
                if (item.TypeOfAgency == this.ListType[stt - 1].ID)
                {
                    CustomMessageBox.Show("Bạn phải xóa hết tất cả đại lý loại này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            TypeOfAgency type = this.ListType[stt - 1];
            DataProvider.Instance.DB.TypeOfAgencies.Remove(type);
            DataProvider.Instance.DB.SaveChanges();

            int count = this.HomeWindow.stkListType_Setting.Children.Count;

            for (int i = 0; i < count - 1; i++)
            {
                this.HomeWindow.stkListType_Setting.Children.RemoveAt(0);
            }
            LoadSettingWindow(this.HomeWindow);
        }

        private void EditType(TypeOfAgencyUC para)
        {
            int stt = int.Parse(para.txbSTT.Text);

            this.ListType = DataProvider.Instance.DB.TypeOfAgencies.ToList();

            AddTypeOfAgencyWindow wd = new AddTypeOfAgencyWindow();
            wd.txtID.Text = this.ListType[stt - 1].ID.ToString();
            wd.txtName.Text = this.ListType[stt - 1].Name;
            wd.txtDebt.Text = SeparateThousands(this.ListType[stt - 1].MaxOfDebt.ToString());

            wd.txtName.SelectionStart = wd.txtName.Text.Length;
            wd.txtDebt.SelectionStart = wd.txtDebt.Text.Length;

            wd.ShowDialog();
        }

        private void SaveRulesType_Setting(HomeWindow para)
        {
            int limit= LimitOfAgencyinDistrict();
            int? count = DataProvider.Instance.DB.Settings.First().NumberStoreInDistrict;

            if (string.IsNullOrEmpty(para.txtNumberAgencyinDistrict_Setting.Text))
            {
                para.txtNumberAgencyinDistrict_Setting.Text = "";
                para.txtNumberAgencyinDistrict_Setting.Focus();
                return;
            }

            if (ConvertToNumber(para.txtNumberAgencyinDistrict_Setting.Text) == count)
            {
                CustomMessageBox.Show("Cài đặt chưa thay đổi...!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (limit > ConvertToNumber(para.txtNumberAgencyinDistrict_Setting.Text))
            {
                string show = string.Format("Số đại lý trong quận phải lớn hơn số đại lý hiện tại!", limit);
                CustomMessageBox.Show(show, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Setting setting = DataProvider.Instance.DB.Settings.Where(x => x.ID == 1).First();
            setting.NumberStoreInDistrict = (int)ConvertToNumber(para.txtNumberAgencyinDistrict_Setting.Text);
            DataProvider.Instance.DB.Settings.AddOrUpdate(setting);
            DataProvider.Instance.DB.SaveChanges();
            CustomMessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void SaveRulesProduct_Setting(HomeWindow para)
        {
            int limit = LimitOfProduct();
            int? count = DataProvider.Instance.DB.Settings.First().NumberProductType;

            if (string.IsNullOrEmpty(para.txtNumberProduct_Setting.Text))
            {
                para.txtNumberProduct_Setting.Text = "";
                para.txtNumberProduct_Setting.Focus();
                return;
            }

            if (ConvertToNumber(para.txtNumberProduct_Setting.Text) == count)
            {
                CustomMessageBox.Show("Cài đặt chưa thay đổi...!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (limit > ConvertToNumber(para.txtNumberProduct_Setting.Text))
            {
                string show = string.Format("Số loại sản phẩm phải lớn hơn số loại sản phẩm hiện tại!", limit);
                CustomMessageBox.Show(show, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Setting setting = DataProvider.Instance.DB.Settings.Where(x => x.ID == 1).First();
            setting.NumberProductType = (int)ConvertToNumber(para.txtNumberProduct_Setting.Text);
            DataProvider.Instance.DB.Settings.AddOrUpdate(setting);
            DataProvider.Instance.DB.SaveChanges();
            CustomMessageBox.Show("Thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void LoadSettingWindow(HomeWindow para)
        {
            this.HomeWindow = para;
            this.ListType = DataProvider.Instance.DB.TypeOfAgencies.ToList<TypeOfAgency>();

            for (int i = 0; i < this.ListType.Count; i++)
            {
                TypeOfAgencyUC uc = new TypeOfAgencyUC();

                uc.txbSTT.Text = (i + 1).ToString();
                uc.txbName.Text = this.ListType[i].Name;
                uc.txbDebt.Text = SeparateThousands(this.ListType[i].MaxOfDebt.ToString());

                para.stkListType_Setting.Children.Add(uc);
            }


            int? countStore = DataProvider.Instance.DB.Settings.First().NumberStoreInDistrict;
            para.txtNumberAgencyinDistrict_Setting.Text = ConvertToString(countStore);
            para.txtNumberAgencyinDistrict_Setting.SelectionStart = para.txtNumberAgencyinDistrict_Setting.Text.Length;

            int? countProduct = DataProvider.Instance.DB.Settings.First().NumberProductType;
            para.txtNumberProduct_Setting.Text = ConvertToString(countProduct);
            para.txtNumberProduct_Setting.SelectionStart = para.txtNumberProduct_Setting.Text.Length;

            Button bt = new Button();
            bt = (Button)para.stkListType_Setting.Children[0];
            para.stkListType_Setting.Children.RemoveAt(0);
            para.stkListType_Setting.Children.Add(bt);
        }

        public void Reload(HomeWindow para)
        {
            this.HomeWindow = para;

            int? countStore = DataProvider.Instance.DB.Settings.First().NumberStoreInDistrict;
            para.txtNumberAgencyinDistrict_Setting.Text = ConvertToString(countStore);
            para.txtNumberAgencyinDistrict_Setting.SelectionStart = para.txtNumberAgencyinDistrict_Setting.Text.Length;

            int? countProduct = DataProvider.Instance.DB.Settings.First().NumberProductType;
            para.txtNumberProduct_Setting.Text = ConvertToString(countProduct);
            para.txtNumberProduct_Setting.SelectionStart = para.txtNumberProduct_Setting.Text.Length;
        }

        private int LimitOfAgencyinDistrict()
        {
            int max = 0;

            List<Agency> listAgency = DataProvider.Instance.DB.Agencies.Where(x => x.IsDelete == false).ToList();

            var results = DataProvider.Instance.DB.Agencies.Select(x => x.District).Distinct().ToList();

            foreach (string item in results)
            {
                int count = 0;
                foreach (Agency agency in listAgency)
                {
                    if (agency.District == item)
                        count++;
                }
                if (count > max)
                    max = count;
            }
            return max;
        }
        private int LimitOfProduct()
        {
            int max = 0;
            List<Product> listProduct = DataProvider.Instance.DB.Products.Where(x => x.IsDelete == false).ToList();

            foreach (Product item in listProduct)
            {
                max++;
            }
            return max;
        }
    }
}
