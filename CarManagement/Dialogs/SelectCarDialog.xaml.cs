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
    /// Interaction logic for SelectCarDialog.xaml
    /// </summary>
    public partial class SelectCarDialog : UserControl, INotifyPropertyChanged
    {
        private readonly CarBranchServices _carBranchServices;
        private readonly CarService _carService;
        private ObservableCollection<CarBranch> _carBrands;
        private ObservableCollection<Car> _carModels;
        private Border _selectedBorder = null;
        private Customer _customer;
        private Car _selectedCar;

        public Car SelectedCar
        {
            get => _selectedCar;
            set
            {
                _selectedCar = value;
                OnPropertyChanged(nameof(SelectedCar));
            }
        }

        private string _selectedBrandName = "None";

        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        public string SelectedBrandName
        {
            get => _selectedBrandName;
            set
            {
                _selectedBrandName = value;
                OnPropertyChanged(nameof(SelectedBrandName));
            }
        }

        public ObservableCollection<CarBranch> CarBrands
        {
            get => _carBrands;
            set
            {
                _carBrands = value;
                OnPropertyChanged(nameof(CarBrands));
            }
        }

        public ObservableCollection<Car> CarModels
        {
            get => _carModels;
            set
            {
                _carModels = value;
                OnPropertyChanged(nameof(CarModels));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public SelectCarDialog(Customer customer)
        {
            Customer = customer;
            _carBranchServices = new CarBranchServices();
            _carService = new CarService();
            InitializeComponent();
            DataContext = this;
            LoadBranches();
            LoadCars();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBranches();
            LoadCars();
        }

        private void LoadBranches()
        {
            // Giả sử bạn có một nguồn dữ liệu chứa danh sách các chi nhánh
            CarBrands = new ObservableCollection<CarBranch>(
                _carBranchServices.GetAllCarBranches().ToList()
            );
        }

        private async void LoadCars()
        {
            CarModels = new ObservableCollection<Car>(await _carService.GetAllCar());
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadCars();
            SearchCarByName(SearchTextBox.Text);
        }

        private void SearchCarByName(string searchValue)
        {
            var filtered = _carModels
                ?.Where(c => c.CarName.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                .ToList();
            CarModels = new ObservableCollection<Car>(filtered!);
        }

        private void OnMouseEnterBrand(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                BrandSelection.SetIsHovered(border, true);
            }
        }

        private void OnMouseLeaveBrand(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                BrandSelection.SetIsHovered(border, false);
            }
        }

        private void OnBrandSelected(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                // Deselect previous selection
                if (_selectedBorder != null)
                {
                    BrandSelection.SetIsSelected(_selectedBorder, false);
                }

                // Select new brand
                BrandSelection.SetIsSelected(border, true);
                _selectedBorder = border;

                // Get the selected Brand for further logic
                if (border.DataContext is CarBranch selectedBrand)
                {
                    LoadCars();
                    SelectedBrandName = selectedBrand.BranchName;
                    FilterCarsByBrand(selectedBrand);
                }
            }
        }

        private void FilterCarsByBrand(CarBranch selectedBrand)
        {
            CarModels.Clear();
            var filtered = _carService.GetCarByBranchId(selectedBrand.Id);
            if (filtered is not null)
            {
                CarModels = new ObservableCollection<Car>(filtered.Result);
            }
        }

        private void ClearFilter_Click(object sender, MouseButtonEventArgs e)
        {
            SelectedBrandName = "None";
            LoadCars();
        }

        private async void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button selectButton && selectButton.Tag is Car car)
            {
                if (car != null)
                {
                    SelectedCar = car;
                    DialogHost.CloseDialogCommand.Execute(true, this);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(false, this);
        }
    }
}
