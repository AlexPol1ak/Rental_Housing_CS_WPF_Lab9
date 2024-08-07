using CS_WPF_Lab9_Rental_Housing.Commands;
using CS_WPF_Lab9_Rental_Housing.Domain.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS_WPF_Lab9_Rental_Housing.Views
{
    /// <summary>
    /// The window for creating or editing an apartment.
    /// </summary>
    public partial class EditApartmentWindow : Window, IDataErrorInfo
    {
        private string root = Directory.GetCurrentDirectory();
        private string photoDir => System.IO.Path.Combine(root, "Images");

        //For temporary storage of added photos.
        private List<Photo> tempAddedNewPhoto = new();
        private List<FileInfo> tempAddedNewPhotoFileInfo = new();
        // To temporarily store photos sent for deletion
        private List<Photo> tempDeletedPhoto = new();
        private List<FileInfo> tempDeletedPhotoFileINfo = new();

        // Objects accepted for change
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

        /// <summary>
        /// A constructor to create a new apartment.
        /// </summary>
        /// <param name="house">House object</param>
        public EditApartmentWindow(House house)
        {
            this.Closing += EditApartmentWindowClosing;
            TitleTextAp = "Добавить квартиру";
            InitializeComponent();        
            SelectedHouse = house;
            SelectedApartment = new Apartment();
            SelectedApartment.House = house;
            SelectedApartment.Photos = new List<Photo>();
            installSettings();
        }

        /// <summary>
        /// Builder to create to edit an existing apartment.
        /// </summary>
        /// <param name="apartment">Apartment object</param>
        public EditApartmentWindow(Apartment apartment)
        {
            this.Closing += EditApartmentWindowClosing;
            TitleTextAp = "Редактировать квартиру";
            InitializeComponent();
            SelectedHouse = apartment.House;
            SelectedApartment = apartment;
            installSettings();           
        }

        /// <summary>
        /// Event handler for the event when the window is closed.
        /// </summary>
        private void EditApartmentWindowClosing(object? sender, CancelEventArgs e)
        {
            // Delete added apartments if the save button is not pressed.
            if (this.DialogResult != true)
            {
                if (tempAddedNewPhotoFileInfo.Count > 0)
                {
                    foreach (FileInfo file in tempAddedNewPhotoFileInfo)
                    {
                        file.Delete();
                    }
                }
            }
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
                typeof(EditApartmentWindow), new PropertyMetadata(new ObservableCollection<Photo>()));

        #endregion

        #region Commands

        private void setCommands()
        {
            // Commands to save all entered information.
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
            //Exit command
            CommandBinding commandBindingExit = new CommandBinding(
                WindowCommands.Exit,
                (s, e) => { this.Close(); }
                );

            this.CommandBindings.Add(commandBindingSave);
            this.CommandBindings.Add(commandBindingExit);

        }

        private ICommand _addPhotoCommnad;
        public ICommand AddPhotoCommnad => _addPhotoCommnad ??= new RelayCommand
            (
                addPhotoExecuted,
                (obj) => { return Photos.Count < 10; }
            );

      
        private ICommand _deletePhohoCommnad;
        public ICommand DeletePhotoCommnad => _deletePhohoCommnad ??= new RelayCommand
            (
                deletePhotoExecuted,
                (obj) => { return obj != null && Photos.Count > 0; }
                
            );

        /// <summary>
        /// Performer for the team to add a photo. 
        /// Opens the file selection window and adds the selected photos.
        /// </summary>
        private void addPhotoExecuted(object obj)
        {
            // Open the file selection window.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;";
            openFileDialog.Multiselect = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.ValidateNames = true;

            if (openFileDialog.ShowDialog() == true)
            // If new files are selected and their total number (old + new) does not exceed 10.
            {
                if (openFileDialog.FileNames.Length + Photos.Count > 10)
                {
                    MessageBox.Show(
                        "Общее количество фотографий не должно привышать 10", 
                        "Загрузка фотографий", 
                        MessageBoxButton.OK, MessageBoxImage.Information
                        );
                }
                else
                {
                    if(openFileDialog.FileNames.Length > 0)
                    {
                        foreach(string fileName in openFileDialog.FileNames)
                        {   
                            // Send files to be saved.
                            addNewPhoto(fileName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executor of the delete photos command. Sends files for deletion
        /// </summary>
        /// <param name="obj">Parameter from ListBox</param>
        private void deletePhotoExecuted(object obj)
        {
            Photo photo = (Photo) obj;
            deletePhoto(photo);          
        }
        #endregion

        #region Supporting methods
        /// <summary>
        /// Sets the commands for the buttons . Reads data from the object.
        /// </summary>
        public void installSettings()
        {
            setCommands();
            ReadData();
        }

        /// <summary>
        /// Transfers the entered information from the input fields to the object. 
        /// Saves the changes. Deletes photos sent for deletion.
        /// </summary>
        private void changeHouse()
        {
            SelectedApartment.Number = Number;
            SelectedApartment.Floor = Floor;
            SelectedApartment.CountRooms = CountRooms;
            SelectedApartment.Area = Area;
            SelectedApartment.Owner = OwnerFIO;
            SelectedApartment.OwnerTel = OwnerTel;
            SelectedApartment.Price = Price;

            //Photo transfers.
            SelectedApartment.Photos.Clear();
            if(Photos.Count > 0)
            {
                foreach(Photo photo in Photos)
                {
                    SelectedApartment.Photos.Add(photo);
                }
            }
            // Deleting photos moved to a deleted collection
            if (tempDeletedPhotoFileINfo.Count > 0)
            {
                foreach(FileInfo file in tempDeletedPhotoFileINfo) { file.Delete(); }
            }
        }

        /// <summary>
        /// Reads data from the object into the dependency properties.
        /// </summary>
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

            if (SelectedApartment.Photos != null && SelectedApartment.Photos.Count > 0)
            {
                foreach (Photo photo in SelectedApartment.Photos)
                {
                    Photos.Add(photo);
                }
            }
        }
        /// <summary>
        /// Adding a new photo
        /// </summary>
        /// <param name="path">Full path</param>
        /// <returns></returns>
        private bool addNewPhoto(string path)
        {
            FileInfo photoFileInfo = new FileInfo(path);
            if (!photoFileInfo.Exists) return false;

            Photo photo = new Photo();
            photo.PhotoName = photoFileInfo.Name;
            photo.Apartment = SelectedApartment;

            string distinationPath = System.IO.Path.Combine(photoDir, photo.PhotoName);
            var photoCopyFile = photoFileInfo.CopyTo(distinationPath, true);

            // Add a photo to the collection of temporary new photos.
            tempAddedNewPhotoFileInfo.Add(photoCopyFile);
            tempAddedNewPhoto.Add(photo);
            Photos.Add(photo);

            return true;
        }

        /// <summary>
        /// Deleting a photo.
        /// </summary>
        /// <param name="photo">Phoyo object</param>
        /// <returns></returns>
        private bool deletePhoto(Photo photo)
        {
            string distinationPath = System.IO.Path.Combine(photoDir, photo.PhotoName);
            FileInfo photoFileInfo = new FileInfo(distinationPath);

            //Delete a photo from the collection of displayed photos
            if (Photos.Contains(photo)) Photos.Remove(photo);

            // Remove a photo from the collection of newly added photos,
            // if they are there (If photos are added in the same window)
            if (tempAddedNewPhoto.Contains(photo)) tempAddedNewPhoto.Remove(photo);
            if(tempAddedNewPhotoFileInfo.Contains(photoFileInfo)) tempAddedNewPhotoFileInfo.Remove(photoFileInfo);
            // Add photos to the temporary collection of photos to be deleted.
            if (!tempDeletedPhoto.Contains(photo)) tempDeletedPhoto.Add(photo);
            if(!tempAddedNewPhotoFileInfo.Contains(photoFileInfo)) tempDeletedPhotoFileINfo.Add(photoFileInfo);

            return true;
        }
        #endregion

        #region Validation data
        /// <summary>
        /// Checks the correctness of the data entered by the user.
        /// </summary>
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
                        if (Floor <=0)
                            error = "Этаж должен быть не отрицательным числом и не равным 0.";
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
                        if (OwnerTel != null && OwnerTel < 1000000 || OwnerTel > 9999999999999)
                            error = "Телефонный номер должен быть в формате 7 или 13 цифр (например, 1234567890).";
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
