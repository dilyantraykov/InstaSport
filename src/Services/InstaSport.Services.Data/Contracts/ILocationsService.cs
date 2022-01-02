namespace InstaSport.Services.Data
{
    using System.Linq;

    using InstaSport.Data.Models;

    public interface ILocationsService
    {
        IQueryable<Location> GetAll();

        IQueryable<Location> GetLocationsByCity(int cityId);

        int GetCount();

        double Rate(int userId, int locationId, int rating);

        Location GetById(int id);
    }
}
