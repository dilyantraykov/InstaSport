using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.State;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class GamesViewModel : BindableBase
    {
        private readonly IAuthenticator authenticator;
        private readonly IGamesService gamesService;
        private Game selectedGame;

        private ObservableCollection<Game> games;

        public ObservableCollection<Game> Games
        {
            get { return this.games; }
        }

        public Game SelectedGame
        {
            get { return this.selectedGame; }
            set { this.SetProperty(ref this.selectedGame, value); }
        }

        public ICommand JoinGameCommand { get; }

        public GamesViewModel(IAuthenticator authenticator, IGamesService gamesService)
        {
            this.authenticator = authenticator;
            this.gamesService = gamesService;
            this.games = new ObservableCollection<Game>(this.gamesService.GetAll());
            this.JoinGameCommand = new DelegateCommand(JoinGame);
        }

        private void JoinGame(object obj)
        {
            this.gamesService.AddPlayer((int)obj, this.authenticator.CurrentUser);
        }
    }
}
