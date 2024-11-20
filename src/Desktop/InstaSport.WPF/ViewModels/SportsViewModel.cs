using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.Helpers;
using InstaSport.WPF.Models;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class SportsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private ISportsService sportsService;
        private ObservableCollection<Location> locations;
        private ObservableCollection<SportDto> sports;

        public ObservableCollection<Location> Locations
        {
            get { return this.locations; }
        }

        public ObservableCollection<SportDto> Sports
        {
            get { return this.sports; }
        }

        public DelegateCommand FilterGamesBySportCommand { get; }

        public SportsViewModel(ISportsService sportsService, IRegionManager regionManager)
        {
            this.sportsService = sportsService;
            this.regionManager = regionManager;
            this.sports = new ObservableCollection<SportDto>(this.sportsService.GetAll().ToDto());
            this.FilterGamesBySportCommand = new DelegateCommand(OnFilterGamesBySport);
        }

        private void OnFilterGamesBySport(object obj)
        {
            var sport = obj as SportDto;
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("Sport", sport);
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
