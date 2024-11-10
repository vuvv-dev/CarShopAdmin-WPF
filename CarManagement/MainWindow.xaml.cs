using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            navframe.Navigate(new Uri("/Pages/Home.xaml", UriKind.Relative));
        }

        private void sideBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = sideBar.SelectedItem as NavigationButton;

            if (selectedItem != null)
            {
                navframe.Navigate(selectedItem?.NavLink);
            }
        }

        private void navframe_Navigated(object sender, NavigationEventArgs e) { }
    }
}
