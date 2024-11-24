using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using InstaSport.Data.Common;
using InstaSport.Data.Common.Models;

namespace InstaSport.Tests.UnitTests
{
    [TestFixture]
    public class DbRepositoryTests
    {
        private DbContextOptions<TestDbContext> options;
        private TestDbContext context;
        private DbRepository<TestEntity> repository;

        [SetUp]
        public void SetUp()
        {
            // Configure the in-memory database
            options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Create a new context using the in-memory database
            context = new TestDbContext(options);

            // Initialize the repository
            repository = new DbRepository<TestEntity>(context);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public void Constructor_ShouldThrowArgumentException_WhenDbContextIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new DbRepository<TestEntity>(null));
            Assert.AreEqual("An instance of DbContext is required to use this repository. (Parameter 'context')", ex.Message);
        }

        [Test]
        public void All_ShouldReturnEntities_ThatAreNotDeleted()
        {
            // Arrange
            context.TestEntities.AddRange(
                new TestEntity { Id = 1, IsDeleted = false },
                new TestEntity { Id = 2, IsDeleted = true },
                new TestEntity { Id = 3, IsDeleted = false }
            );
            context.SaveChanges();

            // Act
            var result = repository.All().ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(x => !x.IsDeleted));
        }

        [Test]
        public void AllWithDeleted_ShouldReturnAllEntities()
        {
            // Arrange
            context.TestEntities.AddRange(
                new TestEntity { Id = 1, IsDeleted = false },
                new TestEntity { Id = 2, IsDeleted = true },
                new TestEntity { Id = 3, IsDeleted = false }
            );
            context.SaveChanges();

            // Act
            var result = repository.AllWithDeleted().ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void GetById_ShouldReturnEntity_WhenEntityExists()
        {
            // Arrange
            context.TestEntities.AddRange(
                new TestEntity { Id = 1, IsDeleted = false },
                new TestEntity { Id = 2, IsDeleted = true },
                new TestEntity { Id = 3, IsDeleted = false }
            );
            context.SaveChanges();

            // Act
            var result = repository.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void Add_ShouldAddEntity_AndSetCreatedOn()
        {
            // Arrange
            var entity = new TestEntity { Id = 1 };

            // Act
            repository.Add(entity);
            repository.Save();

            // Assert
            var addedEntity = context.TestEntities.Find(1);
            Assert.IsNotNull(addedEntity);
            Assert.AreEqual(DateTime.Now.Date, addedEntity.CreatedOn.Date);
        }

        [Test]
        public void Delete_ShouldMarkEntityAsDeleted_AndSetDeletedOn()
        {
            // Arrange
            var entity = new TestEntity { Id = 1 };
            context.TestEntities.Add(entity);
            context.SaveChanges();

            // Act
            repository.Delete(entity);
            repository.Save();

            // Assert
            var deletedEntity = context.TestEntities.Find(1);
            Assert.IsTrue(deletedEntity.IsDeleted);
            Assert.AreEqual(DateTime.Now.Date, deletedEntity.DeletedOn?.Date);
        }

        [Test]
        public void HardDelete_ShouldRemoveEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1 };
            context.TestEntities.Add(entity);
            context.SaveChanges();

            // Act
            repository.HardDelete(entity);
            repository.Save();

            // Assert
            var deletedEntity = context.TestEntities.Find(1);
            Assert.IsNull(deletedEntity);
        }

        [Test]
        public void Save_ShouldCallSaveChangesOnContext()
        {
            // Arrange
            var entity = new TestEntity { Id = 1 };
            repository.Add(entity);

            // Act
            repository.Save();

            // Assert
            var savedEntity = context.TestEntities.Find(1);
            Assert.IsNotNull(savedEntity);
        }

        public class TestEntity : BaseModel<int>
        {
        }

        public class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options)
                : base(options)
            {
            }

            public DbSet<TestEntity> TestEntities { get; set; }
        }
    }
}
