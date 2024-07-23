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
        ManagersFactory factory;
        HouseManager houseManager;
        ApartmentManager apartmentManager;
        PhotoManager photoManager;

        public ObservableCollection<House> Houses { get; set; }
        public ObservableCollection<Apartment> Apartment { get; set; }
        public ObservableCollection<Photo> Photos { get; set; }

        public string Title { get; set; }

        private House _selectedHouses;

        public MainWindowViewModel()
        {
            factory = new ManagersFactory("DefaultConnection");
            houseManager = factory.GetHouseManager();
            apartmentManager = factory.GetApartmentManager();
            photoManager = factory.GetPhotoManager();
            Houses= new ObservableCollection<House>();
            Apartment= new ObservableCollection<Apartment>();
            Photos= new ObservableCollection<Photo>();
            
        }

        public House SelectedHouses
        {
            get { return _selectedHouses; }
            set 
            {
                Set(ref _selectedHouses, value);
            }
        }
    }
}
