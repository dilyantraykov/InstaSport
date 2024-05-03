namespace InstaSport.Data
{
    using System;
    using System.Linq;
    using Common.Models;
    using InstaSport.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class InstaSportDbContext : DbContext
    {
        public DbSet<Sport> Sports { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Data Source=.;Initial Catalog=InstaSport;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var sports = new List<Sport>()
            {
                new Sport { Id = 1, Name = "Football" },
                new Sport { Id = 2, Name = "Basketball" },
                new Sport { Id = 3, Name = "Tennis" }
            };

            modelBuilder.Entity<Sport>().HasData(sports);

            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Sofia" },
                new City { Id = 2, Name = "Plovdiv" },
                new City { Id = 3, Name = "Varna" }
            );

            var locations = new List<Location>() 
            {
                new Location 
                { 
                    Id = 1,
                    CityId = 1,
                    Name = "Tsarsko Selo", 
                    Latitude = 42.6373743M, 
                    Longitude = 23.315249M
                },
                new Location 
                { 
                    Id = 2,
                    CityId = 2,
                    Name = "Sportna Sofia", 
                    Latitude = 42.6846187M, 
                    Longitude = 23.3356765M
                },
                new Location 
                { 
                    Id = 3,
                    CityId = 3,
                    Name = "Musagenitsa Sport", 
                    Latitude = 42.6589267M, 
                    Longitude = 23.3634143M 
                }
            };

            modelBuilder.Entity<Location>().HasData(locations);

            modelBuilder.Entity<Game>().HasData(
                new Game 
                { 
                    Id = 1, 
                    LocationId = 1,
                    SportId = 1,
                    StartingDateTime = DateTime.Now.AddHours(20),
                    MinPlayers = 6,
                    MaxPlayers = 12,
                    Status = GameStatus.WaitingForPlayers
                },
                new Game 
                { 
                    Id = 2, 
                    LocationId = 2, 
                    SportId = 2,
                    StartingDateTime = DateTime.Now,
                    MinPlayers = 6,
                    MaxPlayers = 12,
                    Status = GameStatus.Playing
                },
                new Game 
                { 
                    Id = 3, 
                    LocationId = 3,
                    SportId = 3,
                    StartingDateTime = DateTime.Now.AddHours(-1),
                    MinPlayers = 2,
                    MaxPlayers = 2,
                    Status = GameStatus.Finished
                }
            );

            modelBuilder.Entity<Rating>()
               .HasOne(r => r.Author)
               .WithMany(u => u.AuthoredRatings)
               .HasForeignKey(c => c.AuthorId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rating>()
                .HasOne(c => c.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
