using Application.Common.Pagination;
using MediatR;
using Sieve.Models;

namespace Application.Guests.Queries.GetGuestList
{
    public class GetGuestListQuery : IRequest<PagedResponse<GuestLookup>>
    {
        public SieveModel PaginationModel { get; set; }
    }
}
