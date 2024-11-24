using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.Data.Common;
using InstaSport.Tests.Models;

namespace InstaSport.Tests.IntegrationTests
{
    [TestFixture]
    public class GameServiceIntegrationTests
    {
        private DbContextOptions<TestInstaSportDbContext> options;
        private TestInstaSportDbContext context;
        private IDbRepository<Game> gamesRepository;
        private GamesService gamesService;

        [SetUp]
        public void SetUp()
        {
            // Create a new context using the in-memory database
            context = new TestInstaSportDbContext();

            // Initialize the repository and service
            gamesRepository = new DbRepository<Game>(context);
            gamesService = new GamesService(gamesRepository);

            // Seed the database with test data
            SeedDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public void GetAll_ShouldReturnAllGamesOrderedByDate()
        {
            // Act
            var result = gamesService.GetAll().ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new DateTime(2023, 1, 1), result[0].StartingDateTime);
            Assert.AreEqual(new DateTime(2023, 2, 1), result[1].StartingDateTime);
            Assert.AreEqual(new DateTime(2023, 3, 1), result[2].StartingDateTime);
        }

        [Test]
        public void GetById_ShouldReturnCorrectGame()
        {
            // Act
            var result = gamesService.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(new DateTime(2023, 1, 1), result.StartingDateTime);
        }

        [Test]
        public void GetCount_ShouldReturnCorrectCount()
        {
            // Act
            var result = gamesService.GetCount();

            // Assert
            Assert.AreEqual(3, result);
        }

        private void SeedDatabase()
        {
            context.Games.AddRange(
                new Game { Id = 1, StartingDateTime = new DateTime(2023, 1, 1), SportId = 1, LocationId = 1 },
                new Game { Id = 2, StartingDateTime = new DateTime(2023, 2, 1), SportId = 2, LocationId = 2 },
                new Game { Id = 3, StartingDateTime = new DateTime(2023, 3, 1), SportId = 3, LocationId = 3 }
            );

            context.SaveChanges();
        }
    }
}
