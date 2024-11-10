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
using Microsoft.Win32;

namespace CarManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for UpdateCarDialog.xaml
    /// </summary>
    public partial class UpdateCarDialog : UserControl, INotifyPropertyChanged
    {
        public Car _car;
        private ObservableCollection<CarSample> _samples;
        private ObservableCollection<CarBranch> _branches;
        private ObservableCollection<CarStatus> _statuses;
        private string _selectedColorHex;
        public string SelectedColorHex
        {
            get => _selectedColorHex;
            set
            {
                _selectedColorHex = value;
                OnPropertyChanged(nameof(SelectedColorHex));
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

        public ObservableCollection<CarSample> Samples
        {
            get => _samples;
            set
            {
                _samples = value;
                OnPropertyChanged(nameof(Samples));
            }
        }

        public ObservableCollection<CarBranch> Branches
        {
            get => _branches;
            set
            {
                _branches = value;
                OnPropertyChanged(nameof(Branches));    
            }
        }

        public ObservableCollection<CarStatus> Statuses
        {
            get => _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged(nameof(Statuses));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly CarSampleServices _carSampleServices;
        private readonly CarBranchServices _carBranchServices;
        private readonly CarStatusServices _carStatusServices;

        private readonly CarService _carService;

        public UpdateCarDialog(Car car)
        {
            _carSampleServices = new CarSampleServices();
            _carBranchServices = new CarBranchServices();
            _carStatusServices = new CarStatusServices();
            _carService = new CarService();
            Car = car;
            InitializeComponent();
            this.DataContext = this;
            LoadInit();
        }

        private void LoadInit()
        {
            Samples = new ObservableCollection<CarSample>(_carSampleServices.GetAllCarSamples());
            Branches = new ObservableCollection<CarBranch>(_carBranchServices.GetAllCarBranches());
            Statuses = new ObservableCollection<CarStatus>(_carStatusServices.GetAllCarStatuses());
            SelectedColorHex = _car.Color;
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CarNameTextBox.Text))
                {
                    MessageBox.Show(
                        "Car name is required.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                if (string.IsNullOrWhiteSpace(CarCodeTextBox.Text))
                {
                    MessageBox.Show(
                        "Car code is required.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                if (!decimal.TryParse(CarPriceTextBox.Text, out var price) || price <= 0)
                {
                    MessageBox.Show(
                        "Please enter a valid positive price.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                if (
                    !int.TryParse(StorageAmountTextBox.Text, out var storageAmount)
                    || storageAmount < 0
                )
                {
                    MessageBox.Show(
                        "Please enter a valid storage amount (0 or greater).",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                if (BranchComboBox.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Please select a branch.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                if (StatusComboBox.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Please select a status.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                if (SampleComboBox.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Please select a sample.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                if (MadeInDatePicker.SelectedDate == null)
                {
                    MessageBox.Show(
                        "Please select a valid date for Made In Date.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                string base64Image = null;
                if (CarImagePreview.Source != null)
                {
                    var bitmap = CarImagePreview.Source as BitmapImage;
                    if (bitmap != null)
                    {
                        // Create a memory stream to hold the image data
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            // Save the image data to the memory stream in a common format (e.g., PNG)
                            var encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bitmap));
                            encoder.Save(memoryStream);

                            // Convert the byte array to a Base64 string
                            base64Image = Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }
                }
                var car = new Car
                {
                    CarName = CarNameTextBox.Text,
                    CarCode = CarCodeTextBox.Text,
                    CarImage = base64Image!,
                    CarSampleId = (int)SampleComboBox.SelectedValue,
                    CarStatusId = (int)StatusComboBox.SelectedValue,
                    CarBranchId = (int)BranchComboBox.SelectedValue,
                    CarPrice = CarPriceTextBox.Text,
                    CarDescription = CarDescriptionTextBox.Text,
                    Color = SelectedColorHex,
                    MadeInDate =
                        MadeInDatePicker.SelectedDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    StorageAmount = int.TryParse(
                        StorageAmountTextBox.Text,
                        out var parsedStorageAmount
                    )
                        ? parsedStorageAmount
                        : 0,
                };

                var isAdded = await _carService.UpdateCarById(Car.Id, car);
                if (!isAdded)
                {
                    MessageBox.Show(
                        "Error when trying to update this car",
                        "Unexpected error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }
                else
                {
                    DialogHost.CloseDialogCommand.Execute(true, this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error when trying to update this car",
                    "Unexpected error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(false, this);
        }

        private void PickColorButton_Click(object sender, RoutedEventArgs e)
        {
            var colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.CustomColors = new int[]
            {
                0xFF5733, // Red
                0x33FF57, // Green
                0x3357FF, // Blue
                0xFFFF33, // Yellow
                0xFF33FF, // Magenta
                0x33FFFF, // Cyan
                0xFF00FF, // Purple
                0x00FF00, // Lime
                0xFF8000, // Orange
                0x8000FF, // Violet
                0x00FFFF, // Aqua
                0xFFFF00, // Yellow
                0x800080, // Purple
                0x00FFCC, // Light Cyan
                0xFF9966, // Peach
                0xFFCC00 // Gold
                ,
            };
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedColor = colorDialog.Color;
                SelectedColorHex =
                    $"#{selectedColor.A:X2}{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
            }
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
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
