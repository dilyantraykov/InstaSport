using InstaSport.Data;
using InstaSport.Data.Common;
using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.Services.Data.Localization;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Windows.Media.Protection.PlayReady;

namespace InstaSport.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            StyleManager.ApplicationTheme = new FluentTheme();
            FluentPalette.LoadPreset(FluentPalette.ColorVariation.Dark);
            this.DispatcherUnhandledException += OnAppDispatcherUnhandledException;
        }

        private void OnAppDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        protected override Window CreateShell()
        {
            return null;
        }

        protected override void OnInitialized()
        {
            Container.Resolve<DbContext>().Database.EnsureCreated();

            var font = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Font Awesome 5 Free Regular");
            RadGlyph.RegisterFont(font, "FontAwesome");

            var defaultCulture = new CultureInfo("bg-BG");
            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = defaultCulture;

            LocalizationManager.UseDynamicLocalization = true;
            LocalizationManager.Manager = new LocalizationManager()
            {
                ResourceManager = Strings.ResourceManager,
                Culture = defaultCulture
            };

            MainWindow shellWindow = Container.Resolve<MainWindow>();
            shellWindow.Show();
            this.MainWindow = shellWindow.ParentOfType<Window>();

            RegionManager.SetRegionManager(MainWindow, Container.Resolve<IRegionManager>());
            RegionManager.UpdateRegions();
            InitializeModules();

            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterScoped<DbContext, InstaSportDbContext>();
            containerRegistry.Register<IDbRepository<Game>, DbRepository<Game>>();
            containerRegistry.Register<IDbRepository<Location>, DbRepository<Location>>();
            containerRegistry.Register<IDbRepository<Sport>, DbRepository<Sport>>();
            containerRegistry.Register<IDbRepository<User>, DbRepository<User>>();
            containerRegistry.Register<IPasswordHasher<User>, PasswordHasher<User>>();
            containerRegistry.RegisterSingleton<IAuthenticator, Authenticator>();
            containerRegistry.Register<IAuthenticationService, AuthenticationService>();
            containerRegistry.Register<IGamesService, GamesService>();
            containerRegistry.Register<ILocationsService, LocationsService>();
            containerRegistry.Register<ISportsService, SportsService>();

            // ...

            containerRegistry.RegisterForNavigation<LoginView>();
            containerRegistry.RegisterForNavigation<RegistrationView>();
            containerRegistry.RegisterForNavigation<GamesView>();
            containerRegistry.RegisterForNavigation<SportsView>();
            containerRegistry.RegisterForNavigation<LocationsView>();
            containerRegistry.RegisterForNavigation<GameDetailsView>();
            containerRegistry.RegisterForNavigation<CreateGameView>();
            containerRegistry.RegisterForNavigation<UserDetailsView>();
        }
    }
}
