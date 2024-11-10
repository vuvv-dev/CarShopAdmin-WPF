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

namespace CarManagement.Pages;

/// <summary>
/// Interaction logic for Customer.xaml
/// </summary>
public partial class Customer : Page, INotifyPropertyChanged
{
    private readonly CustomerService _customerService;

    private ObservableCollection<Domains.Entities.Customer> _customer;

    public ObservableCollection<Domains.Entities.Customer> Customers
    {
        get => _customer;
        set
        {
            _customer = value;
            OnPropertyChanged(nameof(Customers));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public Customer()
    {
        _customerService = new CustomerService();
        InitializeComponent();
        LoadData();
    }

    public void LoadData()
    {
        var customers = _customerService.GetAllCustomer();
        Customers = new ObservableCollection<Domains.Entities.Customer>(customers.Result.ToList());
        CustomerDataGrid.ItemsSource = Customers;
    }

    private async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (
            sender is Button editButton
            && editButton.Tag is Domains.Entities.Customer customerToEdit
        )
        {
            if (customerToEdit != null)
            {
                var dialog = new UpdateCustomerDialog(customerToEdit);
                var result = await DialogHost.Show(dialog, "RootDialog");
                if (result != null && (bool)result)
                {
                    LoadData();
                }
            }
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (
            sender is Button deleteButton
            && deleteButton.Tag is Domains.Entities.Customer customerToDelete
        )
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete {customerToDelete.Name}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
            if (result == MessageBoxResult.Yes)
            {
                var deleteResult = _customerService.DeleteCustomerById(customerToDelete.Id).Result;
                if (!deleteResult)
                {
                    MessageBox.Show(
                        "Failed to delete customer!",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
                Customers.Remove(customerToDelete);
                LoadData();
            }
        }
        else
        {
            MessageBox.Show(
                "Failed to delete customer!",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
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
            var filteredCustomers = _customerService.SearchCustomerByName(SearchTextBox.Text);
            Customers = new ObservableCollection<Domains.Entities.Customer>(
                filteredCustomers.Result.ToList()
            );
            CustomerDataGrid.ItemsSource = Customers;
        }
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddNewCustomerDialog();
        var result = await DialogHost.Show(dialog, "RootDialog");

        if (result != null && (bool)result)
        {
            LoadData();
        }
    }

    private void ViewButton_Click(object sender, RoutedEventArgs e)
    {
        // Your event handler code here
    }
}
