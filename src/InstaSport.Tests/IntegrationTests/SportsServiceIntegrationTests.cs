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
    public class SportsServiceIntegrationTests
    {
        private DbContextOptions<TestInstaSportDbContext> options;
        private TestInstaSportDbContext context;
        private IDbRepository<Sport> sportsRepository;
        private SportsService sportsService;

        [SetUp]
        public void SetUp()
        {
            // Create a new context using the in-memory database
            context = new TestInstaSportDbContext();

            // Initialize the repository and service
            sportsRepository = new DbRepository<Sport>(context);
            sportsService = new SportsService(sportsRepository);

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
        public void GetAll_ShouldReturnAllSportsOrderedByName()
        {
            // Act
            var result = sportsService.GetAll().ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Sport 1", result[0].Name);
            Assert.AreEqual("Sport 2", result[1].Name);
            Assert.AreEqual("Sport 3", result[2].Name);
        }

        [Test]
        public void GetById_ShouldReturnCorrectSport()
        {
            // Act
            var result = sportsService.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Sport 1", result.Name);
        }

        [Test]
        public void GetCount_ShouldReturnCorrectCount()
        {
            // Act
            var result = sportsService.GetCount();

            // Assert
            Assert.AreEqual(3, result);
        }

        private void SeedDatabase()
        {
            context.Sports.AddRange(
                new Sport { Id = 1, Name = "Sport 1" },
                new Sport { Id = 2, Name = "Sport 2" },
                new Sport { Id = 3, Name = "Sport 3" }
            );

            context.SaveChanges();
        }
    }
}

