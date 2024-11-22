using InstaSport.Services.Data;
using InstaSport.Services.Data.Constants;
using InstaSport.Services.Data.Localization;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace InstaSport.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ISportsService sportsService;
        private readonly IGamesService gamesService;
        private readonly ILocationsService locationsService;
        private readonly IRegionManager regionManager;
        private string lastSelectedView;
        private NavigationParameters lastParameters;
        private string selectedView;
        private FluentPalette.ColorVariation currentVariation = FluentPalette.ColorVariation.Dark;

        public IAuthenticator Authenticator { get; }

        public ObservableCollection<NavigationItem> NavigationItems { get; set; }

        public ICommand SelectedNavigationItemChangedCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        public ICommand ToggleThemeCommand { get; set; }

        public ICommand ToggleLanguageCommand { get; set; }

        private bool programmaticSelection;

        public string SelectedView
        {
            get { return this.selectedView; }
            set
            {
                this.SetProperty(ref this.selectedView, value);
            }
        }

        public string ToggleLanguageContent
        {
            get 
            {
                var currentLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                this.RaisePropertyChanged(nameof(ToggleThemeContent));
                var newLanguage = currentLanguage == "bg" ? "Български" : "English";
                return newLanguage;
            }
        }

        public string ToggleThemeContent
        {
            get 
            { 
                var newTheme = this.currentVariation == FluentPalette.ColorVariation.Dark ? Strings.DarkThemeLabel : Strings.LightThemeLabel;
                return newTheme;
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
            this.RefreshNavigationItems();

            this.SelectedNavigationItemChangedCommand = new DelegateCommand(OnSelectedNavigationItemChanged);
            this.LogOutCommand = new DelegateCommand(OnLogOut);
            this.ToggleThemeCommand = new DelegateCommand(OnToggleTheme);
            this.ToggleLanguageCommand = new DelegateCommand(OnToggleLanguage);

            this.regionManager = regionManager;
            this.regionManager.Regions.CollectionChanged += OnRegionsCollectionChanged;

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                this.SelectedView = nameof(LoginView);
            }));
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
            var args = obj as SelectionChangedEventArgs;
            if (!this.programmaticSelection && args != null && args.AddedItems.Count > 0)
            {
                var newItem = args.AddedItems[0] as NavigationItem;
                this.regionManager.RequestNavigate(StringConstants.MainRegionName, newItem.View);
            }
            else if (args.AddedItems.Count == 0 && args.RemovedItems.Count > 0)
            {
                var name = regionManager.Regions[StringConstants.MainRegionName].NavigationService.Journal.CurrentEntry.Uri.OriginalString;
                var parameters = regionManager.Regions[StringConstants.MainRegionName].NavigationService.Journal.CurrentEntry.Parameters;
                this.lastSelectedView = name;
                this.lastParameters = parameters;
            }
        }

        private void OnToggleLanguage(object obj)
        {
            var currentLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            if (currentLanguage == "bg")
            {
                this.SetCulture("en");
            }
            else
            {
                this.SetCulture("bg");
            }
        }

        private void SetCulture(string name)
        {
            var newCulture = new CultureInfo(name);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
            LocalizationManager.Manager.Culture = newCulture;
            this.RefreshNavigationItems();
            this.RefreshView();
            this.RaisePropertyChanged(nameof(ToggleLanguageContent));
        }

        private void RefreshView()
        {
            this.regionManager.RequestNavigate(StringConstants.MainRegionName, string.Empty);

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                this.regionManager.RequestNavigate(StringConstants.MainRegionName, this.lastSelectedView, lastParameters);
            }), DispatcherPriority.Background);
        }

        private void RefreshNavigationItems()
        {
            this.NavigationItems.Clear();
            if (this.Authenticator.CurrentUser == null)
            {
                this.NavigationItems.Add(new NavigationItem(Strings.LoginNavItemLabel, "&#xf2f6;", nameof(LoginView)));
                this.NavigationItems.Add(new NavigationItem(Strings.RegisterNavItemLabel, "&#xf234;", nameof(RegistrationView)));
            }
            this.NavigationItems.Add(new NavigationItem(Strings.SportsNavItemLabel, "&#xf1e3;", nameof(SportsView)));
            this.NavigationItems.Add(new NavigationItem(Strings.LocationsNavItemLabel, "&#xf3c5;", nameof(LocationsView)));
            this.NavigationItems.Add(new NavigationItem(Strings.GamesNavItemLabel, "&#xf073;", nameof(GamesView)));
            if (this.Authenticator.CurrentUser != null)
            {
                this.NavigationItems.Add(new NavigationItem(Strings.CreateGameNavItemLabel, "&#xf271;", nameof(CreateGameView)));
            }
        }

        private void OnToggleTheme(object obj)
        {
            if (this.currentVariation == FluentPalette.ColorVariation.Dark)
            {
                FluentPalette.LoadPreset(FluentPalette.ColorVariation.Light);
                this.currentVariation = FluentPalette.ColorVariation.Light;
            }
            else
            {
                FluentPalette.LoadPreset(FluentPalette.ColorVariation.Dark);
                this.currentVariation = FluentPalette.ColorVariation.Dark;
            }

            this.RaisePropertyChanged(nameof(ToggleThemeContent));
        }

        private void OnLogOut(object obj)
        {
            this.Authenticator.LogOut();
        }

        private void NavigationServiceOnNavigated(object? sender, RegionNavigationEventArgs e)
        {
            this.programmaticSelection = true;
            this.SelectedView = e.Uri.OriginalString;
            this.programmaticSelection = false;
        }

        private void AuthenticatorCurrentUserChanged(object? sender, EventArgs e)
        {
            this.RefreshNavigationItems();
        }
    }
}
