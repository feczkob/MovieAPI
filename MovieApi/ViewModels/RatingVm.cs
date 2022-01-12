using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.ViewModels
{
    public class RatingVm
    {
        public int Id { get; set; }
        [Required]
        [Range(0, 10)]
        public int Value { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public int MovieId { get; set; }
    }
}
