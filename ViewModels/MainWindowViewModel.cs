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
            new RelayCommand(SelectHouseExecuted);

        private void SelectHouseExecuted(object obj)
        {
            House h = SelectedHouses;

            Apartments.Clear();  
            Photos.Clear();
            CurrentPhoto = null;
            if(h != null && h.Apartments != null && h.Apartments.Count > 0)
            {
                foreach(Apartment ap in  h.Apartments) Apartments.Add(ap);
                DetailInfo = h.ToString(full: true);
            }
            else { DetailInfo = string.Empty; }
            SelectedApartment = null;           
        }
        #endregion

        #region Select Apartment
        private ICommand _selectApartmentCommand;
        public ICommand SelectApartmentCommand => _selectApartmentCommand ??=
            new RelayCommand(SelectApartmentExecuted);

        private void SelectApartmentExecuted(object obj)
        {
            if(SelectedApartment == null)
            {
                DetailInfo = string.Empty;
            }
            else
            {
                apartmentManager.LoadPhotos(SelectedApartment);
                DetailInfo = SelectedApartment.ToString(full:true);
                Photos.Clear();
                CurrentPhoto = null;
                foreach(Photo photo in SelectedApartment.Photos) Photos.Add(photo);
                if (Photos.Count > 0) CurrentPhoto = Photos[0];
            }

        }
        #endregion

        #region PhotoNavigation
        private ICommand _previousPhotoCommand;
        private ICommand _nextPhotoCommand;

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

        #endregion


        #endregion
    }
}
