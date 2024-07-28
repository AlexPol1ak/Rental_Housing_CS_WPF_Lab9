using CS_WPF_Lab9_Rental_Housing.Business.Managers;
using CS_WPF_Lab9_Rental_Housing.Business.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CS_WPF_Lab9_Rental_Housing.Domain.Entities;

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
        private House _selectedApartment;

        public House SelectedHouses
        {
            get { return _selectedHouses; }
            set { Set(ref _selectedHouses, value); }
        }

        public House SelectedApartment
        {
            get { return _selectedApartment; }
            set { Set(ref _selectedApartment, value); }
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
    }
}
