﻿using CS_WPF_Lab9_Rental_Housing.Business.Managers;
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

        private House _selectedHouses;
        private Apartment? _selectedApartment = null;
        private string _detailInfo = string.Empty;
        private Photo? _currentPhoto = null;

        public House SelectedHouses
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

        #region Select house
        private ICommand _selectHouseCommand;
        public ICommand SelectHouseCommand => _selectHouseCommand ??=
            new RelayCommand(selectHouseExecuted);

        private void selectHouseExecuted(object obj)
        {
            House h = SelectedHouses;

            Apartments.Clear();
            Photos.Clear();
            CurrentPhoto = null;
            VisibilityNavigationBtn = Visibility.Collapsed;

            if (h != null && h.Apartments != null && h.Apartments.Count > 0)
            {
                foreach (Apartment ap in h.Apartments) Apartments.Add(ap);
                DetailInfo = h.ToString(full: true);
            }
            else { DetailInfo = string.Empty; }
            SelectedApartment = null;
        }
        #endregion

        #region Select Apartment
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

        #region PhotoNavigation
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

        #region Control Buttons 

        private ICommand _deleteCommand;
        private ICommand _editCommand;
        private ICommand _addCommand;
        private Func<object, bool> _hasObject => 
            (id)=> SelectedHouses != null || SelectedApartment != null;

        public ICommand DeleteCommand => _deleteCommand ??=
            new RelayCommand(deleteButtonExecuted, _hasObject);

        /// <summary>
        /// Executor for the Delete command 
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

        #endregion
    }
}
