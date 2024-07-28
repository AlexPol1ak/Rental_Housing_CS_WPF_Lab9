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
        private string _detailInfo;

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

        public string DetailInfo
        {
            get => _detailInfo;
            set { Set(ref _detailInfo, value); }
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
            new RelayCommnad(SelectHouseExecuted);

        private void SelectHouseExecuted(object obj)
        {
            House h = SelectedHouses;

            Apartments.Clear();     
            if(h != null && h.Apartments != null && h.Apartments.Count > 0)
            {
                foreach(Apartment ap in  h.Apartments) Apartments.Add(ap);
                DetailInfo = h.ToString(full: true);
            }
            else { DetailInfo = string.Empty; }
        }
        #endregion


        #endregion
    }
}
