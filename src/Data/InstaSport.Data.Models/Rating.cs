namespace InstaSport.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using InstaSport.Data.Common.Models;

    public class Rating : BaseModel<int>
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Range(0, 10)]
        public int Value { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
    }
}
