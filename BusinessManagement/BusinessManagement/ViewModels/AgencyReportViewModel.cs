using BusinessManagement.Models;
using BusinessManagement.Resources.UserControls;
using BusinessManagement.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System;

namespace BusinessManagement.ViewModels
{
    class AgencyReportViewModel : BaseViewModel
    {
        private string uid;
        public HomeWindow HomeWindow { get; set; }
        public ICommand LoadSalesReportCommand { get; set; }
        public ICommand LoadDebtsReportCommand { get; set; }
        public ICommand SwitchCommand { get; set; }
        public ICommand GetUidCommand { get; set; }
        public ICommand SearchAgencyCommand { get; set; }
        public ICommand InitCommand { get; set; }
        public ICommand CheckDateCommand { get; set; }
        public ICommand RPIDSortCommand { get; set; }
        public ICommand RPAgencySortCommand { get; set; }
        public ICommand RPAmountSortCommand { get; set; }
        public ICommand RPSumSortCommand { get; set; }
        public ICommand RPRateSortCommand { get; set; }
        public ICommand RP1IDSortCommand { get; set; }
        public ICommand RP1AgencySortCommand { get; set; }
        public ICommand RP1AmountSortCommand { get; set; }
        public ICommand RP1SumSortCommand { get; set; }
        public ICommand RP1RateSortCommand { get; set; }
        public string CustomFormat { get; set; }

        public class SalesHolder
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int NumberofBills { get; set; }
            public long? Total { get; set; }
            public double Ratio { get; set; }
            public SalesHolder() { }
        }
        List<SalesHolder> SalesHolderList = new List<SalesHolder>();
        public class DebtHolder
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public long? OriginalDebt { get; set; }
            public long? Total { get; set; }
            public long? CostOverRun { get; set; }
            public DebtHolder() { }
        }
        List<DebtHolder> DebtHolderList = new List<DebtHolder>();
        public AgencyReportViewModel()
        {
            LoadSalesReportCommand = new RelayCommand<HomeWindow>((para) => true, (para) => LoadSalesReport(para));
            LoadDebtsReportCommand = new RelayCommand<HomeWindow>((para) => true, (para) => LoadDebtsReport(para));
            GetUidCommand = new RelayCommand<Button>((para) => true, (para) => uid = para.Uid);
            SwitchCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Switch(para));
            SearchAgencyCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Search(para));
            InitCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Init(para));
            CheckDateCommand = new RelayCommand<HomeWindow>((para) => true, (para) => CheckDate(para));
            RPIDSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 1));
            RPAgencySortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 2));
            RPAmountSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 3));
            RPSumSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 4));
            RPRateSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 5));
            RP1IDSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 1));
            RP1AgencySortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 2));
            RP1AmountSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 3));
            RP1SumSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 4));
            RP1RateSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => RPSort(para, 5));
        }
        public void SetDefaultSortDisplay1(HomeWindow para)
        {
            para.RPAscSortImage1.Visibility = Visibility.Hidden;
            para.RPAscSortImage2.Visibility = Visibility.Hidden;
            para.RPAscSortImage3.Visibility = Visibility.Hidden;
            para.RPAscSortImage4.Visibility = Visibility.Hidden;
            para.RPAscSortImage5.Visibility = Visibility.Hidden;
            para.RPDesSortImage1.Visibility = Visibility.Hidden;
            para.RPDesSortImage2.Visibility = Visibility.Hidden;
            para.RPDesSortImage3.Visibility = Visibility.Hidden;
            para.RPDesSortImage4.Visibility = Visibility.Hidden;
            para.RPDesSortImage5.Visibility = Visibility.Hidden;
        }
        public void SetDefaultSortDisplay2(HomeWindow para)
        {
            para.RP1AscSortArrow1.Visibility = Visibility.Hidden;
            para.RP1AscSortArrow2.Visibility = Visibility.Hidden;
            para.RP1AscSortArrow3.Visibility = Visibility.Hidden;
            para.RP1AscSortArrow4.Visibility = Visibility.Hidden;
            para.RP1AscSortArrow5.Visibility = Visibility.Hidden;
            para.RP1DesSortArrow1.Visibility = Visibility.Hidden;
            para.RP1DesSortArrow2.Visibility = Visibility.Hidden;
            para.RP1DesSortArrow3.Visibility = Visibility.Hidden;
            para.RP1DesSortArrow4.Visibility = Visibility.Hidden;
            para.RP1DesSortArrow5.Visibility = Visibility.Hidden;
        }
        bool RPIsSort = false;
        public void RPSort(HomeWindow para, int temp)
        {
            para.stkSalesReport.Children.Clear();
            if (para.comboBoxReport.SelectedIndex == 0 || para.comboBoxReport.SelectedIndex == -1)
            {
                SetDefaultSortDisplay1(para);
                List<SalesHolder> RPSortHolder = new List<SalesHolder>();
                if (temp == 1)
                {
                    if (RPIsSort)
                    {
                        RPSortHolder = SalesHolderList.OrderByDescending(p => p.ID).ToList();
                        para.RPDesSortImage1.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        RPSortHolder = SalesHolderList.OrderBy(p => p.ID).ToList();
                        para.RPAscSortImage1.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                if (temp == 2)
                {
                    if (RPIsSort)
                    {
                        RPSortHolder = SalesHolderList.OrderByDescending(p => p.Name).ToList();
                        para.RPDesSortImage2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        RPSortHolder = SalesHolderList.OrderBy(p => p.Name).ToList();
                        para.RPAscSortImage2.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                if (temp == 3)
                {
                    if (RPIsSort)
                    {
                        RPSortHolder = SalesHolderList.OrderByDescending(p => p.NumberofBills).ToList();
                        para.RPDesSortImage3.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        RPSortHolder = SalesHolderList.OrderBy(p => p.NumberofBills).ToList();
                        para.RPAscSortImage3.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                if (temp == 4)
                {
                    if (RPIsSort)
                    {
                        RPSortHolder = SalesHolderList.OrderByDescending(p => p.Total).ToList();
                        para.RPDesSortImage4.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        RPSortHolder = SalesHolderList.OrderBy(p => p.Total).ToList();
                        para.RPAscSortImage4.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                if (temp == 5)
                {
                    if (RPIsSort)
                    {
                        RPSortHolder = SalesHolderList.OrderByDescending(p => p.Ratio).ToList();
                        para.RPDesSortImage5.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        RPSortHolder = SalesHolderList.OrderBy(p => p.Ratio).ToList();
                        para.RPAscSortImage5.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                foreach(SalesHolder item in RPSortHolder)
                {
                    SalesReportUC saleRPUC = new SalesReportUC();
                    saleRPUC.Width = 1070;
                    saleRPUC.Height = 45;
                    saleRPUC.txtNo.Text = item.ID.ToString();
                    saleRPUC.txtAgency.Text = item.Name;
                    saleRPUC.txtNumberOfBills.Text = item.NumberofBills.ToString();
                    saleRPUC.txtTotal.Text = ConvertToString(item.Total);
                    saleRPUC.txtRatio.Text = item.Ratio.ToString("0.00") + "%";
                    para.stkSalesReport.Children.Add(saleRPUC);
                }
                foreach (SalesReportUC control in para.stkSalesReport.Children)
                {
                    if (!control.txtAgency.Text.ToLower().Contains(para.txtSearchReport.Text))
                    {
                        control.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        control.Visibility = Visibility.Visible;
                    }
                }
            }
            if (para.comboBoxReport.SelectedIndex == 1)
            {
                SetDefaultSortDisplay2(para);
                para.stkDebtReport.Children.Clear();
                List<DebtHolder> SortHolder = new List<DebtHolder>();
                if (temp == 1)
                {
                    if (RPIsSort)
                    {
                        SortHolder = DebtHolderList.OrderByDescending(p => p.ID).ToList();
                        para.RP1DesSortArrow1.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = DebtHolderList.OrderBy(p => p.ID).ToList();
                        para.RP1AscSortArrow1.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                if (temp == 2)
                {
                    if (RPIsSort)
                    {
                        SortHolder = DebtHolderList.OrderByDescending(p => p.Name).ToList();
                        para.RP1DesSortArrow2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = DebtHolderList.OrderBy(p => p.Name).ToList();
                        para.RP1AscSortArrow2.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                if (temp == 3)
                {
                    if (RPIsSort)
                    {
                        SortHolder = DebtHolderList.OrderByDescending(p => p.OriginalDebt).ToList();
                        para.RP1DesSortArrow3.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = DebtHolderList.OrderBy(p => p.OriginalDebt).ToList();
                        para.RP1AscSortArrow3.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                if (temp == 4)
                {
                    if (RPIsSort)
                    {
                        SortHolder = DebtHolderList.OrderByDescending(p => p.CostOverRun).ToList();
                        para.RP1DesSortArrow4.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = DebtHolderList.OrderBy(p => p.CostOverRun).ToList();
                        para.RP1AscSortArrow4.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                if (temp == 5)
                {
                    if (RPIsSort)
                    {
                        SortHolder = DebtHolderList.OrderByDescending(p => p.Total).ToList();
                        para.RP1DesSortArrow5.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = DebtHolderList.OrderBy(p => p.Total).ToList();
                        para.RP1AscSortArrow5.Visibility = Visibility.Visible;
                    }
                    RPIsSort = !RPIsSort;
                }
                foreach(DebtHolder item in SortHolder)
                {
                    DebtReportUC debtReportUC = new DebtReportUC();
                    debtReportUC.Height = 45;
                    debtReportUC.Width = 1070;
                    debtReportUC.txtNo.Text = item.ID.ToString();
                    debtReportUC.txtAgency.Text = item.Name;
                    debtReportUC.txtCostOverrun.Text = ConvertToString(item.CostOverRun);
                    debtReportUC.txtOriginalDebt.Text = ConvertToString(item.OriginalDebt);
                    debtReportUC.txtTotal.Text = ConvertToString(item.Total);
                    para.stkDebtReport.Children.Add(debtReportUC);
                }
                foreach (DebtReportUC control in para.stkDebtReport.Children)
                {
                    if (!control.txtAgency.Text.ToLower().Contains(para.txtSearchReport.Text))
                    {
                        control.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        control.Visibility = Visibility.Visible;
                    }
                }
            }
        }
        private void CheckDate(HomeWindow para)
        {
            if (DateTime.Compare(DateTime.Now, (DateTime)para.Date.SelectedDate) < 0)
            {
                CustomMessageBox.Show("Không thể chọn tháng trong tương lai!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                para.Date.Text = DateTime.Now.ToString();
            }    
        }

        public void Init(HomeWindow para)
        {
            SalesHolderList.Clear();
            DebtHolderList.Clear();
            para.Date.Text = DateTime.Now.ToString();
            para.comboBoxReport.SelectedIndex = 0;
            para.cardSalesReport.Visibility = Visibility.Visible;
            List<Agency> agencies = DataProvider.Instance.DB.Agencies.ToList<Agency>();
            para.stkSalesReport.Children.Clear();
            int check = 0;
            int checkDebt = 0;
            foreach (Agency agency in agencies)
            {
                long? total = 0;
                long? dept = 0;
                int count = 0;
                double totalDebtThisMonth = GetTotalThisMonth(DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString());
                List<Invoice> invoices = new List<Invoice>();
                SalesReportUC salesReportUC = new SalesReportUC();
                SalesHolder Holder1 = new SalesHolder();
                salesReportUC.Width = 1070;
                salesReportUC.Height = 45;
                salesReportUC.txtNo.Text = agency.ID.ToString();
                Holder1.ID = agency.ID;
                salesReportUC.txtAgency.Text = Holder1.Name = agency.Name;
                invoices = agency.Invoices.ToList();
                foreach (Invoice invoice in invoices)
                {
                    try
                    {
                        if (invoice.Checkout.Value.Month == DateTime.Now.Month && invoice.Checkout.Value.Year == DateTime.Now.Year)
                        {
                            total += invoice.Total;
                            dept += invoice.Debt;
                            count++;
                        }
                    }
                    catch { }
                }
                salesReportUC.txtNumberOfBills.Text = count.ToString();
                Holder1.NumberofBills = count;
                salesReportUC.txtTotal.Text = ConvertToString(total);
                Holder1.Total = total;
                salesReportUC.txtRatio.Text = (100 * (double)total / (double)(totalDebtThisMonth + 1)).ToString("0.00") + "%";
                Holder1.Ratio = 100 * (double)total / (double)(totalDebtThisMonth + 1);
                para.stkSalesReport.Children.Add(salesReportUC);
                SalesHolderList.Add(Holder1);
                para.scrollSales.Visibility = Visibility.Visible;
                check++;
                //
                long? deptDebt = 0;
                int countDebt = 0;
                List<Invoice> invoicesDebt = new List<Invoice>();
                DebtReportUC debtReportUC = new DebtReportUC();
                List<Invoice> invoiceDebtAfter = new List<Invoice>();
                DebtHolder Holder2 = new DebtHolder();
                debtReportUC.Height = 45;
                debtReportUC.Width = 1070;
                debtReportUC.txtNo.Text = agency.ID.ToString();
                Holder2.ID = agency.ID;
                debtReportUC.txtAgency.Text = Holder2.Name = agency.Name;
                invoicesDebt = DataProvider.Instance.DB.Invoices.Where(x => x.AgencyID == agency.ID).ToList();
                foreach (Invoice invoice in invoicesDebt)
                {
                    try
                    {
                        if (invoice.Checkout.Value.Month == DateTime.Now.Month && invoice.Checkout.Value.Year == DateTime.Now.Year)
                        {
                            deptDebt += invoice.Debt;
                            countDebt++;
                            invoiceDebtAfter.Add(invoice);
                        }
                    }
                    catch { }
                }
                checkDebt++;
                if (invoiceDebtAfter.Count != 0)
                {
                    debtReportUC.txtOriginalDebt.Text = ConvertToString(invoiceDebtAfter.First().Debt);
                    Holder2.OriginalDebt = invoiceDebtAfter.First().Debt;
                    debtReportUC.txtCostOverrun.Text = ConvertToString(deptDebt - invoiceDebtAfter.First().Debt);
                    Holder2.CostOverRun = deptDebt - invoiceDebtAfter.First().Debt;
                }
                else
                {
                    debtReportUC.txtOriginalDebt.Text = "0";
                    debtReportUC.txtCostOverrun.Text = "0";
                    Holder2.CostOverRun = 0;
                    Holder2.OriginalDebt = 0;
                }
                debtReportUC.txtTotal.Text = ConvertToString(dept);
                Holder2.Total = dept;
                DebtHolderList.Add(Holder2);

                para.stkDebtReport.Children.Add(debtReportUC);
            }
        }

        private void Search(HomeWindow para)
        {
            this.HomeWindow = para;
            foreach (SalesReportUC control in this.HomeWindow.stkSalesReport.Children)
            {
                if (!control.txtAgency.Text.ToLower().Contains(this.HomeWindow.txtSearchReport.Text))
                {
                    control.Visibility = Visibility.Collapsed;
                }
                else
                {
                    control.Visibility = Visibility.Visible;
                }
            }
            foreach (DebtReportUC control in this.HomeWindow.stkDebtReport.Children)
            {
                if (!control.txtAgency.Text.ToLower().Contains(this.HomeWindow.txtSearchReport.Text))
                {
                    control.Visibility = Visibility.Collapsed;
                }
                else
                {
                    control.Visibility = Visibility.Visible;
                }
            }
        }
        private void Switch(HomeWindow para)
        {
            if (para.comboBoxReport.SelectedIndex == 0)
            {
                para.cardTitleSalesReport.Visibility = System.Windows.Visibility.Visible;
                para.cardSalesReport.Visibility = System.Windows.Visibility.Visible;
                para.cardDebtAgencyReport.Visibility = System.Windows.Visibility.Hidden;
                para.scrollSales.Visibility = System.Windows.Visibility.Visible;
                para.scrollDebt.Visibility = System.Windows.Visibility.Hidden;
                RPIsSort = false;
                RPIDSortCommand.Execute(para);
                SetDefaultSortDisplay1(para);
                SetDefaultSortDisplay2(para);
            }
            if (para.comboBoxReport.SelectedIndex == 1)
            {

                para.cardTitleSalesReport.Visibility = System.Windows.Visibility.Visible;
                para.cardSalesReport.Visibility = System.Windows.Visibility.Hidden;
                para.cardDebtAgencyReport.Visibility = System.Windows.Visibility.Visible;
                para.scrollSales.Visibility = System.Windows.Visibility.Hidden;
                para.scrollDebt.Visibility = System.Windows.Visibility.Visible;
                RPIsSort = false;
                RPIDSortCommand.Execute(para);
                SetDefaultSortDisplay2(para);
                SetDefaultSortDisplay1(para);
            }

        }
        public void LoadDebtsReport(HomeWindow para)
        {
            this.HomeWindow = para;
            List<Agency> agencies = DataProvider.Instance.DB.Agencies.ToList<Agency>();
            this.HomeWindow.stkDebtReport.Children.Clear();
            int check = 0;
            DebtHolderList.Clear();
            foreach (Agency agency in agencies)
            {
                long? dept = 0;
                int count = 0;
                List<Invoice> invoices = new List<Invoice>();
                List<Invoice> invoicesAfter = new List<Invoice>();
                DebtReportUC debtReportUC = new DebtReportUC();
                DebtHolder Holder1 = new DebtHolder();
                debtReportUC.Height = 45;
                debtReportUC.Width = 1070;
                debtReportUC.txtNo.Text = agency.ID.ToString();
                Holder1.ID = agency.ID;
                debtReportUC.txtAgency.Text = Holder1.Name = agency.Name;
                invoices = DataProvider.Instance.DB.Invoices.Where(x => x.AgencyID == agency.ID).ToList();
                
                foreach (Invoice invoice in invoices)
                {
                    try
                    {
                        if (invoice.Checkout.Value.Month == para.Date.SelectedDate.Value.Month && invoice.Checkout.Value.Year == para.Date.SelectedDate.Value.Year)
                        {
                            dept += invoice.Debt;
                            count++;
                            invoicesAfter.Add(invoice);
                        }
                    }
                    catch { }
                }
                check++;
                if (invoicesAfter.Count != 0)
                {
                    debtReportUC.txtOriginalDebt.Text = ConvertToString( invoicesAfter.First().Debt);
                    Holder1.OriginalDebt = invoicesAfter.First().Debt;
                    debtReportUC.txtCostOverrun.Text = ConvertToString(dept - invoicesAfter.First().Debt);
                    Holder1.CostOverRun = dept - invoicesAfter.First().Debt;
                }
                else
                {
                    debtReportUC.txtOriginalDebt.Text = "0";
                    debtReportUC.txtCostOverrun.Text = "0";
                    Holder1.OriginalDebt = 0;
                    Holder1.CostOverRun = 0;
                }
                debtReportUC.txtTotal.Text = ConvertToString(dept);
                Holder1.Total = dept;

                this.HomeWindow.stkDebtReport.Children.Add(debtReportUC);
                DebtHolderList.Add(Holder1);
            }
        }
        public void LoadSalesReport(HomeWindow para)
        {
            this.HomeWindow = para;
            List<Agency> agencies = DataProvider.Instance.DB.Agencies.ToList<Agency>();
            this.HomeWindow.stkSalesReport.Children.Clear();
            int check = 0;
            SalesHolderList.Clear();
            foreach (Agency agency in agencies)
            {
                long? total = 0;
                long? dept = 0;
                int count = 0;
                double totalDebtThisMonth = GetTotalThisMonth(para.Date.SelectedDate.Value.Month.ToString(), para.Date.SelectedDate.Value.Year.ToString());
                List<Invoice> invoices = new List<Invoice>();
                SalesReportUC salesReportUC = new SalesReportUC();
                SalesHolder Holder2 = new SalesHolder();
                salesReportUC.Width = 1070;
                salesReportUC.Height = 45;
                salesReportUC.txtNo.Text = agency.ID.ToString();
                Holder2.ID = agency.ID;
                salesReportUC.txtAgency.Text = Holder2.Name = agency.Name;
                invoices = agency.Invoices.ToList();
                foreach (Invoice invoice in invoices)
                {
                    try
                    {
                        if (invoice.Checkout.Value.Month == para.Date.SelectedDate.Value.Month && invoice.Checkout.Value.Year == para.Date.SelectedDate.Value.Year)
                        {
                            total += invoice.Total;
                            dept += invoice.Debt;
                            count++;
                        }
                    }
                    catch { }
                }
                salesReportUC.txtNumberOfBills.Text = count.ToString();
                Holder2.NumberofBills = count;
                salesReportUC.txtTotal.Text = ConvertToString(total);
                Holder2.Total = total;
                Holder2.Ratio = 100 * (double)total / (double)(totalDebtThisMonth + 1);
                salesReportUC.txtRatio.Text = (100 * (double)total / (double)(totalDebtThisMonth + 1)).ToString("0.00") + "%";
                this.HomeWindow.stkSalesReport.Children.Add(salesReportUC);
                SalesHolderList.Add(Holder2);
                check++;
            }
        }

        private double GetTotalThisMonth(string month, string year)
        {
            
            double total = 0;
            try
            {
                string query1 = string.Format("select  sum(Total) as Total from Invoice " +
                                            "WHERE MONTH(CHECKOUT) = {0} AND YEAR(CHECKOUT) = {1} ", month, year);
                Int64 temp = DataProvider.Instance.DB.Database.SqlQuery<Int64>(query1).ToList().First();
                total = (double)temp;
                return total;
            }
            catch
            {
                return total;
            }
        }
    }


}
