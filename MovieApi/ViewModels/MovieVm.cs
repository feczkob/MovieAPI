﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.ViewModels
{
    public class MovieVm
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
        [Range(0,10)]
        public double RatingAvg { get; set; }
        public List<RatingVm> Ratings { get; set; }
        public virtual List<CategoryVm> Categories { get; set; }
    }
}
