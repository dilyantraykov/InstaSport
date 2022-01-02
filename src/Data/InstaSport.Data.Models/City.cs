namespace InstaSport.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using InstaSport.Data.Common.Models;

    public class City : BaseModel<int>
    {
        [MinLength(3)]
        public string Name { get; set; }
    }
}
