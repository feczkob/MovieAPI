using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Filters
{
    public class MovieFilter
    {
        public string NameTerm { get; set; }
        public int? MinRating { get; set; }
        //public string? CategoryTerm { get; set; }
    }
}
