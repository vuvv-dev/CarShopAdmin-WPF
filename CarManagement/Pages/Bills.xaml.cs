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
using CarManagement.Dialogs;
using Domains.Entities;
using MaterialDesignThemes.Wpf;

namespace CarManagement.Pages
{
    /// <summary>
    /// Interaction logic for Bills.xaml
    /// </summary>
    public partial class Bills : Page
    {
        private ObservableCollection<Bill> _bills;
        public ObservableCollection<Bill> BillList
        {
            get => _bills;
            set
            {
                _bills = value;
                OnPropertyChanged(nameof(BillList));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly BillServices _billServices;

        public Bills()
        {
            _billServices = new BillServices();
            InitializeComponent();
            this.DataContext = this;
            LoadData();
        }

        public void LoadData()
        {
            var bills = _billServices.GetAllBills();
            BillList = new ObservableCollection<Bill>(bills.Result.ToList());
            BillDataGrid.ItemsSource = BillList;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectCustomerDialog();
            var resultOfSelectCustomer = await DialogHost.Show(dialog, "RootDialog");
            if (resultOfSelectCustomer != null && (bool)resultOfSelectCustomer)
            {
                var selectedCustomer = dialog.SelectedCustomer;
                var dialog2 = new SelectCarDialog(selectedCustomer);
                var resultOfSelectCar = await DialogHost.Show(dialog2, "RootDialog");
                if (resultOfSelectCar != null && (bool)resultOfSelectCar)
                {
                    var selectedCar = dialog2.SelectedCar;
                    var dialog3 = new CreateBillDialog(selectedCustomer, selectedCar);
                    var result = await DialogHost.Show(dialog3, "RootDialog");
                    if (result != null && (bool)result)
                    {
                        LoadData();
                    }
                }
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button editButton && editButton.Tag is Bill billToEdit)
            {
                var dialog = new UpdateBillDialog(billToEdit);
                var result = await DialogHost.Show(dialog, "RootDialog");
                if (result != null && (bool)result)
                {
                    LoadData();
                }
            }
        }

        private async void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button viewButton && viewButton.Tag is Bill billToView)
            {
                var dialog = new ViewBillDetailDialog(billToView.Id);
                var result = await DialogHost.Show(dialog, "RootDialog");
                if (result != null && (bool)result)
                {
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is Bill billToDelete)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the bill {billToDelete.Car.CarName} of {billToDelete.Customer.Name}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );
                if (result == MessageBoxResult.Yes)
                {
                    var deleteResult = _billServices.DeleteBillById(billToDelete.Id).Result;
                    if (!deleteResult)
                    {
                        MessageBox.Show(
                            "Failed to delete car!",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                    BillList.Remove(billToDelete);
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                LoadData();
            }
            else
            {
                var filteredBills = _billServices.SearchBills(SearchTextBox.Text);
                BillList = new ObservableCollection<Bill>(filteredBills.Result.ToList());
                BillDataGrid.ItemsSource = BillList;
            }
        }
    }
}
