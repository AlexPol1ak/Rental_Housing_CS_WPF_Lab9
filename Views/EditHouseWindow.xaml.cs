using CS_WPF_Lab9_Rental_Housing.Commands;
using CS_WPF_Lab9_Rental_Housing.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CS_WPF_Lab9_Rental_Housing.Views
{
    /// <summary>
    /// Window for creating and editing a house
    /// </summary>
    public partial class EditHouseWindow : Window, IDataErrorInfo
    {
        private House _selectedHouse;
        public House SelectedHouse
        {
            get { return _selectedHouse; }
            set { _selectedHouse = value; }
        }

        public EditHouseWindow()
        {
            TitleText = "Добавить новый дом";

            InitializeComponent();       
            SelectedHouse = new House();
            _installSettings();
            ReadData();
        }

        public EditHouseWindow(House house) : this()
        {
            TitleText = "Редактировать дом";

            InitializeComponent();
            SelectedHouse = house;
            _installSettings();
            ReadData();
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

        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }
        public static readonly DependencyProperty TitleTextProperty =
            DependencyProperty.Register(nameof(TitleText), typeof(string),
                typeof(EditApartmentWindow), new PropertyMetadata(string.Empty));

        public string City
        {
            get { return (string)GetValue(CityProperty); }
            set
            {
                SetValue(CityProperty, value);
            }
        }
        public static readonly DependencyProperty CityProperty =
            DependencyProperty.Register(nameof(City), typeof(string),
                typeof(EditHouseWindow), new PropertyMetadata(default(string)));

        public string Street
        {
            get { return (string)GetValue(StreetProperty); }
            set
            {
                SetValue(StreetProperty, value);
            }
        }
        public static readonly DependencyProperty StreetProperty =
            DependencyProperty.Register(nameof(Street), typeof(string),
                typeof(EditHouseWindow), new PropertyMetadata(default(string)));

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set
            {
                SetValue(NumberProperty, value);
            }
        }
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register(nameof(Number), typeof(int),
                typeof(EditHouseWindow), new PropertyMetadata(default(int)));

        public int? Block
        {
            get { return (int?)GetValue(BlockProperty); }
            set
            {
                SetValue(BlockProperty, value);
            }
        }
        public static readonly DependencyProperty BlockProperty =
            DependencyProperty.Register(nameof(Block), typeof(int?),
                typeof(EditHouseWindow), new PropertyMetadata(default(int?)));

        public int CountFloors
        {
            get { return (int)GetValue(CountFloorsProperty); }
            set
            {
                SetValue(CountFloorsProperty, value);
            }
        }
        public static readonly DependencyProperty CountFloorsProperty =
            DependencyProperty.Register(nameof(CountFloors), typeof(int),
                typeof(EditHouseWindow), new PropertyMetadata(default(int)));

        public bool HasElevator
        {
            get { return (bool)GetValue(HasElevatorProperty); }
            set {  SetValue(HasElevatorProperty, value);}
        }
        public static readonly DependencyProperty HasElevatorProperty =
            DependencyProperty.Register(nameof(HasElevator), typeof(bool),
                typeof(EditHouseWindow), new PropertyMetadata(false));

        public int BuildingYear
        {
            get { return (int)GetValue(BuildingYearProperty); }
            set
            {
                SetValue(BuildingYearProperty, value);
            }
        }

        public static readonly DependencyProperty BuildingYearProperty =
            DependencyProperty.Register(nameof(BuildingYear), typeof(int),
                typeof(EditHouseWindow), new PropertyMetadata(1994));

        #endregion

        #region Supporting methods
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
                (s, e) => { e.CanExecute = !HasError(); }
                );

            CommandBinding commandBindingExit = new CommandBinding(
                WindowCommands.Exit,
                (s, e) => { this.Close(); }
                );

            this.CommandBindings.Add(commandBindingSave);
            this.CommandBindings.Add(commandBindingExit);

        }

        /// <summary>
        /// Sets the allowable range for the construction date of the house.
        /// </summary>
        private void _loadYearsRange()
        {
            for (int year = 1920; year <= DateTime.Now.Year; year++) RangeYears.Add(year);
        }

        /// <summary>
        /// Sets the allowable range of the number of floors.
        /// </summary>
        private void _loadFloorsRange()
        {
            for (int floor = 1; floor <= 30; floor++) RangeFloors.Add(floor);
        }

        /// <summary>
        /// Saves the entered data to the object received for editing.
        /// </summary>
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

        /// <summary>
        /// Saves data from the received object to the properties of dependencies
        /// bound to input fields.
        /// </summary>
        private void ReadData()
        {
            City = SelectedHouse.City;
            Street = SelectedHouse.Street;
            Number = SelectedHouse.Number;
            Block = SelectedHouse.Block;
            CountFloors = SelectedHouse.CountFloors;
            HasElevator = SelectedHouse.HasElevator;
            BuildingYear = SelectedHouse.BuildingYear;
        }
        #endregion

        #region Validation data

        /// <summary>
        /// Checks for errors in the input fields.
        /// </summary>
        /// <returns></returns>
        public bool HasError()
        {
            foreach (var item in Grid_MainChangeHouse.Children)
            {
                if (item is TextBox tb)
                {
                    var errors = Validation.GetErrors(tb);
                    if (errors.Any()) return true;
                }
                if (item is CheckBox cb)
                {
                    var errors = Validation.GetErrors(cb);
                    if (errors.Any()) return true;
                }
            }
            return false;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(City):
                        if (string.IsNullOrWhiteSpace(City))
                            error = "Город не может быть пустым.";
                        break;
                    case nameof(Street):
                        if (string.IsNullOrWhiteSpace(Street))
                            error = "Улица не может быть пустой.";
                        break;
                    case nameof(Number):
                        if (Number <= 0)
                            error = "Номер дома должен быть больше 0.";
                        break;
                    case nameof(Block):
                        if (Block != null && Block <= 0)
                            error = "Корпус должен быть больше 0 или пустым.";
                        break;
                    case nameof(CountFloors):
                        if (CountFloors <= 0)
                            error = "Количество этажей должно быть больше 0.";
                        break;
                    case nameof(BuildingYear):
                        if (BuildingYear < 1920 || BuildingYear > DateTime.Now.Year)
                            error = $"Год постройки должен быть между 1920 и {DateTime.Now.Year}.";
                        break;  
                    
                }
                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }

        #endregion
    }
}
