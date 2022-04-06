using Telerik.Windows.Controls.Map;

namespace InstaSport.WPF.Models
{
    public class MapItem
    {
        private Location location;
        private string name;

        public MapItem(InstaSport.Data.Models.Location location)
        {
            this.location = new Location((double)location.Latitude, (double)location.Longitude);
            this.name = location.Name;
        }

        public Location Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public override bool Equals(object? obj)
        {
            var other = obj as MapItem;
            return other != null && this.Location.Equals(other.Location);
        }
    }
}
