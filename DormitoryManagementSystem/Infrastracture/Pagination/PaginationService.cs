using Application.Common.Pagination;
using Sieve.Models;
using Sieve.Services;

namespace Infrastracture.Pagination
{
    public class PaginationService : SieveProcessor, IPaginationService
    {
        public PaginationService() : base(Microsoft.Extensions.Options.Options.Create(new SieveOptions()))
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            return mapper;
        }
    }
}
