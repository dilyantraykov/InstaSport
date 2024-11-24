using InstaSport.Data.Common;
using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.Services.Data.Constants;
using InstaSport.Tests.Models;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.ViewModels;
using InstaSport.WPF.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Prism.Regions;
using System.Collections.Generic;
using System.Linq;

namespace InstaSport.Tests.IntegrationTests
{
    [TestFixture]
    public class GameDetailsViewModelIntegrationTests
    {
        private DbContextOptions<TestInstaSportDbContext> options;
        private TestInstaSportDbContext context;
        private Mock<IAuthenticator> authenticatorMock;
        private Mock<IRegionManager> regionManagerMock;
        private Mock<IRegionNavigationService> regionNavigationServiceMock;
        private GameDetailsViewModel viewModel;
        private IGamesService gamesService;
        private User currentUser;
        private GameDto game;

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

            currentUser = new User { Id = 1, UserName = "testuser" };
            authenticatorMock.Setup(a => a.CurrentUser).Returns(currentUser);

            game = new GameDto
            {
                Id = 1,
                Sport = new SportDto { Id = 1, Name = "Football" },
                SportId = 1,
                LocationId = 1,
                Location = new LocationDto { 
                    Id = 1, 
                    Name = "WinBet Center"
                },
                Players = new List<UserDto>()
            };

            viewModel = new GameDetailsViewModel(authenticatorMock.Object, gamesService, regionManagerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public void LeaveGameCommand_ShouldRemovePlayerFromGame()
        {
            // Arrange
            game.Players.Add(new UserDto { Id = currentUser.Id, UserName = currentUser.UserName });
            var navigationContext = new NavigationContext(regionNavigationServiceMock.Object, new Uri(nameof(GameDetailsView), UriKind.Relative));
            navigationContext.Parameters.Add("GameId", 1);
            viewModel.OnNavigatedTo(navigationContext);
            viewModel.Game = game;

            // Act
            viewModel.LeaveGameCommand.Execute(1);

            // Assert
            Assert.IsFalse(viewModel.Players.Any(p => p.Id == currentUser.Id));
        }

        [Test]
        public void OnNavigatedTo_ShouldSetGameProperty()
        {
            // Act
            var navigationContext = new NavigationContext(regionNavigationServiceMock.Object, new Uri(nameof(GameDetailsView), UriKind.Relative));
            navigationContext.Parameters.Add("GameId", 1);
            viewModel.OnNavigatedTo(navigationContext);

            // Assert
            Assert.IsNotNull(viewModel.Game);
            Assert.AreEqual(1, viewModel.Game.Id);
        }

        [Test]
        public void GoToUserProfileCommand_ShouldNavigateToUserProfile()
        {
            // Arrange
            var user = new UserDto { Id = 2, UserName = "otheruser" };
            var parameters = new NavigationParameters();
            parameters.Add("Username", user.UserName);

            // Act
            viewModel.GoToUserProfileCommand.Execute(user);

            // Assert
            regionManagerMock.Verify(r => r.RequestNavigate(StringConstants.MainRegionName, nameof(UserDetailsView), parameters), Times.Once);
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
