namespace InstaSport.Data.Models
{
    using InstaSport.Data.Common.Models;
    using System.Collections.Generic;

    public class User : BaseModel<int>
    {
        public User()
        {
            this.Ratings = new HashSet<Rating>();
            this.AuthoredRatings = new HashSet<Rating>();
            this.Games = new HashSet<Game>();
            this.FavouriteSports = new HashSet<Sport>();
        }

        public string UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? AvatarUrl { get; set; }

        public string? FacebookUrl { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime DateJoined { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<Rating> AuthoredRatings { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<Sport> FavouriteSports { get; set; }
    }
}
