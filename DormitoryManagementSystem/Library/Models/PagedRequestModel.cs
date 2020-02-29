using System.Collections.Generic;

namespace Library.Models
{
    public class PagedRequestModel
    {
        public int PageSize { get; set; } = 10;

        public int PageNumber { get; set; } = 1;

        public IEnumerable<string> Filters { get; set; }

        public IEnumerable<string> Sorts { get; set; }

        public PagedRequestModel()
        {
            Filters = new List<string>();
            Sorts = new List<string>();
        }
    }
}
