﻿using InstaSport.Services.Data;
using InstaSport.Services.Data.Constants;
using InstaSport.Services.Data.Localization;
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
        private readonly ISportsService sportsService;
        private readonly IGamesService gamesService;
        private readonly ILocationsService locationsService;
        private readonly IRegionManager regionManager;
        private string selectedView;

        public IAuthenticator Authenticator { get; }

        public ObservableCollection<NavigationItem> NavigationItems { get; set; }

        public ICommand SelectedNavigationItemChangedCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        private bool programmaticSelection;

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
            this.NavigationItems.Add(new NavigationItem(Strings.LoginNavItemLabel, "&#xf2f6;", nameof(LoginView)));
            this.NavigationItems.Add(new NavigationItem(Strings.RegisterNavItemLabel, "&#xf234;", nameof(RegistrationView)));
            this.NavigationItems.Add(new NavigationItem(Strings.SportsNavItemLabel, "&#xf1e3;", nameof(SportsView)));
            this.NavigationItems.Add(new NavigationItem(Strings.LocationsNavItemLabel, "&#xf3c5;", nameof(LocationsView)));
            this.NavigationItems.Add(new NavigationItem(Strings.GamesNavItemLabel, "&#xf073;", nameof(GamesView)));
            this.NavigationItems.Add(new NavigationItem(Strings.CreateGameNavItemLabel, "&#xf271;", nameof(CreateGameView)) { IsVisible = false });

            this.SelectedNavigationItemChangedCommand = new DelegateCommand(OnSelectedNavigationItemChanged);
            this.LogOutCommand = new DelegateCommand(OnLogOut);

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
            this.NavigationItems.First(x => x.Title == Strings.LoginNavItemLabel).IsVisible = this.Authenticator.CurrentUser == null;
            this.NavigationItems.First(x => x.Title == Strings.RegisterNavItemLabel).IsVisible = this.Authenticator.CurrentUser == null;
            this.NavigationItems.First(x => x.Title == Strings.CreateGameNavItemLabel).IsVisible = this.Authenticator.CurrentUser != null;
        }
    }
}
