using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class SportsViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private ISportsService sportsService;
        private ObservableCollection<Location> locations;
        private ObservableCollection<Sport> sports;

        public ObservableCollection<Location> Locations
        {
            get { return this.locations; }
        }

        public ObservableCollection<Sport> Sports
        {
            get { return this.sports; }
        }

        public DelegateCommand FilterGamesBySportCommand { get; }

        public SportsViewModel(ISportsService sportsService, IRegionManager regionManager)
        {
            this.sportsService = sportsService;
            this.regionManager = regionManager;
            this.sports = new ObservableCollection<Sport>(this.sportsService.GetAll());
            this.FilterGamesBySportCommand = new DelegateCommand(OnFilterGamesBySport);
        }

        private void OnFilterGamesBySport(object obj)
        {
            var sport = obj as Sport;
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("Sport", sport);
            this.regionManager.RequestNavigate("MainRegion", nameof(GamesView), navigationParameters);
        }
    }
}
