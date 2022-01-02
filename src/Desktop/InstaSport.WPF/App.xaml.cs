﻿using InstaSport.Data;
using InstaSport.Data.Common;
using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;
using Telerik.Windows.Controls;

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
        }

        protected override Window CreateShell()
        {
            return null;
        }

        protected override void OnInitialized()
        {
            MainWindow shellWindow = Container.Resolve<MainWindow>();
            shellWindow.Show();
            this.MainWindow = shellWindow.ParentOfType<Window>();
            //RegionManager.SetRegionManager(MainWindow, Container.Resolve<IRegionManager>());
            //RegionManager.UpdateRegions();
            InitializeModules();
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<DbContext, InstaSportDbContext>();
            containerRegistry.Register<IDbRepository<Game>, DbRepository<Game>>();
            containerRegistry.Register<IDbRepository<Location>, DbRepository<Location>>();
            containerRegistry.Register<IDbRepository<Sport>, DbRepository<Sport>>();
            containerRegistry.Register<IDbRepository<User>, DbRepository<User>>();
            containerRegistry.Register<IPasswordHasher<User>, PasswordHasher<User>>();
            containerRegistry.Register<IAuthenticator, Authenticator>();
            containerRegistry.Register<IAuthenticationService, AuthenticationService>();
            containerRegistry.Register<IGamesService, GamesService>();
            containerRegistry.Register<ILocationsService, LocationsService>();
            containerRegistry.Register<ISportsService, SportsService>();
        }
    }
}