using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BusinessLayer.Services;
using CarManagement.Dialogs;
using Domains.Entities;
using MaterialDesignThemes.Wpf;

namespace CarManagement.Pages;

/// <summary>
/// Interaction logic for CarBranchs.xaml
/// </summary>
public partial class CarBranchs : Page
{
    private readonly CarBranchServices _carBranchServices;
    private ObservableCollection<CarBranch>? _allBranches;
    public ObservableCollection<CarBranch>? FilteredBranches { get; set; }

    public CarBranchs()
    {
        _carBranchServices = new CarBranchServices();
        InitializeComponent();
        LoadBranches();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        LoadBranches();
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddNewBranchDialog();
        var result = await DialogHost.Show(dialog, "RootDialog");

        if (result != null && (bool)result)
        {
            LoadBranches();
        }
    }

    private void BranchListView_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

    private void LoadBranches()
    {
        // Giả sử bạn có một nguồn dữ liệu chứa danh sách các chi nhánh
        _allBranches = new ObservableCollection<CarBranch>(
            _carBranchServices.GetAllCarBranches().ToList()
        );
        FilteredBranches = new ObservableCollection<CarBranch>(_allBranches);
        BranchListView.ItemsSource = FilteredBranches;
    }

    private async void EditCarBrand_Click(object sender, RoutedEventArgs e)
    {
        if (BranchListView.SelectedItem is CarBranch selectedBrand)
        {
            var dialog = new UpdateBranchDialog(selectedBrand);
            var result = await DialogHost.Show(dialog, "RootDialog");
            if (result != null && (bool)result)
            {
                LoadBranches();
            }
        }
    }

    private void DeleteCarBrand_Click(object sender, RoutedEventArgs e)
    {
        if (BranchListView.SelectedItem is CarBranch selectedBrand)
        {
            var confirmResult = MessageBox.Show(
                "Are you sure you want to delete this branch?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
            if (confirmResult == MessageBoxResult.Yes)
            {
                var isDeleted = _carBranchServices.DeleteCarBranchesById(selectedBrand.Id);
                if (isDeleted?.Result == true)
                {
                    LoadBranches();
                    MessageBox.Show(
                        "Branch deleted successfully!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Failed to delete branch!",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        FilterBranches(SearchTextBox.Text);
    }

    private void FilterBranches(string searchText)
    {
        // Clear the filtered list and add only the branches that match the search
        FilteredBranches?.Clear();

        var filtered = _allBranches
            ?.Where(b =>
                string.IsNullOrEmpty(searchText)
                || b.BranchName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        foreach (var branch in filtered!)
        {
            FilteredBranches?.Add(branch);
        }
    }
}
