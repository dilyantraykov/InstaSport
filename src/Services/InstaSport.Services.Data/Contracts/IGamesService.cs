namespace InstaSport.Services.Data
{
    using System.Linq;
    using InstaSport.Data.Models;

    public interface IGamesService
    {
        IQueryable<Game> GetAll();

        IQueryable<Game> GetUpcoming();

        IQueryable<Game> GetByCity(int cityId);

        IQueryable<Game> GetBySport(int sportId);

        int GetCount();

        Game GetById(int id);

        int AddPlayer(int gameId, User player);

        int RemovePlayer(int gameId, User player);

        int Create(Game game);

        void Save();
    }
}
