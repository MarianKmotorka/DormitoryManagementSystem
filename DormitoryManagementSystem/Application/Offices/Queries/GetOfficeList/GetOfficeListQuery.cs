using Application.Common.Pagination;
using MediatR;
using Sieve.Models;

namespace Application.Offices.Queries.GetOfficeList
{
    public class GetOfficeListQuery : IRequest<PagedResponse<OfficeLookup>>
    {
        public SieveModel PaginationModel { get; set; }
    }
}
