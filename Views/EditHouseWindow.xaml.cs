using CS_WPF_Lab9_Rental_Housing.Domain.Entities;
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
using System.Windows.Shapes;

namespace CS_WPF_Lab9_Rental_Housing.Views
{
    /// <summary>
    /// Window for creating and editing a house
    /// </summary>
    public partial class EditHouseWindow : Window
    {
        private House _selectedHouse;
        public House SelectedHouse
        {
            get { return _selectedHouse; }
            set { _selectedHouse = value; }
        }

        public EditHouseWindow()
        {
            InitializeComponent();
            this.Title = "Добавить новый дом";
            SelectedHouse = new House();
            showData();
        }

        public EditHouseWindow(ref House house) : this()
        {
            this.Title = "Редактировать дом";
            SelectedHouse = house;
            showData();
        }

        #region Dependency properties

        public string City
        {
            get { return (string)GetValue(CityProperty); }
            set { SetValue(CityProperty, value); }
        }
        public static readonly DependencyProperty CityProperty =
            DependencyProperty.Register(nameof(City), typeof(string),
                typeof(EditHouseWindow), new PropertyMetadata(default(string)));

        public string Street
        {
            get { return (string)GetValue(StreetProperty); }
            set { SetValue(StreetProperty, value); }
        }
        public static readonly DependencyProperty StreetProperty =
            DependencyProperty.Register(nameof(Street), typeof(string),
                typeof(EditHouseWindow), new PropertyMetadata(default(string)));

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register(nameof(Number), typeof(int),
                typeof(EditHouseWindow), new PropertyMetadata(default(int)));

        public int? Block
        {
            get { return (int?)GetValue(BlockProperty); }
            set { SetValue(BlockProperty, value); }
        }
        public static readonly DependencyProperty BlockProperty =
            DependencyProperty.Register(nameof(Block), typeof(int?),
                typeof(EditHouseWindow), new PropertyMetadata(default(int?)));

        public int CountFloors
        {
            get { return (int)GetValue(CountFloorsProperty); }
            set { SetValue(CountFloorsProperty, value); }
        }
        public static readonly DependencyProperty CountFloorsProperty =
            DependencyProperty.Register(nameof(CountFloors), typeof(int),
                typeof(EditHouseWindow), new PropertyMetadata(default(int)));

        public bool HasElevator
        {
            get { return (bool)GetValue(HasElevatorProperty); }
            set { SetValue(HasElevatorProperty, value); }
        }
        public static readonly DependencyProperty HasElevatorProperty =
            DependencyProperty.Register(nameof(HasElevator), typeof(bool),
                typeof(EditHouseWindow), new PropertyMetadata(false));

        public int BuildingYear
        {
            get { return (int)GetValue(BuildingYearProperty); }
            set { SetValue(BuildingYearProperty, value); }
        }
        public static readonly DependencyProperty BuildingYearProperty =
            DependencyProperty.Register(nameof(BuildingYear), typeof(int),
                typeof(EditHouseWindow), new PropertyMetadata(1994));

        #endregion

        private void changeHouse()
        {
            SelectedHouse.City = City;
            SelectedHouse.Street = Street;
            SelectedHouse.Number = Number;
            SelectedHouse.Block = Block;
            SelectedHouse.CountFloors = CountFloors;
            SelectedHouse.HasElevator = HasElevator;
            SelectedHouse.BuildingYear = BuildingYear;
        }

        private void showData()
        {
            City = SelectedHouse.City;
            Street = SelectedHouse.Street;
            Number = SelectedHouse.Number;
            Block = SelectedHouse.Block;
            CountFloors = SelectedHouse.CountFloors;
            HasElevator = SelectedHouse.HasElevator;
            BuildingYear = SelectedHouse.BuildingYear;
        }
            
    }

}
