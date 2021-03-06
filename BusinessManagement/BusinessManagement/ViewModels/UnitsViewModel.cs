using BusinessManagement.Models;
using BusinessManagement.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BusinessManagement.ViewModels
{
    public class UnitsViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }

        public UnitsViewModel()
        {
            SaveCommand = new RelayCommand<AddUnitsWindow>((para) => true, (para) => AddUnits(para));
            CloseWindowCommand = new RelayCommand<AddUnitsWindow>((para) => true, (para) => CloseWindow(para));
        }

        private void AddUnits(AddUnitsWindow para)
        {

/*            StreamReader sr = new StreamReader("../../cache.txt");
            string cache = sr.ReadToEnd();
            sr.Close();
            string[] rulesSetting = cache.Split(' ');

            if (DataProvider.Instance.DB.Units.Count() >= int.Parse(rulesSetting[3]))
            {
                CustomMessageBox.Show("Unit limit!", "Notify", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }*/

            if (string.IsNullOrEmpty(para.txtName.Text))
            {
                para.txtName.Focus();
                para.txtName.Text = "";
                return;
            }

            try
            {
                Unit unit = new Unit();
                unit.ID = int.Parse(para.txtID.Text);
                unit.Name = para.txtName.Text;
                para.isSaveSucceed = true;

                DataProvider.Instance.DB.Units.Add(unit);
                DataProvider.Instance.DB.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                para.isSaveSucceed = false;
            }
            finally
            {
                para.Close();
            }
        }

        private void CloseWindow(AddUnitsWindow para)
        {
            para.Close();
        }
    }
}
