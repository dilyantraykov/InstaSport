using Microsoft.EntityFrameworkCore;
using InstaSport.Data;

namespace InstaSport.Tests.Models
{
    public class TestInstaSportDbContext : InstaSportDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
        }
    }
}

