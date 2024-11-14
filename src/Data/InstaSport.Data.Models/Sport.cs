namespace InstaSport.Data.Models
{
    using System.Collections.Generic;
    using InstaSport.Data.Common.Models;

    public class Sport : BaseModel<int>
    {
        public Sport()
        {
            this.Players = new HashSet<User>();
            this.Locations = new HashSet<Location>();
        }

        public string Name { get; set; }

        public string? NameTranslations { get; set; }

        public virtual ICollection<User> Players { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}
