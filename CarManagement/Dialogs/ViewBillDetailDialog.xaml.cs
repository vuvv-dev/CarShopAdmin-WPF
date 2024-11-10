using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLayer.Services;
using Domains.Entities;
using MaterialDesignThemes.Wpf;

namespace CarManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for ViewBillDetailDialog.xaml
    /// </summary>
    public partial class ViewBillDetailDialog : UserControl, INotifyPropertyChanged
    {
        private int BillID;
        private Bill _bills;
        private ObservableCollection<BillStatus> _billStatuses;
        public ObservableCollection<BillStatus> BillStatuses
        {
            get => _billStatuses;
            set
            {
                _billStatuses = value;
                OnPropertyChanged(nameof(BillStatuses));
            }
        }
        public Bill BillDetail
        {
            get { return _bills; }
            set
            {
                _bills = value;
                OnPropertyChanged("BillDetail");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly BillServices _billServices;

        public ViewBillDetailDialog(int billID)
        {
            BillID = billID;
            _billServices = new BillServices();
            InitializeComponent();
            LoadData();
            this.DataContext = this;
        }

        public void LoadData()
        {
            var bills = _billServices.GetBillById(BillID);
            BillDetail = bills.Result;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(true, this);
        }
    }
}
