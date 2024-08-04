using CS_WPF_Lab9_Rental_Housing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
    /// The window for creating or editing an apartment.
    /// </summary>
    public partial class EditApartmentWindow : Window, IDataErrorInfo
    {
        private House _selectedHouse;
        private Apartment _selectedApartment;

        public House SelectedHouse
        {
            get { return _selectedHouse; }
            private set { _selectedHouse = value; }
        }
        public Apartment SelectedApartment
        {
            get { return _selectedApartment; }
            private set { _selectedApartment = value; }
        }

        public EditApartmentWindow(House house)
        {
            TitleTextAp = "Добавить квартиру";
            InitializeComponent();        
            SelectedHouse = house;
            SelectedApartment = new Apartment();
            ReadData();
        }

        public EditApartmentWindow(Apartment apartment)
        {
            TitleTextAp = "Редактировать квартиру";
            InitializeComponent();
            SelectedHouse = apartment.House;
            SelectedApartment = apartment;
            ReadData();
            
        }

        #region Dependency properties
        public string HouseInfoText
        {
            get { return (string) GetValue(HouseInfoTextProperty); }
            set { SetValue(HouseInfoTextProperty, value); }
        }
        public static readonly DependencyProperty HouseInfoTextProperty =
            DependencyProperty.Register(nameof(HouseInfoText), typeof(string),
                typeof(EditApartmentWindow), new PropertyMetadata(string.Empty));

        public string TitleTextAp
        {
            get { return(string) GetValue(TitleTextApProperty); }
            set { SetValue(TitleTextApProperty, value); }
        }
        public static readonly DependencyProperty TitleTextApProperty =
            DependencyProperty.Register(nameof(TitleTextAp), typeof(string),
                typeof(EditApartmentWindow), new PropertyMetadata(string.Empty));

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register(nameof(Number), typeof(int),
                typeof(EditApartmentWindow), new PropertyMetadata(default(int)));

        public int Floor
        {
            get { return (int)GetValue(FloorProperty); }
            set { SetValue(FloorProperty, value); }
        }
        public static readonly DependencyProperty FloorProperty =
            DependencyProperty.Register(nameof(Floor), typeof(int),
                typeof(EditApartmentWindow), new PropertyMetadata(default(int)));

        public int CountRooms
        {
            get { return (int)GetValue(CountRoomsProperty); }
            set { SetValue(CountRoomsProperty, value); }
        }
        public static readonly DependencyProperty CountRoomsProperty =
            DependencyProperty.Register(nameof(CountRooms), typeof(int),
                typeof(EditApartmentWindow), new PropertyMetadata(default(int)));

        public double Area
        {
            get { return (double)GetValue(AreaProperty); }
            set { SetValue(AreaProperty, value); }
        }
        public static readonly DependencyProperty AreaProperty =
            DependencyProperty.Register(nameof(Area), typeof(double),
                typeof(EditApartmentWindow), new PropertyMetadata(default(double)));

        public string? OwnerFIO
        {
            get { return (string)GetValue(OwnerFIOProperty); }
            set { SetValue(OwnerFIOProperty, value); }
        }
        public static readonly DependencyProperty OwnerFIOProperty =
            DependencyProperty.Register(nameof(OwnerFIO), typeof(string),
                typeof(EditApartmentWindow), new PropertyMetadata(default(string)));

        public ulong? OwnerTel
        {
            get { return (ulong?)GetValue(OwnerTelProperty); }
            set { SetValue(OwnerTelProperty, value); }
        }
        public static readonly DependencyProperty OwnerTelProperty =
            DependencyProperty.Register(nameof(OwnerTel), typeof(ulong?),
                typeof(EditApartmentWindow), new PropertyMetadata(default(ulong?)));

        public double? Price
        {
            get { return (double?)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }
        public static readonly DependencyProperty PriceProperty =
            DependencyProperty.Register(nameof(Price), typeof(double?),
                typeof(EditApartmentWindow), new PropertyMetadata(default(double?)));

        public ObservableCollection<Photo> Photos
        {
            get { return (ObservableCollection<Photo>)GetValue(PhotosProperty); }
            set { SetValue(PhotosProperty, value); }
        }

        public static readonly DependencyProperty PhotosProperty =
            DependencyProperty.Register(nameof(Photos), typeof(ObservableCollection<Photo>),
                typeof(EditApartmentWindow), new PropertyMetadata(default(ObservableCollection<Photo>)));

        #endregion

        #region Supporting methods
        private void _installSettings()
        {

        }

        private void ReadData()
        {
            string houseInfoText = $"Город: {SelectedHouse.City}, " +
                $"улица: {SelectedHouse.Street}, дом: {SelectedHouse.Number} " +
                $"корпус: {(SelectedHouse.Block != null ? SelectedHouse.Block : "Нет")}";
            Photos = new ObservableCollection<Photo>();

            HouseInfoText = houseInfoText;
            Number = SelectedApartment.Number;
            Floor = SelectedApartment.Floor;
            CountRooms = SelectedApartment.CountRooms;
            Area = SelectedApartment.Area;
            OwnerFIO = SelectedApartment.Owner;
            OwnerTel = SelectedApartment.OwnerTel;
            Price = SelectedApartment.Price;
            _readPhotos();
        }

        private void _readPhotos()
        {
            if (SelectedApartment.Photos != null && SelectedApartment.Photos.Count > 0)
            {
                foreach (Photo photo in SelectedApartment.Photos)
                {
                    Photos.Add(photo);
                }
            }
        }
        #endregion
        #region Validation data

        public bool HasError()
        {
            foreach (var item in Grid_InputData.Children)
            {
                if (item is TextBox tb)
                {
                    var errors = Validation.GetErrors(tb);
                    if (errors.Any()) return true;
                }
            }
            return false;
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Number):
                        if (Number <= 0)
                            error = "Номер квартиры должен быть больше 0.";
                        break;

                    case nameof(Floor):
                        if (Floor < 0)
                            error = "Этаж должен быть не отрицательным числом.";
                        break;

                    case nameof(CountRooms):
                        if (CountRooms <= 0)
                            error = "Количество комнат должно быть больше 0.";
                        break;

                    case nameof(Area):
                        if (Area <= 0)
                            error = "Площадь должна быть больше 0.";
                        break;

                    case nameof(OwnerFIO):
                        break;

                    case nameof(OwnerTel):
                        if (OwnerTel != null && OwnerTel < 1000000 || OwnerTel > 999999999999)
                            error = "Телефонный номер должен быть в формате 7 или 10 цифр (например, 1234567890).";
                        break;

                    case nameof(Price):
                        if (Price < 0)
                            error = "Цена не может быть отрицательной.";
                        break;
                }
                return error;
            }
        }
        #endregion
    }
}
