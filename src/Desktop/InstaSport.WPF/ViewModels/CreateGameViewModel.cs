using InstaSport.Data.Models;
using InstaSport.Services.Data;
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
        private ObservableCollection<Sport> sports;
        private ObservableCollection<Location> locations;

        public ObservableCollection<Sport> Sports
        {
            get { return this.sports; }
        }

        public ObservableCollection<Location> Locations
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
            this.locations = new ObservableCollection<Location>(this.locationsService.GetAll());
            this.sports = new ObservableCollection<Sport>(this.sportsService.GetAll());
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
