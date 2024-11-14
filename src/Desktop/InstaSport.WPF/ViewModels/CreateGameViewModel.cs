using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.Helpers;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class CreateGameViewModel : BindableBase, INavigationAware
    {
        private IRegionManager regionManager;
        private readonly IAuthenticator authenticator;
        private readonly IGamesService gamesService;
        private readonly ILocationsService locationsService;
        private readonly ISportsService sportsService;
        private Game game;
        private ObservableCollection<SportDto> sports;
        private ObservableCollection<LocationDto> locations;

        public ObservableCollection<SportDto> Sports
        {
            get { return this.sports; }
        }

        public ObservableCollection<LocationDto> Locations
        {
            get { return this.locations; }
        }

        public Game Game
        {
            get { return this.game; }
            set
            {
                this.SetProperty(ref this.game, value);
            }
        }

        public ICommand CreateGameCommand { get; }

        public CreateGameViewModel(IAuthenticator authenticator, IGamesService gamesService, ILocationsService locationsService, ISportsService sportsService, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.authenticator = authenticator;
            this.gamesService = gamesService;
            this.locationsService = locationsService;
            this.sportsService = sportsService;
            this.locations = new ObservableCollection<LocationDto>(this.locationsService.GetAll().ToDto());
            this.sports = new ObservableCollection<SportDto>(this.sportsService.GetAll().ToDto());
            this.Game = new Game();
            this.Game.MinPlayers = 0;
            this.Game.MaxPlayers = 0;
            this.Game.StartingDateTime = DateTime.Now;
            this.CreateGameCommand = new DelegateCommand(OnCreateGame);
        }

        private void OnCreateGame(object obj)
        {
            this.gamesService.Create(this.Game);
            this.gamesService.AddPlayer(this.Game.Id, this.authenticator.CurrentUser);
            this.regionManager.RequestNavigate("MainRegion", nameof(GamesView));
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
