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
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace CarManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for AddNewCarDialog.xaml
    /// </summary>
    public partial class AddNewCarDialog : UserControl
    {
        public AddNewCarDialog()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại chọn file
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Choose a Car Image",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Hiển thị hình ảnh đã chọn
                CarImagePreview.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                CarImagePreview.Visibility = Visibility.Visible;
            }
        }
    }
}
