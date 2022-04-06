using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.Services.Data.Constants;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class GamesViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IAuthenticator authenticator;
        private readonly IGamesService gamesService;
        private Game selectedGame;

        private ObservableCollection<Game> games;

        public ObservableCollection<Game> Games
        {
            get { return this.games; }
            set { this.games = value; this.RaisePropertyChanged(); }
        }

        public Game SelectedGame
        {
            get { return this.selectedGame; }
            set { this.SetProperty(ref this.selectedGame, value); }
        }

        public ICommand OpenGameCommand { get; }

        public GamesViewModel(IAuthenticator authenticator, IGamesService gamesService, IRegionManager regionManager)
        {
            this.authenticator = authenticator;
            this.gamesService = gamesService;
            this.regionManager = regionManager;
            this.OpenGameCommand = new DelegateCommand(OpenGame);
        }

        private void OpenGame(object obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("GameId", obj);
            this.regionManager.RequestNavigate(StringConstants.MainRegionName, nameof(GameDetailsView), parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.Games = new ObservableCollection<Game>(this.gamesService.GetAll());
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
