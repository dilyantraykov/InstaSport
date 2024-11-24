using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Prism.Regions;
using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.ViewModels;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.Views;

namespace InstaSport.Tests.UnitTests
{
    [TestFixture]
    public class CreateGameViewModelTests
    {
        private Mock<IAuthenticator> authenticatorMock;
        private Mock<IGamesService> gamesServiceMock;
        private Mock<ILocationsService> locationsServiceMock;
        private Mock<ISportsService> sportsServiceMock;
        private Mock<IRegionManager> regionManagerMock;
        private CreateGameViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            // Mock the dependencies
            authenticatorMock = new Mock<IAuthenticator>();
            gamesServiceMock = new Mock<IGamesService>();
            locationsServiceMock = new Mock<ILocationsService>();
            sportsServiceMock = new Mock<ISportsService>();
            regionManagerMock = new Mock<IRegionManager>();

            // Setup mock data
            locationsServiceMock.Setup(s => s.GetAll()).Returns(GetTestLocations().AsQueryable());
            sportsServiceMock.Setup(s => s.GetAll()).Returns(GetTestSports().AsQueryable());

            // Initialize the CreateGameViewModel
            viewModel = new CreateGameViewModel(authenticatorMock.Object, gamesServiceMock.Object, locationsServiceMock.Object, sportsServiceMock.Object, regionManagerMock.Object);
        }

        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            // Assert
            Assert.IsNotNull(viewModel.Locations);
            Assert.IsNotNull(viewModel.Sports);
            Assert.IsNotNull(viewModel.Game);
            Assert.AreEqual(0, viewModel.Game.MinPlayers);
            Assert.AreEqual(0, viewModel.Game.MaxPlayers);
            Assert.AreEqual(DateTime.Now.Date, viewModel.Game.StartingDateTime.Date);
        }

        [Test]
        public void CreateGameCommand_ShouldCreateGameAndNavigateToGameDetailsView()
        {
            // Arrange
            var game = new Game { Id = 1 };
            gamesServiceMock.Setup(s => s.Create(It.IsAny<Game>())).Callback<Game>(g => g.Id = game.Id);
            authenticatorMock.Setup(a => a.CurrentUser).Returns(new User { Id = 1 });

            // Act
            viewModel.CreateGameCommand.Execute(null);

            // Assert
            gamesServiceMock.Verify(s => s.Create(It.IsAny<Game>()), Times.Once);
            gamesServiceMock.Verify(s => s.AddPlayer(game.Id, authenticatorMock.Object.CurrentUser), Times.Once);
            regionManagerMock.Verify(r => r.RequestNavigate("MainRegion", nameof(GameDetailsView), It.IsAny<NavigationParameters>()), Times.Once);
        }

        private IEnumerable<Location> GetTestLocations()
        {
            return new List<Location>
            {
                new Location { Id = 1, Name = "Location 1", Latitude = 40.712776m, Longitude = -74.005974m, CityId = 1, City = new City { Id = 1, Name = "City 1" } },
                new Location { Id = 2, Name = "Location 2", Latitude = 34.052235m, Longitude = -118.243683m, CityId = 2, City = new City { Id = 2, Name = "City 2" } },
                new Location { Id = 3, Name = "Location 3", Latitude = 51.507351m, Longitude = -0.127758m, CityId = 3, City = new City { Id = 3, Name = "City 3" } }
            };
        }

        private IEnumerable<Sport> GetTestSports()
        {
            return new List<Sport>
            {
                new Sport { Id = 1, Name = "Sport 1" },
                new Sport { Id = 2, Name = "Sport 2" },
                new Sport { Id = 3, Name = "Sport 3" }
            };
        }
    }
}
