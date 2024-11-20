using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.Helpers;
using InstaSport.WPF.Models;
using InstaSport.WPF.Views;
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
    public class LocationsViewModel : BindableBase, INavigationAware
    {
        private IRegionManager regionManager;
        private ILocationsService locationsService;
        private IEnumerable<MapItem> mapItems;
        private ObservableCollection<LocationDto> locations;
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

        public ObservableCollection<LocationDto> Locations
        {
            get { return this.locations; }
        }

        public ICommand SelectCenterCommand { get; set; }
        public ICommand SelectPinCommand { get; set; }
        public ICommand FilterGamesByLocationCommand { get; }

        public LocationsViewModel(ILocationsService locationsService, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.locationsService = locationsService;
            this.locations = new ObservableCollection<LocationDto>(this.locationsService.GetAll().ToDto());
            this.mapItems = this.locations.Select(l => new MapItem(l));
            this.SelectCenterCommand = new DelegateCommand(SelectCenter);
            this.SelectPinCommand = new DelegateCommand(SelectPin);
            this.CenterLocation = this.mapItems.First().Location;
            this.SelectedLocation = this.mapItems.First();
            this.FilterGamesByLocationCommand = new DelegateCommand(OnFilterGamesByLocation);
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

        private void OnFilterGamesByLocation(object obj)
        {
            var mapItem = obj as MapItem;
            var location = this.Locations.FirstOrDefault(l => l.Name == mapItem.Name);
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("Location", location);
            this.regionManager.RequestNavigate("MainRegion", nameof(GamesView), navigationParameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
