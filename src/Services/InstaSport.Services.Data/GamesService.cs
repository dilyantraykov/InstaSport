namespace InstaSport.Services.Data
{
    using System;
    using System.Linq;
    using InstaSport.Data.Common;
    using InstaSport.Data.Models;

    public class GamesService : IGamesService
    {
        private readonly IDbRepository<Game> games;

        public GamesService(IDbRepository<Game> games)
        {
            this.games = games;
        }

        public int AddPlayer(int gameId, User player)
        {
            var game = this.games.GetById(gameId);
            game.Players.Add(player);
            this.games.Save();

            return game.Players.Count;
        }

        public int RemovePlayer(int gameId, User player)
        {
            var game = this.games.GetById(gameId);
            game.Players.Remove(game.Players.FirstOrDefault(p => p.Id == player.Id));
            this.games.Save();

            return game.Players.Count;
        }

        public int Create(Game game)
        {
            this.games.Add(game);
            this.games.Save();

            return game.Id;
        }

        public IQueryable<Game> GetAll()
        {
            var games = this.games.All();
            return games;
        }

        public IQueryable<Game> GetByCity(int cityId)
        {
            var games = this.games.All().Where(x => x.Location.CityId == cityId);
            return games;
        }

        public Game GetById(int id)
        {
            var game = this.games.GetById(id);
            return game;
        }

        public IQueryable<Game> GetBySport(int sportId)
        {
            var games = this.games.All().Where(g => g.SportId == sportId);
            return games;
        }

        public int GetCount()
        {
            var count = this.games.All().Count();
            return count;
        }

        public IQueryable<Game> GetUpcoming()
        {
            var games = this.games.All()
                .OrderBy(x => x.StartingDateTime)
                .Where(x => x.StartingDateTime > DateTime.UtcNow &&
                        x.Status == GameStatus.WaitingForPlayers);

            return games;
        }

        public void Save()
        {
            this.games.Save();
        }
    }
}
