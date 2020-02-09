using Application.Common.Pagination;
using MediatR;
using Sieve.Models;

namespace Application.RepairRequests.Queries.GetRepairRequestList
{
    public class GetRepairRequestListQuery : IRequest<PagedResponse<RepairRequestLookup>>
    {
        public string GuestId { get; set; }

        public SieveModel PaginationModel { get; set; }
    }
}
