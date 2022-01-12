using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Models
{
    [Table("Rating")]
    public class Rating
    {
        public int Id { get; set; }
        [Required]
        [Range(0, 10)]
        public int Value { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
