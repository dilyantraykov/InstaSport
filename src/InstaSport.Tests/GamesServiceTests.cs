using InstaSport.Data.Common;
using InstaSport.Data.Models;
using InstaSport.Services.Data;
using Moq;

namespace InstaSport.Tests;

[TestFixture]
public class GamesServiceTests
{
    private Mock<IDbRepository<Game>> mockGameRepository;
    private GamesService gamesService;

    [SetUp]
    public void SetUp()
    {
        mockGameRepository = new Mock<IDbRepository<Game>>();
        gamesService = new GamesService(mockGameRepository.Object);
    }

    [Test]
    public void AddPlayer_ShouldAddPlayerToGame()
    {
        // Arrange
        var game = new Game { Id = 1, Players = new List<User>() };
        var player = new User { Id = 1 };
        mockGameRepository.Setup(repo => repo.GetById(1)).Returns(game);

        // Act
        var playerCount = gamesService.AddPlayer(1, player);

        // Assert
        Assert.That(playerCount, Is.EqualTo(1));
        Assert.That(game.Players, Does.Contain(player));
        mockGameRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [Test]
    public void RemovePlayer_ShouldRemovePlayerFromGame()
    {
        // Arrange
        var player = new User { Id = 1 };
        var game = new Game { Id = 1, Players = new List<User> { player } };
        mockGameRepository.Setup(repo => repo.GetById(1)).Returns(game);

        // Act
        var playerCount = gamesService.RemovePlayer(1, player);

        // Assert
        Assert.AreEqual(0, playerCount);
        Assert.IsFalse(game.Players.Contains(player));
        mockGameRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [Test]
    public void Create_ShouldAddGame()
    {
        // Arrange
        var game = new Game { Id = 1 };

        // Act
        var gameId = gamesService.Create(game);

        // Assert
        Assert.AreEqual(1, gameId);
        mockGameRepository.Verify(repo => repo.Add(game), Times.Once);
        mockGameRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [Test]
    public void GetAll_ShouldReturnAllGames()
    {
        // Arrange
        var games = new List<Game> { new Game { Id = 1 }, new Game { Id = 2 } }.AsQueryable();
        mockGameRepository.Setup(repo => repo.All()).Returns(games);

        // Act
        var result = gamesService.GetAll();

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual(games, result);
    }

    [Test]
    public void GetByCity_ShouldReturnGamesInCity()
    {
        // Arrange
        var games = new List<Game>
        {
            new Game { Id = 1, Location = new Location { CityId = 1 } },
            new Game { Id = 2, Location = new Location { CityId = 2 } }
        }.AsQueryable();
        mockGameRepository.Setup(repo => repo.All()).Returns(games);

        // Act
        var result = gamesService.GetByCity(1);

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual(1, result.First().Id);
    }

    [Test]
    public void GetById_ShouldReturnGameById()
    {
        // Arrange
        var game = new Game { Id = 1 };
        mockGameRepository.Setup(repo => repo.GetById(1)).Returns(game);

        // Act
        var result = gamesService.GetById(1);

        // Assert
        Assert.AreEqual(game, result);
    }

    [Test]
    public void GetBySport_ShouldReturnGamesBySport()
    {
        // Arrange
        var games = new List<Game>
        {
            new Game { Id = 1, SportId = 1 },
            new Game { Id = 2, SportId = 2 }
        }.AsQueryable();
        mockGameRepository.Setup(repo => repo.All()).Returns(games);

        // Act
        var result = gamesService.GetBySport(1);

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual(1, result.First().Id);
    }

    [Test]
    public void GetCount_ShouldReturnGameCount()
    {
        // Arrange
        var games = new List<Game> { new Game { Id = 1 }, new Game { Id = 2 } }.AsQueryable();
        mockGameRepository.Setup(repo => repo.All()).Returns(games);

        // Act
        var count = gamesService.GetCount();

        // Assert
        Assert.AreEqual(2, count);
    }

    [Test]
    public void GetUpcoming_ShouldReturnUpcomingGames()
    {
        // Arrange
        var games = new List<Game>
        {
            new Game { Id = 1, StartingDateTime = DateTime.UtcNow.AddHours(1), Status = GameStatus.WaitingForPlayers },
            new Game { Id = 2, StartingDateTime = DateTime.UtcNow.AddHours(-1), Status = GameStatus.WaitingForPlayers }
        }.AsQueryable();
        mockGameRepository.Setup(repo => repo.All()).Returns(games);

        // Act
        var result = gamesService.GetUpcoming();

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual(1, result.First().Id);
    }
}