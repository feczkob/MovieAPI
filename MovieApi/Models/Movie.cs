using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Models
{
    [Table("Movie")]
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(10, 43200)]
        public int Length { get; set; }
        [Range(1900, 2022)]
        public int Year { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<CategoryMovie> CategoryMovies { get; set; }
        //public virtual List<Category> Categories { get; set; }
    }
}
