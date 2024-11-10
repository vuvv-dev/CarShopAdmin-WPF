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

namespace CarManagement.Dialogs;

/// <summary>
/// Interaction logic for SelectCustomerDialog.xaml
/// </summary>
public partial class SelectCustomerDialog : UserControl
{
    private readonly CustomerService _customerServices;
    private ObservableCollection<Customer> _customer;
    private Customer _selectedCustomer;

    public Customer SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            _selectedCustomer = value;
            OnPropertyChanged(nameof(SelectedCustomer));
        }
    }

    public ObservableCollection<Customer> Customers
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

    public SelectCustomerDialog()
    {
        _customerServices = new CustomerService();
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        // Fetch and populate data for Samples, Branches, and Statuses
        var customers = _customerServices.GetAllCustomer().Result;
        Customers = new ObservableCollection<Domains.Entities.Customer>(customers.ToList());
        CustomerDataGrid.ItemsSource = Customers;
    }

    private async void SelectButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button selectButton && selectButton.Tag is Customer customer)
        {
            if (customer != null)
            {
                SelectedCustomer = customer;
                DialogHost.CloseDialogCommand.Execute(true, this);
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
            var filteredCustomers = _customerServices.SearchCustomerByName(SearchTextBox.Text);
            Customers = new ObservableCollection<Customer>(filteredCustomers.Result.ToList());
            CustomerDataGrid.ItemsSource = Customers;
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        DialogHost.CloseDialogCommand.Execute(false, this);
    }
}
