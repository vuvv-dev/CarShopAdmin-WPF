using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Cars : Page
    {
        private readonly CarBranchServices _carBranchServices;

        public ObservableCollection<CarBranch> CarBrands { get; set; }

        public Cars()
        {
            _carBranchServices = new CarBranchServices();
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            CarBrands = new ObservableCollection<CarBranch>(); // Khởi tạo ObservableCollection
            var branches = _carBranchServices.GetAllCarBranches(); // Phương thức này cần được triển khai
            foreach (var branch in branches)
            {
                CarBrands.Add(branch); // Thêm từng hãng vào ObservableCollection
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddNewCarDialog();
            var result = await DialogHost.Show(dialog, "RootDialog");
        }

        private void EditButton_Click(object sender, RoutedEventArgs e) { }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) { }
    }
}
