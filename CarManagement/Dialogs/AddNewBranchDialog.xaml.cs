using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddNewBranchDialog.xaml
    /// </summary>
    public partial class AddNewBranchDialog : UserControl
    {
        private readonly CarBranchServices _carbranchServices;

        public AddNewBranchDialog()
        {
            _carbranchServices = new CarBranchServices();
            InitializeComponent();
        }

        private void LoadLogoButton_Click(object sender, RoutedEventArgs e)
        {
            string logoUrl = BranchLogoTextBox.Text;
            if (!string.IsNullOrEmpty(logoUrl))
            {
                try
                {
                    // Tạo một BitmapImage từ URL
                    BitmapImage bitmap = new BitmapImage(new Uri(logoUrl));
                    BranchLogoImage.Source = bitmap;

                    // Hiển thị hình ảnh
                    BranchLogoImage.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load image: " + ex.Message);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(false, this);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Trim whitespace from the input to avoid allowing spaces only
            string branchName = BranchNameTextBox.Text.Trim();
            string branchLogo = BranchLogoTextBox.Text.Trim();

            // Check if either text box is empty
            if (string.IsNullOrWhiteSpace(branchName))
            {
                MessageBox.Show(
                    "Branch Name cannot be empty!",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return; // Exit the method
            }

            if (string.IsNullOrWhiteSpace(branchLogo))
            {
                MessageBox.Show(
                    "Branch Logo URL cannot be empty!",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return; // Exit the method
            }

            // Create a new CarBranch instance
            var carBranch = new CarBranch() { BranchName = branchName, BranchLogo = branchLogo };

            // Attempt to add the new CarBranch
            var result = _carbranchServices.AddCarBranches(carBranch);
            if (result != null)
            {
                MessageBox.Show("Branch added successfully!");
                DialogHost.CloseDialogCommand.Execute(true, this);
            }
            else
            {
                MessageBox.Show(
                    "Failed to add branch!",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}
