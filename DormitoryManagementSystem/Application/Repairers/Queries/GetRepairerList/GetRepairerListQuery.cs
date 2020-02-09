using Application.Common.Pagination;
using MediatR;
using Sieve.Models;

namespace Application.Repairers.Queries.GetRepairerList
{
    public class GetRepairerListQuery : IRequest<PagedResponse<RepairerLookup>>
    {
        public SieveModel PaginationModel { get; set; }
    }
}
