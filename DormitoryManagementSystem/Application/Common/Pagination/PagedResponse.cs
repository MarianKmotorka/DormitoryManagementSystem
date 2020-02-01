using System.Collections.Generic;

namespace Application.Common.Pagination
{
    public class PagedResponse<T> where T : class
    {
        public PagedResponse() { }

        public PagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public IEnumerable<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Pages { get; set; }
        public long TotalRecords { get; set; }
    }
}
