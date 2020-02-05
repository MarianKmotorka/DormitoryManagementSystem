using Application.Common.Pagination;
using MediatR;
using Sieve.Models;

namespace Application.AccomodationRequests.Queries.GetAccomodationRequestList
{
    public class GetAccomodationRequestListQuery : IRequest<PagedResponse<AccomodationRequestLookup>>
    {
        public SieveModel PaginationModel { get; set; }
    }
}
