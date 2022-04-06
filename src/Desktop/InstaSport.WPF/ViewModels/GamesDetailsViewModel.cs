using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.State;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class GameDetailsViewModel : BindableBase, INavigationAware
    {
        private readonly IAuthenticator authenticator;
        private readonly IGamesService gamesService;
        private Game game;

        public Game Game
        {
            get { return this.game; }
            set
            {
                this.SetProperty(ref this.game, value);
                this.RaisePropertyChanged(nameof(PlayerHasJoinedGame));
            }
        }

        public bool PlayerHasJoinedGame
        {
            get { return this.Game != null && this.Game.Players.Any(p => p.Id == this.authenticator.CurrentUser.Id); }
        }

        public ICommand JoinGameCommand { get; }
        public ICommand LeaveGameCommand { get; }

        public GameDetailsViewModel(IAuthenticator authenticator, IGamesService gamesService)
        {
            this.authenticator = authenticator;
            this.gamesService = gamesService;
            this.JoinGameCommand = new DelegateCommand(JoinGame);
            this.LeaveGameCommand = new DelegateCommand(LeaveGame);
        }

        private void LeaveGame(object obj)
        {
            this.gamesService.RemovePlayer((int)obj, this.authenticator.CurrentUser);
            this.RaisePropertyChanged(nameof(PlayerHasJoinedGame));
        }

        private void JoinGame(object obj)
        {
            this.gamesService.AddPlayer((int)obj, this.authenticator.CurrentUser);
            this.RaisePropertyChanged(nameof(PlayerHasJoinedGame));
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
