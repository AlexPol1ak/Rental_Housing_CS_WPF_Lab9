using CS_WPF_Lab9_Rental_Housing.ViewModels;
using System.Windows;
using System.Windows.Controls;

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

        /// <summary>
        /// Uniform resizing of columns when the window is resized
        /// </summary>
        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView? listView = sender as ListView;
            if (listView is null) return;
            GridView? gridView = listView.View as GridView;
            if (gridView is null) return;

            double actualWidth = listView.ActualWidth;
            double newWidth = actualWidth / gridView.Columns.Count;

            foreach (var column in gridView.Columns)
            {
                column.Width = newWidth;
            }

        }
    }
}