namespace InstaSport.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using InstaSport.Data.Common.Models;

    public class Location : BaseModel<int>
    {
        public Location()
        {
            this.AvailableSports = new HashSet<Sport>();
            this.Ratings = new HashSet<Rating>();
        }

        public string Name { get; set; }

        public string? NameTranslations { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Longitude { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<Sport> AvailableSports { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
