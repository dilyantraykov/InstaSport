using InstaSport.Data.Models;
using InstaSport.Services.Data;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace InstaSport.WPF.ViewModels
{
    public class LocationsViewModel : BindableBase
    {
        private ILocationsService locationsService;
        private ObservableCollection<Location> locations;

        public ObservableCollection<Location> Locations
        {
            get { return this.locations; }
        }

        public LocationsViewModel(ILocationsService locationsService)
        {
            this.locationsService = locationsService;
            this.locations = new ObservableCollection<Location>(this.locationsService.GetAll());
        }
    }
}
