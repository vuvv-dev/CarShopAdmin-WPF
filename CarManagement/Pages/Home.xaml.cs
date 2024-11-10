using System.Collections.ObjectModel;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;

namespace CarManagement.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        // Fake data for sales growth
        public ChartValues<double> SalesValues { get; set; }

        // Fake data for inventory stock
        public ChartValues<int> InventoryValues { get; set; }

        // Fake data for customer sales
        public ChartValues<double> CustomerSalesData { get; set; }

        // Data to show percentage change for sales, inventory, and customer sales
        public ChartValues<double> SalesPercentageChange { get; set; }
        public ChartValues<double> InventoryPercentageChange { get; set; }
        public ChartValues<double> CustomerSalesPercentageChange { get; set; }

        public Home()
        {
            InitializeComponent();
            // Fake data for sales growth
            SalesValues = new ChartValues<double>
            {
                150000,
                175000,
                200000,
                225000,
                250000,
                275000,
                300000,
                350000,
                400000,
                450000,
                500000,
            };

            // Fake data for inventory stock
            InventoryValues = new ChartValues<int>
            {
                100,
                150,
                50,
                200,
                75,
                125,
                100,
                180,
                220,
                300,
                250,
            };

            // Fake data for customer sales
            CustomerSalesData = new ChartValues<double> { 100, 200, 300, 400, 500 };

            // Calculate percentage changes
            SalesPercentageChange = CalculatePercentageChange(SalesValues);
            InventoryPercentageChange = CalculatePercentageChange(InventoryValues);
            CustomerSalesPercentageChange = CalculatePercentageChange(CustomerSalesData);

            this.DataContext = this;
        }

        // Method to calculate percentage change
        private ChartValues<double> CalculatePercentageChange(ChartValues<double> values)
        {
            var percentageChanges = new ChartValues<double>();

            for (int i = 1; i < values.Count; i++)
            {
                var change = ((values[i] - values[i - 1]) / values[i - 1]) * 100;
                percentageChanges.Add(change);
            }

            // Insert 0 for the first item, since no percentage change can be calculated for it
            percentageChanges.Insert(0, 0);

            return percentageChanges;
        }

        // Overloaded method for Inventory (int type values)
        private ChartValues<double> CalculatePercentageChange(ChartValues<int> values)
        {
            var percentageChanges = new ChartValues<double>();

            for (int i = 1; i < values.Count; i++)
            {
                var change = ((values[i] - values[i - 1]) / (double)values[i - 1]) * 100;
                percentageChanges.Add(change);
            }

            percentageChanges.Insert(0, 0); // Insert 0 for the first item
            return percentageChanges;
        }
    }
}
