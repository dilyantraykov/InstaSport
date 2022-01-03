using InstaSport.Services.Data;
using InstaSport.Services.Data.Constants;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private ISportsService sportsService;
        private IGamesService gamesService;
        private ILocationsService locationsService;
        private IRegionManager regionManager;
        private string selectedView;

        public IAuthenticator Authenticator { get; }

        public ObservableCollection<NavigationItem> NavigationItems { get; set; }

        public ICommand SelectedNavigationItemChangedCommand { get; set; }

        public string SelectedView
        {
            get { return this.selectedView; }
            set
            {
                this.SetProperty(ref this.selectedView, value);
            }
        }

        public MainWindowViewModel(IRegionManager regionManager, 
            IAuthenticator authenticator, 
            ISportsService sportsService, 
            IGamesService gamesService, 
            ILocationsService locationsService)
        {
            this.Authenticator = authenticator;
            this.Authenticator.CurrentUserChanged += AuthenticatorCurrentUserChanged;

            this.sportsService = sportsService;
            this.gamesService = gamesService;
            this.locationsService = locationsService;

            this.NavigationItems = new ObservableCollection<NavigationItem>();
            this.NavigationItems.Add(new NavigationItem("Login", "&#xf2f6;", nameof(LoginView)));
            this.NavigationItems.Add(new NavigationItem("Register", "&#xf234;", nameof(RegistrationView)));
            this.NavigationItems.Add(new NavigationItem("Sports", "&#xf1e3;", nameof(SportsView)));
            this.NavigationItems.Add(new NavigationItem("Locations", "&#xf3c5;", nameof(LocationsView)));
            this.NavigationItems.Add(new NavigationItem("Games", "&#xf073;", nameof(GamesView)));

            this.SelectedNavigationItemChangedCommand = new DelegateCommand(OnSelectedNavigationItemChanged);

            this.regionManager = regionManager;
            this.regionManager.Regions.CollectionChanged += OnRegionsCollectionChanged;

            this.SelectedView = nameof(LoginView);
        }

        private void OnRegionsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var region = e.NewItems[0] as Region;
            if (region.Name == StringConstants.MainRegionName)
            {
                region.NavigationService.Navigated += NavigationServiceOnNavigated;
            }
        }

        private void OnSelectedNavigationItemChanged(object obj)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                var args = obj as SelectionChangedEventArgs;
                var newItem = args.AddedItems[0] as NavigationItem;
                this.regionManager.RequestNavigate(StringConstants.MainRegionName, newItem.View);
            }));
        }

        private void NavigationServiceOnNavigated(object? sender, RegionNavigationEventArgs e)
        {
            this.SelectedView = e.Uri.OriginalString;
        }

        private void AuthenticatorCurrentUserChanged(object? sender, EventArgs e)
        {
            this.NavigationItems.First(x => x.Title == "Login").IsVisible = this.Authenticator.CurrentUser == null;
            this.NavigationItems.First(x => x.Title == "Register").IsVisible = this.Authenticator.CurrentUser == null;
        }
    }
}
