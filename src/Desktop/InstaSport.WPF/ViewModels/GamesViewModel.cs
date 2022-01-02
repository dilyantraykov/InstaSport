using InstaSport.Data.Models;
using InstaSport.Services.Data;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class GamesViewModel : BindableBase
    {
        private IGamesService gamesService;

        private ObservableCollection<Game> games;

        public ObservableCollection<Game> Games
        {
            get { return this.games; }
        }

        public GamesViewModel(IGamesService gamesService)
        {
            this.gamesService = gamesService;
            this.games = new ObservableCollection<Game>(this.gamesService.GetAll());
        }
    }
}
