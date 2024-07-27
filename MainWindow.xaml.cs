using CS_WPF_Lab9_Rental_Housing.Business.Infastructure;
using CS_WPF_Lab9_Rental_Housing.Business.Managers;
using CS_WPF_Lab9_Rental_Housing.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

namespace CS_WPF_Lab9_Rental_Housing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel; 

        public MainWindow()
        {
            InitializeComponent();
            mainWindowViewModel = new MainWindowViewModel();

            this.DataContext = mainWindowViewModel;
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView? listView = sender as ListView;
            if (listView is null) return;
            GridView? gridView = listView.View as GridView;
            if (gridView is null) return;

            double actualWidth = listView.ActualWidth;
            double newWidth = actualWidth/ gridView.Columns.Count;

            foreach(var column in gridView.Columns)
            {
                column.Width = newWidth;
            }
            
        }
    }
}