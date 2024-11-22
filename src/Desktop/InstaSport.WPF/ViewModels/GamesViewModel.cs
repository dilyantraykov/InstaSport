using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.Services.Data.Constants;
using InstaSport.WPF.Helpers;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class GamesViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IAuthenticator authenticator;
        private readonly IGamesService gamesService;
        private GameDto selectedGame;

        private ObservableCollection<GameDto> games;

        public ObservableCollection<GameDto> Games
        {
            get { return this.games; }
            set { this.games = value; this.RaisePropertyChanged(); }
        }

        public GameDto SelectedGame
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
            var games = this.gamesService
                .GetAll()
                .OrderByDescending(g => g.StartingDateTime)
                .ToDto();
            var sportFilter = navigationContext.Parameters["Sport"] as SportDto;
            if (sportFilter != null)
            {
                games = games.Where(x => x.SportId == sportFilter.Id);
            }

            var locationFilter = navigationContext.Parameters["Location"] as LocationDto;
            if (locationFilter != null)
            {
                games = games.Where(x => x.LocationId == locationFilter.Id);
            }

            this.Games = new ObservableCollection<GameDto>(games);
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
