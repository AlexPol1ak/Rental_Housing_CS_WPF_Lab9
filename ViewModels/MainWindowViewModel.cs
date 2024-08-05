using CS_WPF_Lab9_Rental_Housing.Business.Managers;
using CS_WPF_Lab9_Rental_Housing.Business.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CS_WPF_Lab9_Rental_Housing.Domain.Entities;
using System.Windows.Input;
using CS_WPF_Lab9_Rental_Housing.Commands;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Controls;
using CS_WPF_Lab9_Rental_Housing.Views;

namespace CS_WPF_Lab9_Rental_Housing.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ManagersFactory factory;
        private HouseManager houseManager;
        private ApartmentManager apartmentManager;
        private PhotoManager photoManager;

        public ObservableCollection<House> Houses { get; set; }
        public ObservableCollection<Apartment> Apartments { get; set; }
        public ObservableCollection<Photo> Photos { get; set; }

        private House? _selectedHouses;
        private Apartment? _selectedApartment = null;
        private string _detailInfo = string.Empty;
        private Photo? _currentPhoto = null;

        public House? SelectedHouses
        {
            get { return _selectedHouses; }
            set { Set(ref _selectedHouses, value); }
        }

        public Apartment? SelectedApartment
        {
            get { return _selectedApartment; }
            set { Set(ref _selectedApartment, value); }
        }

        public string DetailInfo
        {
            get => _detailInfo;
            set { Set(ref _detailInfo, value); }
        }
        public Photo? CurrentPhoto
        {
            get => _currentPhoto;
            set { Set(ref _currentPhoto, value); }
        }

        public MainWindowViewModel()
        {
            factory = new ManagersFactory("DefaultConnection");
            houseManager = factory.GetHouseManager();
            apartmentManager = factory.GetApartmentManager();
            photoManager = factory.GetPhotoManager();

            DbInitData.SetupData(factory);

            Houses = new ObservableCollection<House>(houseManager.GetAllHouses(true));
            Apartments = new ObservableCollection<Apartment>();
            Photos = new ObservableCollection<Photo>();

        }

        #region Commands

        #region Commands select house
        private ICommand _selectHouseCommand;
        public ICommand SelectHouseCommand => _selectHouseCommand ??=
            new RelayCommand(selectHouseExecuted);

        private void selectHouseExecuted(object obj)
        {
            House? h = SelectedHouses;

            Apartments.Clear();
            Photos.Clear();
            CurrentPhoto = null;
            VisibilityNavigationBtn = Visibility.Collapsed;

            if(h != null)
            {
                DetailInfo = h.ToString(full: true);
                if (h.Apartments != null && h.Apartments.Count > 0)
                {
                    foreach (Apartment ap in h.Apartments) Apartments.Add(ap);

                }
            }           
            else { DetailInfo = string.Empty; }
            SelectedApartment = null;
        }
        #endregion 

        #region Commands select Apartment
        private ICommand _selectApartmentCommand;
        public ICommand SelectApartmentCommand => _selectApartmentCommand ??=
            new RelayCommand(selectApartmentExecuted);

        private void selectApartmentExecuted(object obj)
        {
            if (SelectedApartment == null)
            {
                DetailInfo = string.Empty;
            }
            else
            {
                apartmentManager.LoadPhotos(SelectedApartment);
                DetailInfo = SelectedApartment.ToString(full: true);
                Photos.Clear();
                CurrentPhoto = null;
                foreach (Photo photo in SelectedApartment.Photos) Photos.Add(photo);
                if (Photos.Count > 0) CurrentPhoto = Photos[0];

            }
        }
        #endregion

        #region Commands photo navigation
        private ICommand _previousPhotoCommand;
        private ICommand _nextPhotoCommand;
        private ICommand _gridDetailInfoMouseEnterCommand;
        private ICommand _gridDetailInfoMouseLeaveCommand;
        private Visibility _visibilityNavigationBtn = Visibility.Collapsed;

        // The visibility property of photo swipe buttons.
        public Visibility VisibilityNavigationBtn
        {
            get => _visibilityNavigationBtn;
            set { Set(ref _visibilityNavigationBtn, value); }
        }

        // The command to press the “previous photo” button. Displays the previous photo.
        public ICommand PreviousPhotoCommand => _previousPhotoCommand ??= new
            RelayCommand(
                // Sets the previous photo in the collection as the current photo
                (id) =>
                {
                    int index = Photos.IndexOf(CurrentPhoto!);
                    CurrentPhoto = Photos[--index];
                },
                // Checks whether the command can be executed.
                // Are there any previous photos in the collection?
                (id) =>
                {
                    if (CurrentPhoto != null && Photos.Count > 0 && Photos.IndexOf(CurrentPhoto) - 1 >= 0)
                        return true;
                    else return false;
                }
            );

        // Press the Next Photo button command. Displays the next photo.
        public ICommand NextPhotoCommand => _nextPhotoCommand ??= new
            RelayCommand(
                (id) =>
                {
                    //Sets the next photo following the current one in the collection as the current photo.
                    int index = Photos.IndexOf(CurrentPhoto!);
                    CurrentPhoto = Photos[++index];
                },
                (id) =>
                {
                    // Checks if the command can be executed.
                    // Is the current photo not the last photo in the collection?
                    if (CurrentPhoto != null && Photos.Count > 0 && Photos.IndexOf(CurrentPhoto) + 1 <= Photos.Count - 1)
                        return true;
                    else return false;
                }

             );

        // Command to enter the cursor into the detailed information grid area.
        // Makes the photo scroll buttons visible.
        public ICommand GridDetailInfoMouseEnterCommand => _gridDetailInfoMouseEnterCommand ??=
            new RelayCommand(
                (id) => { VisibilityNavigationBtn = Visibility.Visible; },
                (id) =>
                    {
                        if (Photos.Count > 0 && CurrentPhoto != null) return true;
                        else return false;
                    }
                );

        // Command to exit the cursor from the area and detailed information grid.
        // Makes photo scroll buttons invisible.
        public ICommand GridDetailInfoMouseLeaveCommand => _gridDetailInfoMouseLeaveCommand ??=
            new RelayCommand(
                (id) => { VisibilityNavigationBtn = Visibility.Collapsed; }
                );
        #endregion

        #region Commands control buttons 

        private ICommand _deleteCommand;
        private ICommand _editCommand;
        private ICommand _addCommand;
        private Func<object, bool> _hasObject => 
            (id)=> SelectedHouses != null || SelectedApartment != null;


        public ICommand DeleteCommand => _deleteCommand ??=
            new RelayCommand(deleteButtonExecuted, _hasObject);

        public ICommand EditCommand => _editCommand ??=
            new RelayCommand(editButtonExecuted, _hasObject);

        public ICommand AddComand => _addCommand ??=
            new RelayCommand(addButtonExecuted);

        /// <summary>
        /// Executor for the AddCommand command 
        /// </summary>
        private void addButtonExecuted(object obj)
        {
            if(SelectedHouses == null)
            {
                addHouse();
            }
            else
            {
                ChoiceAddWindow choiceAddWindow = new ChoiceAddWindow()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                };
                choiceAddWindow.ShowDialog();
                var choice = choiceAddWindow.Choice;

                if (choice == null) return;
                if (choice == "House") addHouse();
                if (choice == "Apartment") addApartment();
            }
                     
        }

        /// <summary>
        /// Executor for the EditCommand command 
        /// </summary>
        private void editButtonExecuted(object obj)
        {
            if (SelectedApartment == null && SelectedHouses != null)
            {            
                EditHouse();
                return;
            }
            if (SelectedApartment != null)
            {
                EditApartment();
                return;
            }
        }

        /// <summary>
        /// Executor for the DeleteCommand command 
        /// </summary>
        private void deleteButtonExecuted(object obj)
        {
            string title = "Удаление";
            // If a house is selected - delete the house.
            if (SelectedApartment == null && SelectedHouses != null)
            {
                var result = MessageBox.Show($"Удалить дом ?\n{SelectedHouses.ToString()}",
                    title, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool result2 =  DeleteHouse(SelectedHouses);
                    if(result2)
                    {
                        MessageBox.Show("Удалено!", title,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                                      
                }
            }
            // If an apartment is selected, delete the apartment.
            if (SelectedApartment != null)
            {
                var result = MessageBox.Show(
                    $"Удалить кваритру ?\n{SelectedApartment.ToString()}",title, 
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool result2 = DeleteApartment(SelectedApartment);
                    if (result2)
                    {
                        MessageBox.Show("Удалено!", title,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }                
        }

        #endregion

        #region Encapsulated CRUD actions
        // Auxiliary methods used in commands.

        /// <summary>
        /// Removes house from the database, from the linked collection, from the “selected item”.
        /// </summary>
        /// <param name="house">House object</param>
        /// <returns>True - if the deletion was successful, otherwise False.</returns>
        public bool DeleteHouse(House house)
        {
            bool result = false;

            if (houseManager.ContainsHouse(house))
            {
                result = houseManager.DeleteHouse(house.HouseId);
                houseManager.SaveChanges();
            }
            if(Houses.Contains(house))
            {
                result = Houses.Remove(house);
            }
            if (SelectedHouses == house)
            {
                SelectedHouses = null;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Deletes an apartment from the database, from the linked collection, from the “selected item”. 
        /// </summary>
        /// <param name="apartment">Apartment object</param>
        /// <returns>True - if the deletion was successful, otherwise False.</returns>
        public bool DeleteApartment(Apartment apartment)
        {
            bool result = false;

            if (apartmentManager.ContainsApartment(apartment))
            {
                result = apartmentManager.DeleteApartment(apartment.ApartmentId);
                apartmentManager.SaveChanges();
            }
            if (Apartments.Contains(apartment))
            {
                result = Apartments.Remove(apartment);               
            }
            if(SelectedApartment == apartment)
            {
                SelectedApartment = null;
                result = true;
            }
            return result ;
        }

        /// <summary>
        /// Opens the house in a new window for editing.
        /// </summary>
        /// <returns></returns>
        public bool EditHouse()
        {
            if (SelectedHouses == null) return false;

            EditHouseWindow editHouseWindow = new EditHouseWindow(SelectedHouses);
            bool? result = editHouseWindow.ShowDialog();

            if(result == true)
            {
                houseManager.UpdateHouse(editHouseWindow.SelectedHouse);
                houseManager.SaveChanges();
                Reload();
            }
            return result == true;  
        }

        /// <summary>
        /// Opens the apartment in a new window for editing.
        /// </summary>
        /// <returns></returns>
        public bool EditApartment()
        {
            if (SelectedApartment == null) return false;

            EditApartmentWindow editApartWindow = new EditApartmentWindow(SelectedApartment);
            bool? result = editApartWindow.ShowDialog();

            if (result == true) 
            { 
                apartmentManager.UpdateApartment(editApartWindow.SelectedApartment);
                apartmentManager.SaveChanges();
                Reload();         
            }
            return result ==true;
        }

        public void addHouse()
        {
            EditHouseWindow editHouseWindow = new EditHouseWindow();
            bool? result = editHouseWindow.ShowDialog();

            if (result == true)
            {
                houseManager.AddHouse(editHouseWindow.SelectedHouse);
                houseManager.SaveChanges();
                Reload();
            }
        }

        public void addApartment()
        {
            EditApartmentWindow editApartWindow = new EditApartmentWindow(SelectedHouses!);
            bool? result = editApartWindow.ShowDialog();

            if (result == true)
            {
                apartmentManager.AddApartment(SelectedHouses!.HouseId,editApartWindow.SelectedApartment);
                apartmentManager.SaveChanges();
                Reload();
            }

        }
        #endregion

        #endregion
        /// <summary>
        /// Reloads information from the database and overwrites the ListView.
        /// Sets the user selection to the position before reloading, if possible.
        /// </summary>
        public void Reload()
        {
            int? SelectedHouseId = null;
            int? SelectedApartmentId = null;
            
            if(SelectedHouses != null) SelectedHouseId = SelectedHouses.HouseId;
            if(SelectedApartment != null) SelectedApartmentId = SelectedApartment.ApartmentId;

            Houses.Clear();
            Apartments.Clear();
            Photos.Clear();
            SelectedHouses = null;
            SelectedApartment = null;

            foreach(House house in houseManager.GetAllHouses(true)) Houses.Add(house);

            if (SelectedHouseId != null)
            {
                foreach(House house in Houses)
                {
                    if(house.HouseId == SelectedHouseId)
                    {
                        SelectedHouses = house;
                        selectHouseExecuted(house);
                    }
                }
                
                if(SelectedApartmentId != null)
                {
                    foreach(Apartment ap in Apartments)
                    {
                        if(SelectedApartmentId == ap.ApartmentId)
                        {
                            SelectedApartment = ap;
                            selectApartmentExecuted(ap);
                        }
                    }
                }
            }
        }
    }
}
