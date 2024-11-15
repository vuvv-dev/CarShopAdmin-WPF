﻿using System;
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
    /// Interaction logic for Cars.xaml
    /// </summary>
    public partial class Cars : Page, INotifyPropertyChanged
    {
        private readonly CarBranchServices _carBranchServices;
        private readonly CarService _carService;
        private ObservableCollection<CarBranch> _carBrands;
        private ObservableCollection<Car> _carModels;
        private Border _selectedBorder = null;

        private string _selectedBrandName = "None";

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

        public Cars()
        {
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

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddNewCarDialog();
            var result = await DialogHost.Show(dialog, "RootDialog");
            if (result != null && (bool)result)
            {
                var newCar = await _carService.GetLastedCar();
                CarModels.Add(newCar);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button editButton && editButton.Tag is Car carToEdit)
            {
                var dialog = new UpdateCarDialog(carToEdit);
                var result = await DialogHost.Show(dialog, "RootDialog");

                if (result != null && (bool)result)
                {
                    LoadCars();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is Car carToDelete)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete {carToDelete.CarName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    var deleteResult = _carService.DeleteCarById(carToDelete.Id).Result;
                    if (!deleteResult)
                    {
                        MessageBox.Show(
                            "Failed to delete car!",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                    CarModels.Remove(carToDelete);
                }
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadCars();
        }

        private void Branch_Click(object sender, RoutedEventArgs e)
        {
            CarBranchs carBranchs = new CarBranchs();

            if (NavigationService != null)
            {
                NavigationService.Navigate(carBranchs);
            }
            else
            {
                var window = new Window() { Content = new CarBranchs() };
                window.Show();
            }
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
    }
}
