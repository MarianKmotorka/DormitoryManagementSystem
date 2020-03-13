using Application.Common.Pagination;
using MediatR;
using Sieve.Models;

namespace Application.AppUsers.Queries.GetAppUserList
{
    public class GetAppUserListQuery : IRequest<PagedResponse<AppUserLookup>>
    {
        public SieveModel PaginationModel { get; set; }
    }
}
