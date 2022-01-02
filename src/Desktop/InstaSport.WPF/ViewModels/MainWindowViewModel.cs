using InstaSport.Services.Data;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaSport.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public IAuthenticator authenticator;
        public ISportsService sportsService;
        public IGamesService gamesService;
        public ILocationsService locationsService;
        private BindableBase currentViewModel;

        public ObservableCollection<NavigationItem> NavigationItems { get; set; }

        public BindableBase CurrentViewModel
        {
            get { return this.currentViewModel; }
            set
            {
                this.SetProperty(ref this.currentViewModel, value);
            }
        }

        public MainWindowViewModel(IAuthenticator authenticator, ISportsService sportsService, IGamesService gamesService, ILocationsService locationsService)
        {
            this.authenticator = authenticator;
            this.sportsService = sportsService;
            this.gamesService = gamesService;
            this.locationsService = locationsService;
            this.NavigationItems = new ObservableCollection<NavigationItem>();

            this.NavigationItems.Add(new NavigationItem("Login", "&#xe500;", new LoginViewModel(this.authenticator)));
            this.NavigationItems.Add(new NavigationItem("Sports", "&#xe701;", new SportsViewModel(this.sportsService)));
            this.NavigationItems.Add(new NavigationItem("Games", "&#xe108;", new GamesViewModel(this.gamesService)));
        }
    }
}
