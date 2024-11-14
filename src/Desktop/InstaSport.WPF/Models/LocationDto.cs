namespace InstaSport.WPF.Models
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
