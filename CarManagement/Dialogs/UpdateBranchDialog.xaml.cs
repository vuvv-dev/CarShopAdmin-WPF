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
    /// Interaction logic for UpdateBranchDialog.xaml
    /// </summary>
    public partial class UpdateBranchDialog : UserControl
    {
        private readonly CarBranchServices _carBranchServices;
        private CarBranch CarBranch;

        public UpdateBranchDialog(CarBranch carBranch)
        {
            _carBranchServices = new CarBranchServices();
            InitializeComponent();
            CarBranch = carBranch;
            DataContext = CarBranch;
            LoadInit();
        }

        public void LoadInit()
        {
            if (CarBranch != null)
            {
                BranchNameTextBox.Text = CarBranch.BranchName;
                BranchLogoTextBox.Text = CarBranch.BranchLogo;
            }
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

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
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
            var result = _carBranchServices.UpdateCarBranchesById(CarBranch.Id, carBranch);
            if (result != null)
            {
                MessageBox.Show("Branch updated successfully!");
                DialogHost.CloseDialogCommand.Execute(true, this);
            }
            else
            {
                MessageBox.Show(
                    "Failed to update branch!",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(false, this);
        }
    }
}
