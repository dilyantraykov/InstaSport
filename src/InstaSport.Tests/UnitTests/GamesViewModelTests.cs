using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.WPF.Models;
using InstaSport.WPF.State;
using InstaSport.WPF.ViewModels;
using InstaSport.WPF.Views;
using Moq;
using Prism.Regions;

namespace InstaSport.Tests.UnitTests
{
    [TestFixture]
    public class GamesViewModelTests
    {
        private Mock<IAuthenticator> authenticatorMock;
        private Mock<IGamesService> gamesServiceMock;
        private Mock<IRegionNavigationService> regionNavigationServiceMock;
        private Mock<IRegionManager> regionManagerMock;
        private GamesViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            authenticatorMock = new Mock<IAuthenticator>();
            gamesServiceMock = new Mock<IGamesService>();
            regionManagerMock = new Mock<IRegionManager>();
            regionNavigationServiceMock = new Mock<IRegionNavigationService>();

            viewModel = new GamesViewModel(
                authenticatorMock.Object,
                gamesServiceMock.Object,
                regionManagerMock.Object);
        }

        [Test]
        public void OnNavigatedTo_ShouldLoadGames()
        {
            // Arrange
            var games = new List<Game>
                        {
                            CreateGame(1, "City 1", "Location 1", "Sport 1"),
                            CreateGame(2, "City 2", "Location 2", "Sport 2")
                        }.AsQueryable();

            gamesServiceMock.Setup(s => s.GetAll()).Returns(games);

            var navigationContext = new NavigationContext(regionNavigationServiceMock.Object, new Uri("http://test"), null);

            // Act
            viewModel.OnNavigatedTo(navigationContext);

            // Assert
            Assert.AreEqual(2, viewModel.Games.Count);
        }

        [Test]
        public void OnNavigatedTo_ShouldFilterGamesBySport()
        {
            // Arrange
            var games = new List<Game>
                        {
                            CreateGame(1, "City 1", "Location 1", "Sport 1"),
                            CreateGame(2, "City 2", "Location 2", "Sport 2")
                        }.AsQueryable();

            gamesServiceMock.Setup(s => s.GetAll()).Returns(games);

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
            var games = new List<Game>
                        {
                            CreateGame(1, "City 1", "Location 1", "Sport 1"),
                            CreateGame(2, "City 2", "Location 2", "Sport 2")
                        }.AsQueryable();

            gamesServiceMock.Setup(s => s.GetAll()).Returns(games);

            var navigationContext = new NavigationContext(regionNavigationServiceMock.Object,
                new Uri(nameof(GamesView), UriKind.Relative),
                new NavigationParameters());

            navigationContext.Parameters.Add("Location", new LocationDto { Id = 1 });

            // Act
            viewModel.OnNavigatedTo(navigationContext);

            // Assert
            Assert.AreEqual(1, viewModel.Games.Count);
            Assert.AreEqual(1, viewModel.Games.First().LocationId);
        }

        private Game CreateGame(int id, string cityName, string locationName, string sportName)
        {
            return new Game
            {
                Id = id,
                SportId = id,
                LocationId = id,
                StartingDateTime = DateTime.Now,
                Location = new Location
                {
                    Id = id,
                    Name = locationName,
                    City = new City { Id = id, Name = cityName }
                },
                Sport = new Sport
                {
                    Id = id,
                    Name = sportName
                }
            };
        }
    }
}
