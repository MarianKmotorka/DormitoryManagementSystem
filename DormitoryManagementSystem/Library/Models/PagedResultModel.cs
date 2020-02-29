using System.Collections.Generic;

namespace Library.Models
{
    public class PagedResultModel<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int Pages { get; set; }
    }
}
