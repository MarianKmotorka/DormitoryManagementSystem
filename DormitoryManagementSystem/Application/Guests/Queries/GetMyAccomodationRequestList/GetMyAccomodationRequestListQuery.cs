using Application.AccomodationRequests.Queries.GetAccomodationRequestList;
using Application.Common.Pagination;
using MediatR;
using Sieve.Models;

namespace Application.Guests.Queries.GetMyAccomodationRequestList
{
    public class GetMyAccomodationRequestListQuery : IRequest<PagedResponse<AccomodationRequestLookup>>
    {
        public string GuestId { get; set; }
        public SieveModel PaginationModel { get; set; }
    }
}
