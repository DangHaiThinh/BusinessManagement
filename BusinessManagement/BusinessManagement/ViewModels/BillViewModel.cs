using Microsoft.Win32;
using BusinessManagement.Models;
using BusinessManagement.Resources.UserControls;
using BusinessManagement.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using Microsoft;
using FastMember;

namespace BusinessManagement.ViewModels
{
    public class BillViewModel : BaseViewModel
    {
        private string uid;
        private int status = 0;
        private long? total = 0;
        private long? pay = 0;
        public List<Agency> ListAgency = DataProvider.Instance.DB.Agencies.Where(x => x.IsDelete == false).ToList();

        public HomeWindow HomeWindow { get; set; }
        public ICommand OpenInvoiceWindowCommand { get; set; }
        public ICommand OpenReceiptWindowCommand { get; set; }
        public ICommand SeparateThousandsCommand { get; set; }
        public ICommand LoadBillCommand { get; set; }
        public ICommand LoadReceiptBillCommand { get; set; }
        public ICommand LoadStockReceiptCommnad { get; set; }
        public ICommand SwitchCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }
        public ICommand GetUidCommand { get; set; }
        public ICommand SearchAgencyCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand InitCommand { get; set; }
        public ICommand ShellOutCommand { get; set; }
        public ICommand ChooseAgencyCommand { get; set; }
        public ICommand PaymentShellOutCommand { get; set; }
        public ICommand PayCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand CloseReceiptBillWindowCommand { get; set; }
        public ICommand IDSortCommand { get; set; }
        public ICommand AgencySortCommand { get; set; }
        public ICommand CheckOutSortCommand { get; set; }
        public ICommand LastBoxSortCommand { get; set; }
        public BillViewModel()
        {
            LoadBillCommand = new RelayCommand<HomeWindow>((para) => true, (para) => LoadBill(para));
            LoadReceiptBillCommand = new RelayCommand<HomeWindow>((para) => true, (para) => LoadReceiptBill(para));
            LoadStockReceiptCommnad = new RelayCommand<HomeWindow>((para) => true, (para) => LoadStockReceipt(para));
            GetUidCommand = new RelayCommand<ComboBox>((para) => true, (para) => uid = para.Uid);
            SwitchCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Switch(para));
            SearchAgencyCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Search(para));
            OpenInvoiceWindowCommand = new RelayCommand<InvoiceUC>((para) => true, (para) => OpenInvoiceWindow(para));
            OpenReceiptWindowCommand = new RelayCommand<ReceiptBillUC>((para) => true, (para) => OpenReceiptWindow(para));
            ExportExcelCommand = new RelayCommand<HomeWindow>((para) => true, (para) => ExportExcel(para));
            ClearCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Clear(para));
            InitCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Init(para));
            ShellOutCommand = new RelayCommand<HomeWindow>((para) => true, (para) => ShellOut(para));
            SeparateThousandsCommand = new RelayCommand<TextBox>((para) => true, (para) => SeparateThousands(para));
            ChooseAgencyCommand = new RelayCommand<ShellOutWindow>((para) => true, (para) => ChooseAgency(para));
            PaymentShellOutCommand = new RelayCommand<ShellOutWindow>((para) => true, (para) =>
            {
                SeparateThousands(para.txtPayment);
                LimitPayment(para);
            });
            PayCommand = new RelayCommand<ShellOutWindow>((para) => true, (para) => PayReceiptBill(para));
            CloseWindowCommand = new RelayCommand<ShellOutWindow>((para) => true, (para) => para.Close());
            CloseReceiptBillWindowCommand = new RelayCommand<ReceiptBillWindow>((para) => true, (para) => para.Close());
            IDSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Sort(para, 1));
            AgencySortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Sort(para, 2));
            CheckOutSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Sort(para, 3));
            LastBoxSortCommand = new RelayCommand<HomeWindow>((para) => true, (para) => Sort(para, 4));
        }
        public class BillContentHolder
        {
            public int ID { get; set; }
            public string AgencyName { get; set; }
            public DateTime CheckOut { get; set; }
            public long? Debt { get; set; }
            public BillContentHolder()
            { }
        }
        public class ReceiptContentHolder
        {
            public int ID { get; set; }
            public string AgencyName { get; set; }
            public DateTime CheckOut { get; set; }
            public long? Amount { get; set; }
            public ReceiptContentHolder()
            { }
        }
        public class StockContentHolder
        {
            public int ID { get; set; }
            public string AgencyName { get; set; }
            public DateTime? CheckIn { get; set; }
            public long? Total { get; set; }
            public StockContentHolder()
            { }
        }
        private void SetSortImageToHidden(HomeWindow para)
        {
            para.DesSortImage1.Visibility = Visibility.Hidden;
            para.DesSortImage2.Visibility = Visibility.Hidden;
            para.DesSortImage3.Visibility = Visibility.Hidden;
            para.DesSortImage4.Visibility = Visibility.Hidden;
            para.AscSortImage1.Visibility = Visibility.Hidden;
            para.AscSortImage2.Visibility = Visibility.Hidden;
            para.AscSortImage3.Visibility = Visibility.Hidden;
            para.AscSortImage4.Visibility = Visibility.Hidden;
        }
        bool IsSort = false;
        public void Sort(HomeWindow para, int temp)
        {
            SetSortImageToHidden(para);
            if (para.comboBoxBill.SelectedIndex == 1 || para.comboBoxBill.SelectedIndex == -1)
            {
                para.stkBill.Children.Clear();
                List<Invoice> invoices = new List<Invoice>();
                invoices = DataProvider.Instance.DB.Invoices.ToList<Invoice>();
                List<BillContentHolder> Holder = new List<BillContentHolder>();
                foreach (Invoice invoice in invoices)
                {
                    BillContentHolder holder = new BillContentHolder();
                    holder.ID = invoice.ID;
                    holder.AgencyName = invoice.Agency.Name.ToString();
                    holder.CheckOut = invoice.Checkout.Value;
                    holder.Debt = invoice.Debt;
                    Holder.Add(holder);
                }
                List<BillContentHolder> SortHolder = new List<BillContentHolder>();
                if (temp == 1)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.ID).ToList();
                        para.DesSortImage1.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.ID).ToList();
                        para.AscSortImage1.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                if (temp == 2)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.AgencyName).ToList();
                        para.DesSortImage2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.AgencyName).ToList();
                        para.AscSortImage2.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                if (temp == 3)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.CheckOut).ToList();
                        para.DesSortImage3.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.CheckOut).ToList();
                        para.AscSortImage3.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                if (temp == 4)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.Debt).ToList();
                        para.DesSortImage4.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.Debt).ToList();
                        para.AscSortImage4.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                foreach (BillContentHolder item in SortHolder)
                {
                    InvoiceUC invoiceUC = new InvoiceUC();
                    invoiceUC.InvoiceID.Text = item.ID.ToString();
                    invoiceUC.AgencyName.Text = item.AgencyName;
                    invoiceUC.CheckOut.Text = item.CheckOut.ToShortDateString();
                    invoiceUC.Debt.Text = ConvertToString(item.Debt);
                    this.HomeWindow.stkBill.Children.Add(invoiceUC);
                }
                foreach (InvoiceUC control in this.HomeWindow.stkBill.Children)
                {
                    if (!control.AgencyName.Text.ToLower().Contains(this.HomeWindow.txtSearchAgencyinBill.Text))
                    {
                        control.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        control.Visibility = Visibility.Visible;
                    }
                }
            }
            if (para.comboBoxBill.SelectedIndex == 2)
            {
                para.stkReceiptBill.Children.Clear();
                List<Receipt> receipts = new List<Receipt>();
                receipts = DataProvider.Instance.DB.Receipts.ToList<Receipt>();
                List<ReceiptContentHolder> Holder = new List<ReceiptContentHolder>();
                foreach (Receipt receipt in receipts)
                {
                    ReceiptContentHolder holder = new ReceiptContentHolder();
                    holder.ID = receipt.ID;
                    holder.AgencyName = receipt.Agency.Name;
                    holder.CheckOut = receipt.Date.Value;
                    holder.Amount = receipt.Amount;
                    Holder.Add(holder);
                }
                List<ReceiptContentHolder> SortHolder = new List<ReceiptContentHolder>();
                if (temp == 1)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.ID).ToList();
                        para.DesSortImage1.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.ID).ToList();
                        para.AscSortImage1.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                if (temp == 2)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.AgencyName).ToList();
                        para.DesSortImage2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.AgencyName).ToList();
                        para.AscSortImage2.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                if (temp == 3)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.CheckOut).ToList();
                        para.DesSortImage3.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.CheckOut).ToList();
                        para.AscSortImage3.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                if (temp == 4)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.Amount).ToList();
                        para.DesSortImage4.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.Amount).ToList();
                        para.AscSortImage4.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                foreach (ReceiptContentHolder item in SortHolder)
                {
                    ReceiptBillUC receiptBillUC = new ReceiptBillUC();
                    receiptBillUC.ReceiptID.Text = item.ID.ToString();
                    receiptBillUC.AgencyName.Text = item.AgencyName.ToString();
                    receiptBillUC.CheckOut.Text = item.CheckOut.ToShortDateString();
                    receiptBillUC.Amount.Text = ConvertToString(item.Amount);
                    this.HomeWindow.stkReceiptBill.Children.Add(receiptBillUC);
                }                
                foreach (ReceiptBillUC control in this.HomeWindow.stkReceiptBill.Children)
                {
                    if (!control.AgencyName.Text.ToLower().Contains(this.HomeWindow.txtSearchAgencyinBill.Text))
                    {
                        control.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        control.Visibility = Visibility.Visible;
                    }
                }
            }
            if (para.comboBoxBill.SelectedIndex == 0)
            {
                this.HomeWindow.stkStockReceipt.Children.Clear();
                List<StockReceipt> stockReceipts = new List<StockReceipt>();
                stockReceipts = DataProvider.Instance.DB.StockReceipts.ToList<StockReceipt>();
                List<StockContentHolder> Holder = new List<StockContentHolder>();
                foreach (StockReceipt stock in stockReceipts)
                {
                    StockContentHolder holder = new StockContentHolder();
                    holder.ID = stock.ID;
                    holder.AgencyName = "Công ty này";
                    holder.CheckIn = stock.CheckIn;
                    holder.Total = stock.Total;
                    Holder.Add(holder);
                }
                List<StockContentHolder> SortHolder = new List<StockContentHolder>();
                if (temp == 1)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.ID).ToList(); 
                        para.DesSortImage1.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.ID).ToList();
                        para.AscSortImage1.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                if (temp == 2)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.AgencyName).ToList(); 
                        para.DesSortImage2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.AgencyName).ToList();
                        para.AscSortImage2.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                if (temp == 3)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.CheckIn).ToList();
                        para.DesSortImage3.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.CheckIn).ToList();
                        para.AscSortImage3.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                if (temp == 4)
                {
                    if (IsSort)
                    {
                        SortHolder = Holder.OrderByDescending(p => p.Total).ToList();
                        para.DesSortImage4.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SortHolder = Holder.OrderBy(p => p.Total).ToList();
                        para.AscSortImage4.Visibility = Visibility.Visible;
                    }
                    IsSort = !IsSort;
                }
                
                foreach (StockContentHolder item in SortHolder)
                {
                    InvoiceUC invoiceUC = new InvoiceUC();
                    invoiceUC.InvoiceID.Text = item.ID.ToString();
                    invoiceUC.AgencyName.Text = "Công ty này";
                    invoiceUC.CheckOut.Text = item.CheckIn.Value.ToShortDateString();
                    invoiceUC.Debt.Text = ConvertToString(item.Total);
                    this.HomeWindow.stkStockReceipt.Children.Add(invoiceUC);
                }
            }
        }
        private void PayReceiptBill(ShellOutWindow para)
        {
            if (string.IsNullOrWhiteSpace(para.cbbName.Text))
            {
                para.cbbName.Text = "";
                para.cbbName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(para.txtPayment.Text))
            {
                para.txtPayment.Text = "";
                para.txtPayment.Focus();
                return;
            }


            if (ConvertToNumber(para.txtPayment.Text) != 0)
            {
                Receipt newitem = new Receipt();
                newitem.ID = int.Parse(para.txtID.Text);
                newitem.AgencyID = this.ListAgency[para.cbbName.SelectedIndex].ID;
                newitem.Date = DateTime.Now;
                newitem.Amount = ConvertToNumber(para.txtPayment.Text);
                newitem.Message = para.txtMessage.Text;
                DataProvider.Instance.DB.Receipts.Add(newitem);
                DataProvider.Instance.DB.SaveChanges();

                Agency agency = DataProvider.Instance.DB.Agencies.Where(x => x.ID == newitem.AgencyID).First();
                agency.Debt -= newitem.Amount;
                DataProvider.Instance.DB.Agencies.AddOrUpdate(agency);
                DataProvider.Instance.DB.SaveChanges();

                ReportViewModel reportViewModel = new ReportViewModel();
                reportViewModel.InitColumnChart(this.HomeWindow);
                reportViewModel.LoadSales(this.HomeWindow);

                reportViewModel.LoadChartByAgency();
                reportViewModel.LoadChartByProduct();

                //Init(this.HomeWindow);
                //LoadBill(this.HomeWindow);
                //LoadStockReceipt(this.HomeWindow);
                LoadReceiptBill(this.HomeWindow);

                AgencyReportViewModel agencyReportViewModel = new AgencyReportViewModel();
                agencyReportViewModel.Init(this.HomeWindow);
                agencyReportViewModel.LoadSalesReport(this.HomeWindow);
                agencyReportViewModel.LoadDebtsReport(this.HomeWindow);

                para.Close();
            } 
            else
            {
                CustomMessageBox.Show("Thanh toán phải lớn hơn 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LimitPayment(ShellOutWindow para)
        {
            if (para.txtDebt.Text.Length > 0)
            {
                if (para.txtPayment.Text.Length > 0)
                {
                    long debt = ConvertToNumber(para.txtDebt.Text);
                    long payment = ConvertToNumber(para.txtPayment.Text);
                    if (payment > debt)
                    {
                        CustomMessageBox.Show("Thanh toán không thể vượt quá nợ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        para.txtPayment.Text = "0";
                    }
                }
                else
                    para.txtPayment.Text = "0";
            }
            else
            {
                para.cbbName.Focus();
                para.txtPayment.Text = "0";
            }
        }

        private void ChooseAgency(ShellOutWindow para)
        {
            int idAgency = this.ListAgency[para.cbbName.SelectedIndex].ID;
            Agency item = DataProvider.Instance.DB.Agencies.Where(x => x.ID == idAgency).First();

            para.txtDebt.Text = SeparateThousands(item.Debt.ToString());
            para.txtAddress.Text = item.Address;
        }

        private void ShellOut(HomeWindow para)
        {

            ShellOutWindow wd = new ShellOutWindow();
            LoadListAgency(wd);

            if (DataProvider.Instance.DB.Receipts.Count() > 0)
            {
                Receipt receipt = DataProvider.Instance.DB.Receipts.ToList().Last();
                wd.txtID.Text = (receipt.ID + 1).ToString();
            }
            else
            {
                wd.txtID.Text = "1";
            }
            wd.ShowDialog();
        }

        private void LoadListAgency(ShellOutWindow para)
        {
            ListAgency = DataProvider.Instance.DB.Agencies.Where(x => x.IsDelete == false).ToList();

            para.cbbName.ItemsSource = this.ListAgency;
            para.cbbName.SelectedValuePath = "ID";
            para.cbbName.DisplayMemberPath = "Name";
        }

        //private String SeparateThousands(String txt)
        //{
        //    if (!string.IsNullOrEmpty(txt))
        //    {
        //        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
        //        ulong valueBefore = ulong.Parse(ConvertToNumber(txt).ToString(), System.Globalization.NumberStyles.AllowThousands);
        //        txt = String.Format(culture, "{0:N0}", valueBefore);
        //    }
        //    return txt;
        //}

        public void Init(HomeWindow para)
        {
            this.HomeWindow = para;
            this.HomeWindow.stkBill.Children.Clear();
            List<Invoice> invoices = new List<Invoice>();
            invoices = DataProvider.Instance.DB.Invoices.ToList<Invoice>();
            foreach (Invoice invoice in invoices)
            {
                InvoiceUC invoiceUC = new InvoiceUC();
                invoiceUC.InvoiceID.Text = invoice.ID.ToString();
                invoiceUC.AgencyName.Text = invoice.Agency.Name.ToString();
                invoiceUC.CheckOut.Text = invoice.Checkout.Value.ToShortDateString();
                invoiceUC.Debt.Text = ConvertToString(invoice.Debt);
                total += invoice.Total;
                this.HomeWindow.stkBill.Children.Add(invoiceUC);             
            }
            this.HomeWindow.textCollect.Text = ConvertToString(total);
            this.HomeWindow.comboBoxBill.Text = "Releasing Bill";
            status = 1;
            this.HomeWindow.InvoiceTable.Visibility = System.Windows.Visibility.Visible;
            this.HomeWindow.stkBill.Visibility = System.Windows.Visibility.Visible;
            this.HomeWindow.stkReceiptBill.Visibility = System.Windows.Visibility.Hidden;
            this.HomeWindow.stkStockReceipt.Visibility = System.Windows.Visibility.Hidden;
            this.HomeWindow.ColumnHeaderBill.Visibility = System.Windows.Visibility.Visible;
            this.HomeWindow.ScrollInvoice.Visibility = System.Windows.Visibility.Visible;
            this.HomeWindow.ScrollReceipt.Visibility = System.Windows.Visibility.Hidden;
            this.HomeWindow.ScrollStockReceipt.Visibility = System.Windows.Visibility.Hidden;
            this.HomeWindow.LastBlock.Text = "Nợ";
        }

        public void Clear(HomeWindow para)
        {
            total = 0;
            pay = 0;
        }

        private void ExportExcel(HomeWindow para)
        {
            if (status == 0)
            {
                CustomMessageBox.Show("Chọn loại hóa đơn trước!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" };
            if (sfd.ShowDialog() == true)
            {
                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                application.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook workbook = application.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
                DataTable data = new DataTable();
                if (status == 1)
                {
                    List<Invoice> invoices = DataProvider.Instance.DB.Invoices.ToList();
                    using (var reader = ObjectReader.Create(invoices))
                    {
                        data.Load(reader);
                    }
                    data.Columns.Remove("Agency");
                    data.Columns.Remove("InvoiceInfoes");
                }
                if (status == 2)
                {
                    List<Receipt> receipts = DataProvider.Instance.DB.Receipts.ToList();
                    using (var reader = ObjectReader.Create(receipts))
                    {
                        data.Load(reader);
                    }
                    data.Columns.Remove("Agency");

                }
                if (status == 3)
                {
                    List<StockReceipt> stockReceipts = DataProvider.Instance.DB.StockReceipts.ToList();
                    using (var reader = ObjectReader.Create(stockReceipts))
                    {
                        data.Load(reader);
                    }
                    data.Columns.Remove("StockReceiptInfoes");
                }
                worksheet = application.Worksheets.Add(misValue, misValue, misValue, misValue);
                worksheet.Name = "Bill";
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = data.Columns[i].ColumnName;
                }
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = data.Rows[i][j].ToString();
                    }
                }
                workbook.SaveAs(sfd.FileName);
                workbook.Close();
                application.Quit();
            }
        }
        private void OpenReceiptWindow(ReceiptBillUC para)
        {
            int id = int.Parse(para.ReceiptID.Text);
            Receipt receipt = (Receipt)DataProvider.Instance.DB.Receipts.Where(x => x.ID == id).First();
            ReceiptBillWindow receiptBillWindow = new ReceiptBillWindow();
            receiptBillWindow.txtAgencyName.Text = receipt.Agency.Name;
            receiptBillWindow.txtPhoneNumber.Text = receipt.Agency.PhoneNumber;
            receiptBillWindow.txtAddress.Text = receipt.Agency.Address;
            receiptBillWindow.txtEmail.Text = receipt.Agency.Email;
            receiptBillWindow.txtProceed.Text = receipt.Message;
            receiptBillWindow.dateCheckout.Text = receipt.Date.ToString();
            receiptBillWindow.ShowDialog();
        }
        private void OpenInvoiceWindow(InvoiceUC para)
        {
            if (para.AgencyName.Text == "Công ty này")
            {
                int no = 1;
                StockReceipt stockReceipt = new StockReceipt();
                int id = int.Parse(para.InvoiceID.Text);
                stockReceipt = (StockReceipt)DataProvider.Instance.DB.StockReceipts.Where(x => x.ID == id).First();
                List<StockReceiptInfo> stockReceiptInfos = stockReceipt.StockReceiptInfoes.ToList();
                InvoiceWindow invoiceWindow = new InvoiceWindow();
                invoiceWindow.txbName.Text = "Công ty này";
                invoiceWindow.txbAddress.Text = "khu phố 6, Thủ Đức, Thành phố Hồ Chí Minh";
                invoiceWindow.txbPhone.Text = "+84 28 3725 2002";
                invoiceWindow.txbIDinvoice.Text = stockReceipt.ID.ToString();
                invoiceWindow.txbDate.Text = stockReceipt.CheckIn.Value.ToShortDateString();

                invoiceWindow.stkListProductChosenInvoice.Children.Add(new BillUC());
                foreach (StockReceiptInfo stockReceiptInfo in stockReceiptInfos)
                {
                    BillUC billUC = new BillUC();
                    billUC.ID.Text = no.ToString();
                    no++;
                    billUC.UnitName.Text = stockReceiptInfo.Product.Name.ToString();
                    billUC.Unit.Text = stockReceiptInfo.Product.Unit.Name.ToString();
                    billUC.Amount.Text = ConvertToString(stockReceiptInfo.Amount);
                    billUC.Price.Text = ConvertToString(stockReceiptInfo.Price);
                    billUC.Total.Text = ConvertToString(stockReceiptInfo.Amount * stockReceiptInfo.Price);
                    invoiceWindow.stkListProductChosenInvoice.Children.Add(billUC);
                }
                invoiceWindow.txbTotal.Text = ConvertToString(stockReceipt.Total);
                invoiceWindow.txbRetainer.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbRetainerText.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbRetainerVND.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbChange.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbChangeText.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbChangeVND.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.ShowDialog();
            }
            else
            {
                int no = 1;
                Invoice invoice = new Invoice();
                int id = int.Parse(para.InvoiceID.Text);
                invoice = (Invoice)DataProvider.Instance.DB.Invoices.Where(x => x.ID == id).First();
                List<InvoiceInfo> invoiceInfos = invoice.InvoiceInfoes.ToList();
                InvoiceWindow invoiceWindow = new InvoiceWindow();
                invoiceWindow.txbName.Text = invoice.Agency.Name;
                invoiceWindow.txbAddress.Text = invoice.Agency.Address;
                invoiceWindow.txbPhone.Text = invoice.Agency.PhoneNumber;
                invoiceWindow.txbIDinvoice.Text = invoice.ID.ToString();
                invoiceWindow.txbDate.Text = invoice.Checkout.Value.ToShortDateString();

                invoiceWindow.stkListProductChosenInvoice.Children.Add(new BillUC());
                foreach (InvoiceInfo invoiceInfo in invoiceInfos)
                {
                    Product product = new Product();
                    BillUC billUC = new BillUC();
                    billUC.ID.Text = no.ToString();
                    no++;
                    billUC.UnitName.Text = invoiceInfo.Product.Name.ToString();
                    billUC.Unit.Text = invoiceInfo.Product.Unit.Name.ToString();
                    billUC.Amount.Text = ConvertToString(invoiceInfo.Amount);
                    billUC.Price.Text = ConvertToString(invoiceInfo.Product.ExportPrice);
                    billUC.Total.Text = ConvertToString(invoiceInfo.Total);
                    invoiceWindow.stkListProductChosenInvoice.Children.Add(billUC);
                }
                invoiceWindow.txbTotal.Text = ConvertToString(invoice.Total);
                invoiceWindow.txbRetainer.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbRetainerText.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbRetainerVND.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbChange.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbChangeText.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.txbChangeVND.Visibility = System.Windows.Visibility.Hidden;
                invoiceWindow.ShowDialog();
            }
        }
        private void Search(HomeWindow para)
        {
            this.HomeWindow = para;
            foreach (InvoiceUC control in this.HomeWindow.stkBill.Children)
            {
                if (!control.AgencyName.Text.ToLower().Contains(this.HomeWindow.txtSearchAgencyinBill.Text))
                {
                    control.Visibility = Visibility.Collapsed;
                }
                else
                {
                    control.Visibility = Visibility.Visible;
                }
            }
            foreach (ReceiptBillUC control in this.HomeWindow.stkReceiptBill.Children)
            {
                if (!control.AgencyName.Text.ToLower().Contains(this.HomeWindow.txtSearchAgencyinBill.Text))
                {
                    control.Visibility = Visibility.Collapsed;
                }
                else
                {
                    control.Visibility = Visibility.Visible;
                }
            }
        }
        public void LoadBill(HomeWindow para)
        {
            this.HomeWindow = para;
            this.HomeWindow.stkBill.Children.Clear();
            List<Invoice> invoices = new List<Invoice>();
            invoices = DataProvider.Instance.DB.Invoices.ToList<Invoice>();
            foreach (Invoice invoice in invoices)
            {
                InvoiceUC invoiceUC = new InvoiceUC();
                invoiceUC.InvoiceID.Text = invoice.ID.ToString();
                invoiceUC.AgencyName.Text = invoice.Agency.Name.ToString();
                invoiceUC.CheckOut.Text = invoice.Checkout.Value.ToShortDateString();
                invoiceUC.Debt.Text = ConvertToString(invoice.Debt);
                total += invoice.Total;
                this.HomeWindow.stkBill.Children.Add(invoiceUC);
            }           
            this.HomeWindow.textCollect.Text = ConvertToString(total);
        }
        public void LoadReceiptBill(HomeWindow para)
        {
            this.HomeWindow = para;
            this.HomeWindow.stkReceiptBill.Children.Clear();
            List<Receipt> receipts = new List<Receipt>();
            receipts = DataProvider.Instance.DB.Receipts.ToList<Receipt>();
            foreach (Receipt receipt in receipts)
            {
                ReceiptBillUC receiptBillUC = new ReceiptBillUC();
                receiptBillUC.ReceiptID.Text = receipt.ID.ToString();
                receiptBillUC.AgencyName.Text = receipt.Agency.Name.ToString();
                receiptBillUC.CheckOut.Text = receipt.Date.Value.ToShortDateString();
                receiptBillUC.Amount.Text = ConvertToString(receipt.Amount);
                this.HomeWindow.stkReceiptBill.Children.Add(receiptBillUC);
                total += receipt.Amount;
            }
            this.HomeWindow.textCollect.Text = ConvertToString(total);
        }
        public void LoadStockReceipt(HomeWindow para)
        {
            this.HomeWindow = para;
            this.HomeWindow.stkStockReceipt.Children.Clear();
            List<StockReceipt> stockReceipts = new List<StockReceipt>();
            stockReceipts = DataProvider.Instance.DB.StockReceipts.ToList<StockReceipt>();
            foreach (StockReceipt stockReceipt in stockReceipts)
            {
                InvoiceUC invoiceUC = new InvoiceUC();
                invoiceUC.InvoiceID.Text = stockReceipt.ID.ToString();
                invoiceUC.AgencyName.Text = "Công ty này";
                invoiceUC.CheckOut.Text = stockReceipt.CheckIn.Value.ToShortDateString();
                invoiceUC.Debt.Text = ConvertToString(stockReceipt.Total);
                pay += stockReceipt.Total;
                this.HomeWindow.stkStockReceipt.Children.Add(invoiceUC);
            }
            this.HomeWindow.textPay.Text = ConvertToString(pay);
        }
        private void Switch(HomeWindow para)
        {
            if (para.comboBoxBill.SelectedIndex == 1)
            {
                status = 1;
                para.InvoiceTable.Visibility = System.Windows.Visibility.Visible;
                para.stkBill.Visibility = System.Windows.Visibility.Visible;
                para.stkReceiptBill.Visibility = System.Windows.Visibility.Hidden;
                para.stkStockReceipt.Visibility = System.Windows.Visibility.Hidden;
                para.ColumnHeaderBill.Visibility = System.Windows.Visibility.Visible;
                para.ScrollInvoice.Visibility = System.Windows.Visibility.Visible;
                para.ScrollReceipt.Visibility = System.Windows.Visibility.Hidden;
                para.ScrollStockReceipt.Visibility = System.Windows.Visibility.Hidden;
                para.LastBlock.Text = "Nợ";
                IsSort = false;
                SetSortImageToHidden(para);
            }
            if (para.comboBoxBill.SelectedIndex == 2)
            {
                status = 2;
                para.InvoiceTable.Visibility = System.Windows.Visibility.Visible;
                para.stkBill.Visibility = System.Windows.Visibility.Hidden;
                para.stkReceiptBill.Visibility = System.Windows.Visibility.Visible;
                para.stkStockReceipt.Visibility = System.Windows.Visibility.Hidden;
                para.ColumnHeaderBill.Visibility = System.Windows.Visibility.Visible;
                para.ScrollReceipt.Visibility = System.Windows.Visibility.Visible;
                para.ScrollInvoice.Visibility = System.Windows.Visibility.Hidden;
                para.ScrollStockReceipt.Visibility = System.Windows.Visibility.Hidden;
                para.LastBlock.Text = "Số lượng";
                IsSort = false;
                SetSortImageToHidden(para);
            }
            if (para.comboBoxBill.SelectedIndex == 0)
            {
                status = 3;
                para.InvoiceTable.Visibility = System.Windows.Visibility.Visible;
                para.stkBill.Visibility = System.Windows.Visibility.Hidden;
                para.stkReceiptBill.Visibility = System.Windows.Visibility.Hidden;
                para.stkStockReceipt.Visibility = System.Windows.Visibility.Visible;
                para.ColumnHeaderBill.Visibility = System.Windows.Visibility.Visible;
                para.ScrollInvoice.Visibility = System.Windows.Visibility.Hidden;
                para.ScrollReceipt.Visibility = System.Windows.Visibility.Hidden;
                para.ScrollStockReceipt.Visibility = System.Windows.Visibility.Visible;
                para.LastBlock.Text = "Tổng";
                IsSort = false;
                SetSortImageToHidden(para);
            }

        }
    }
}
