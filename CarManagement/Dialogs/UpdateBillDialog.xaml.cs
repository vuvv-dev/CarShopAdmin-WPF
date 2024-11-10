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
    /// Interaction logic for UpdateBillDialog.xaml
    /// </summary>
    public partial class UpdateBillDialog : UserControl, INotifyPropertyChanged
    {
        private Bill Bill;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly BillStatuesServices _billStatusesServices;
        private readonly BillServices _billServices;

        public UpdateBillDialog(Bill bill)
        {
            Bill = bill;
            _billServices = new BillServices();
            _billStatusesServices = new BillStatuesServices();
            InitializeComponent();
            BillStatuses = new ObservableCollection<BillStatus>(
                _billStatusesServices.GetAllBillStatuses().Result.ToList()
            );
            BillStatusComboBox.SelectedValue = Bill.BillStatusId;
            BillIdTextBlock.Text = $"Update for bill with ID: #ID-{Bill.Id}";
            this.DataContext = this;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(false, this);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var billStatusId = BillStatusComboBox.SelectedValue.ToString();

            if (billStatusId == null || billStatusId == "")
            {
                MessageBox.Show("Please select a bill status.");
                return;
            }

            Bill.BillStatusId = int.Parse(billStatusId);
            Bill.UpdateDate = DateTime.Now;
            var result = _billServices.UpdateBillById(Bill.Id, Bill);
            if (result.Result == false)
            {
                MessageBox.Show("Update bill failed.");
                return;
            }
            DialogHost.CloseDialogCommand.Execute(true, this);
        }
    }
}
