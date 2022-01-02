namespace InstaSport.Services.Data
{
    using System;
    using System.Linq;

    using InstaSport.Data.Common;
    using InstaSport.Data.Models;

    public class SportsService : ISportsService
    {
        private readonly IDbRepository<Sport> sports;

        public SportsService(IDbRepository<Sport> sports)
        {
            this.sports = sports;
        }

        public IQueryable<Sport> GetAll()
        {
            return this.sports.All().OrderBy(x => x.Name);
        }

        public Sport GetById(int id)
        {
            return this.sports.GetById(id);
        }

        public int GetCount()
        {
            var count = this.sports.All().Count();
            return count;
        }
    }
}
