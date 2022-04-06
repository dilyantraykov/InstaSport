using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.Models;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class LocationsViewModel : BindableBase
    {
        private ILocationsService locationsService;
        private IEnumerable<MapItem> mapItems;
        private ObservableCollection<Location> locations;
        private Telerik.Windows.Controls.Map.Location centerLocation;
        private MapItem selectedLocation;

        public Telerik.Windows.Controls.Map.Location CenterLocation
        {
            get { return centerLocation; }
            set
            {
                if (centerLocation != value)
                {
                    centerLocation = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public MapItem SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                if (value != null && !value.Equals(selectedLocation))
                {
                    selectedLocation = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<MapItem> MapItems
        {
            get { return this.mapItems; }
        }

        public ObservableCollection<Location> Locations
        {
            get { return this.locations; }
        }

        public ICommand SelectCenterCommand { get; set; }
        public ICommand SelectPinCommand { get; set; }

        public LocationsViewModel(ILocationsService locationsService)
        {
            this.locationsService = locationsService;
            this.locations = new ObservableCollection<Location>(this.locationsService.GetAll());
            this.mapItems = this.locations.Select(l => new MapItem(l));
            this.SelectCenterCommand = new DelegateCommand(SelectCenter);
            this.SelectPinCommand = new DelegateCommand(SelectPin);
            this.CenterLocation = this.mapItems.First().Location;
            this.SelectedLocation = this.mapItems.First();
        }

        private void SelectPin(object obj)
        {
            var location = (Telerik.Windows.Controls.Map.Location)obj;
            this.SelectedLocation = this.MapItems.First(i => i.Location == location);
            this.CenterLocation = location;
        }

        private void SelectCenter(object obj)
        {
            var args = obj as SelectionChangedEventArgs;
            var item = ((MapItem)args.AddedItems[0]);
            this.CenterLocation = item.Location;
        }
    }
}
