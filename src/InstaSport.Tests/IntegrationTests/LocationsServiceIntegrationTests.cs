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
    public class LocationsServiceIntegrationTests
    {
        private DbContextOptions<TestInstaSportDbContext> options;
        private TestInstaSportDbContext context;
        private IDbRepository<Location> locationsRepository;
        private LocationsService locationsService;

        [SetUp]
        public void SetUp()
        {
            // Create a new context using the in-memory database
            context = new TestInstaSportDbContext();

            // Initialize the repository and service
            locationsRepository = new DbRepository<Location>(context);
            locationsService = new LocationsService(locationsRepository);

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
        public void GetAll_ShouldReturnAllLocationsOrderedByName()
        {
            // Act
            var result = locationsService.GetAll().ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Location 1", result[0].Name);
            Assert.AreEqual("Location 2", result[1].Name);
            Assert.AreEqual("Location 3", result[2].Name);
        }

        [Test]
        public void GetById_ShouldReturnCorrectLocation()
        {
            // Act
            var result = locationsService.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Location 1", result.Name);
        }

        [Test]
        public void GetCount_ShouldReturnCorrectCount()
        {
            // Act
            var result = locationsService.GetCount();

            // Assert
            Assert.AreEqual(3, result);
        }

        private void SeedDatabase()
        {
            context.Locations.AddRange(
                new Location { Id = 1, Name = "Location 1" },
                new Location { Id = 2, Name = "Location 2" },
                new Location { Id = 3, Name = "Location 3" }
            );

            context.SaveChanges();
        }
    }
}
