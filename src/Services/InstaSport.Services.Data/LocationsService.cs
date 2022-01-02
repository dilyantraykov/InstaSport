namespace InstaSport.Services.Data
{
    using System;
    using System.Linq;

    using InstaSport.Data.Common;
    using InstaSport.Data.Models;

    public class LocationsService : ILocationsService
    {
        private readonly IDbRepository<Location> locations;

        public LocationsService(IDbRepository<Location> locations)
        {
            this.locations = locations;
        }

        public IQueryable<Location> GetAll()
        {
            return this.locations.All();
        }

        public Location GetById(int id)
        {
            var location = this.locations.GetById(id);
            return location;
        }

        public int GetCount()
        {
            var count = this.locations.All().Count();
            return count;
        }

        public IQueryable<Location> GetLocationsByCity(int cityId)
        {
            return this.locations.All().Where(l => l.CityId == cityId);
        }

        public double Rate(int userId, int locationId, int rating)
        {
            var location = this.locations.All().FirstOrDefault(x => x.Id == locationId);
            var currentRating = location.Ratings.FirstOrDefault(x => x.AuthorId == userId);

            if (currentRating == null)
            {
                location.Ratings.Add(new Rating
                {
                    AuthorId = userId,
                    Value = rating
                });
            }
            else
            {
                currentRating.Value = rating;
            }

            this.locations.Save();

            var newRating = location.Ratings.Average(x => x.Value);
            return newRating;
        }
    }
}
