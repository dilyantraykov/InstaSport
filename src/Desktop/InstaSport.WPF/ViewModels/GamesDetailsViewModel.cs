using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.Services.Data.Constants;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class GameDetailsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IAuthenticator authenticator;
        private readonly IGamesService gamesService;
        private Game game;

        public Game Game
        {
            get { return this.game; }
            set
            {
                this.SetProperty(ref this.game, value);
                this.RaisePropertyChanged(nameof(IsGameActive));
                this.RaisePropertyChanged(nameof(PlayerHasJoinedGame));
                this.RaisePropertyChanged(nameof(Players));
            }
        }

        public ObservableCollection<User> Players { get { return this.Game == null ? null : new ObservableCollection<User>(this.Game.Players); } }

        public bool IsGameActive
        {
            get { return this.Game != null && this.Game.Status == GameStatus.WaitingForPlayers; }
        }

        public bool PlayerHasJoinedGame
        {
            get { return this.Game != null && this.Game.Players.Any(p => p.Id == this.authenticator.CurrentUser.Id); }
        }

        public ICommand GoToUserProfileCommand { get; }
        public ICommand JoinGameCommand { get; }
        public ICommand LeaveGameCommand { get; }

        public GameDetailsViewModel(IAuthenticator authenticator, IGamesService gamesService, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.authenticator = authenticator;
            this.gamesService = gamesService;
            this.JoinGameCommand = new DelegateCommand(JoinGame);
            this.LeaveGameCommand = new DelegateCommand(LeaveGame);
            this.GoToUserProfileCommand = new DelegateCommand(GoToProfile);
        }

        private void GoToProfile(object obj)
        {
            var user = obj as User; 
            var parameters = new NavigationParameters();
            parameters.Add("Username", user.UserName);
            this.regionManager.RequestNavigate(StringConstants.MainRegionName, nameof(UserDetailsView), parameters);
        }

        private void LeaveGame(object obj)
        {
            this.gamesService.RemovePlayer((int)obj, this.authenticator.CurrentUser);
            this.RaisePropertyChanged(nameof(PlayerHasJoinedGame));
            this.RaisePropertyChanged(nameof(Players));
        }

        private void JoinGame(object obj)
        {
            this.gamesService.AddPlayer((int)obj, this.authenticator.CurrentUser);
            this.RaisePropertyChanged(nameof(PlayerHasJoinedGame));
            this.RaisePropertyChanged(nameof(Players));
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var gameId = (int)navigationContext.Parameters["GameId"];
            this.Game = this.gamesService.GetById(gameId);
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
