using Application.Common.Pagination;
using MediatR;
using Sieve.Models;

namespace Application.Officers.Queries.GetOfficerList
{
    public class GetOfficerListQuery : IRequest<PagedResponse<OfficerLookup>>
    {
        public SieveModel PaginationModel { get; set; }
    }
}
