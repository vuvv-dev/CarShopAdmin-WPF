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
    /// Interaction logic for CreateBillDialog.xaml
    /// </summary>
    public partial class CreateBillDialog : UserControl, INotifyPropertyChanged
    {
        private Customer _customer;
        private Car _car;
        private Double _totalPrice;
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
        public Double TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }
        public Car Car
        {
            get => _car;
            set
            {
                _car = value;
                OnPropertyChanged(nameof(Car));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly BillServices _billServices;
        private readonly BillStatuesServices _billStatusesServices;

        public CreateBillDialog(Customer customer, Car car)
        {
            _billServices = new BillServices();
            _billStatusesServices = new BillStatuesServices();
            Customer = customer;
            Car = car;
            TotalPrice = Double.Parse(car.CarPrice) + Double.Parse(car.CarPrice) * 0.2;
            BillStatuses = new ObservableCollection<BillStatus>(
                _billStatusesServices.GetAllBillStatuses().Result.ToList()
            );
            InitializeComponent();
            BillStatusComboBox.SelectedIndex = 0;
            this.DataContext = this;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var bill = new Bill()
                {
                    CustomerId = Customer.Id,
                    CarId = Car.Id,
                    TotalPrice = TotalPrice,
                    BillStatusId = (int)BillStatusComboBox.SelectedValue,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                _billServices.AddNewBill(bill);
                DialogHost.CloseDialogCommand.Execute(true, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(false, this);
        }
    }
}
