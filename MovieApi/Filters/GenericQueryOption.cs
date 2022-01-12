using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Filters
{
    public class GenericQueryOption<TFilter>
    {
        public TFilter Filter { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SortOrder SortOrder { get; set; } = SortOrder.Descending;
        //public string SortFieldName { get; set; }
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }
}
