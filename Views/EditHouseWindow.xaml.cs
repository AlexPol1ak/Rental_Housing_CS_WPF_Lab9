using CS_WPF_Lab9_Rental_Housing.Commands;
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
            _installSettings();
            showData();
        }

        public EditHouseWindow(House house) : this()
        {
            this.Title = "Редактировать дом";
            SelectedHouse = house;
            _installSettings();
            showData();
        }

        #region ComboBox Data
        private List<int> _rangeYears = new List<int>();
        private List<int> _rangeFloors = new List<int>();

        public List<int> RangeYears 
        { 
            get => _rangeYears; 
            private set => _rangeYears = value; 
        }
        public List<int> RangeFloors 
        { 
            get => _rangeFloors; 
            private set => _rangeFloors = value; 
        }
        #endregion

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

        /// <summary>
        /// Sets the commands for the save and exit buttons. 
        /// Displays object data, sets initial selection data for ComboBox.
        /// </summary>
        public void _installSettings()
        {
            _loadYearsRange();
            _loadFloorsRange();

            CommandBinding commandBindingSave = new CommandBinding(
                ApplicationCommands.Save, 
                (s, e) =>
                {
                    changeHouse();
                    this.DialogResult = true;
                    this.Close();
                },
                (s, e) => { e.CanExecute = canChangeHouse(); }
                );
            
            CommandBinding commandBindingExit = new CommandBinding(
                WindowCommands.Exit,
                (s, e) => { this.Close(); }
                );

            this.CommandBindings.Add(commandBindingSave);
            this.CommandBindings.Add(commandBindingExit);

            Btn_Cancel.Command = WindowCommands.Exit;
         
        }

        private void _loadYearsRange()
        {
            for(int year = 1920; year <=DateTime.Now.Year;  year++) RangeYears.Add(year);
        }

        private void _loadFloorsRange()
        {
            for(int floor = 1; floor <=30; floor++) RangeFloors.Add(floor);
        }

        public bool canChangeHouse()
        {

            return true;
        }

        private void changeHouse()
        {
            MessageBox.Show(SelectedHouse.ToString());
            SelectedHouse.City = City;
            SelectedHouse.Street = Street;
            SelectedHouse.Number = Number;
            SelectedHouse.Block = Block;
            SelectedHouse.CountFloors = CountFloors;
            SelectedHouse.HasElevator = HasElevator;
            SelectedHouse.BuildingYear = BuildingYear;
            MessageBox.Show(SelectedHouse.ToString());
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
