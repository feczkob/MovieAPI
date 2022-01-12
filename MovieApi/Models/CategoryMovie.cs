using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Models
{
    [Table("CategoryMovie")]
    public class CategoryMovie
    {
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
