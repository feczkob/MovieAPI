using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.ViewModels
{
    public class MovieRowVm
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Range(10, 43200)]
        public int Length { get; set; }
        [Range(1900, 2022)]
        public int Year { get; set; }
        public double RatingAvg { get; set; }
    }
}
