﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Dtos
{
    public class NewRatingDto
    {
        [Required]
        [Range(0, 10)]
        public int Value { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public int MovieId { get; set; }
    }
}
