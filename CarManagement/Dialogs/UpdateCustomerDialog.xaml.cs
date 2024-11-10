using System;
using System.Collections.Generic;
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
    /// Interaction logic for UpdateCustomerDialog.xaml
    /// </summary>
    public partial class UpdateCustomerDialog : UserControl
    {
        private Customer _selectedCustomer;
        private readonly CustomerService _customerService;

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public UpdateCustomerDialog(Customer selectedCustomer)
        {
            _customerService = new CustomerService();
            InitializeComponent();
            SelectedCustomer = selectedCustomer;
            this.DataContext = this;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(false, this);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(CustomerNameTextBox.Text))
            {
                MessageBox.Show(
                    "Customer Name is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
            if (string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
            {
                MessageBox.Show(
                    "Phone Number is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
            if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                MessageBox.Show(
                    "Address is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || !IsValidEmail(EmailTextBox.Text))
            {
                MessageBox.Show(
                    "A valid Email is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            try
            {
                var customer = new Customer
                {
                    Name = CustomerNameTextBox.Text,
                    Phone = PhoneNumberTextBox.Text,
                    Address = AddressTextBox.Text,
                    Email = EmailTextBox.Text,
                };

                // Gọi dịch vụ để thêm khách hàng mới
                _customerService.UpdateCustomerById(SelectedCustomer.Id, customer);

                // Đóng dialog khi thêm thành công
                DialogHost.CloseDialogCommand.Execute(true, this);
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nếu có lỗi khi thêm khách hàng
                MessageBox.Show(
                    $"An error occurred while updating the customer: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var emailAddress = new System.Net.Mail.MailAddress(email);
                return emailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
