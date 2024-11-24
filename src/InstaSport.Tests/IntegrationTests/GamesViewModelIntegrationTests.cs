using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.ViewModels;
using Prism.Regions;
using InstaSport.Tests.Models;
using InstaSport.WPF.Views;
using InstaSport.Data.Common;

namespace InstaSport.Tests.IntegrationTests
{
    [TestFixture]
    public class GamesViewModelIntegrationTests
    {
        private DbContextOptions<TestInstaSportDbContext> options;
        private TestInstaSportDbContext context;
        private Mock<IAuthenticator> authenticatorMock;
        private Mock<IRegionNavigationService> regionNavigationServiceMock;
        private Mock<IRegionManager> regionManagerMock;
        private GamesViewModel viewModel;
        private IGamesService gamesService;

        [SetUp]
        public void SetUp()
        {
            // Create a new context using the in-memory database
            context = new TestInstaSportDbContext();

            // Initialize the repository and service
            var gamesRepository = new DbRepository<Game>(context);
            gamesService = new GamesService(gamesRepository);

            // Seed the database with test data
            SeedDatabase();

            authenticatorMock = new Mock<IAuthenticator>();
            regionManagerMock = new Mock<IRegionManager>();
            regionNavigationServiceMock = new Mock<IRegionNavigationService>();

            viewModel = new GamesViewModel(
                authenticatorMock.Object,
                gamesService,
                regionManagerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public void OnNavigatedTo_ShouldLoadGames()
        {
            // Arrange
            var navigationContext = new NavigationContext(regionNavigationServiceMock.Object, new Uri("http://test"), null);

            // Act
            viewModel.OnNavigatedTo(navigationContext);

            // Assert
            Assert.AreEqual(3, viewModel.Games.Count);
        }

        [Test]
        public void OnNavigatedTo_ShouldFilterGamesBySport()
        {
            // Arrange
            var navigationContext = new NavigationContext(regionNavigationServiceMock.Object,
                new Uri(nameof(GamesView), UriKind.Relative));
            navigationContext.Parameters.Add("Sport", new SportDto { Id = 1 });

            // Act
            viewModel.OnNavigatedTo(navigationContext);

            // Assert
            Assert.AreEqual(1, viewModel.Games.Count);
            Assert.AreEqual(1, viewModel.Games.First().SportId);
        }

        [Test]
        public void OnNavigatedTo_ShouldFilterGamesByLocation()
        {
            // Arrange
            var navigationContext = new NavigationContext(regionNavigationServiceMock.Object,
                new Uri(nameof(GamesView), UriKind.Relative));
            navigationContext.Parameters.Add("Location", new LocationDto { Id = 1 });

            // Act
            viewModel.OnNavigatedTo(navigationContext);

            // Assert
            Assert.AreEqual(1, viewModel.Games.Count);
            Assert.AreEqual(1, viewModel.Games.First().LocationId);
        }

        private void SeedDatabase()
        {
            context.Games.AddRange(
                new Game { Id = 1, StartingDateTime = DateTime.Now, SportId = 1, LocationId = 1, Sport = new Sport { Id = 1, Name = "Sport 1" }, Location = new Location { Id = 1, Name = "Location 1", City = new City { Id = 1, Name = "City 1" } } },
                new Game { Id = 2, StartingDateTime = DateTime.Now, SportId = 2, LocationId = 2, Sport = new Sport { Id = 2, Name = "Sport 2" }, Location = new Location { Id = 2, Name = "Location 2", City = new City { Id = 2, Name = "City 2" } } },
                new Game { Id = 3, StartingDateTime = DateTime.Now, SportId = 3, LocationId = 3, Sport = new Sport { Id = 3, Name = "Sport 3" }, Location = new Location { Id = 3, Name = "Location 3", City = new City { Id = 3, Name = "City 3" } } }
            );

            context.SaveChanges();
        }
    }
}
