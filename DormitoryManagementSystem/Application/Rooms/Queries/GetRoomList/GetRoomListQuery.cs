using Application.Common.Pagination;
using MediatR;
using Sieve.Models;

namespace Application.Rooms.Queries.GetRoomList
{
    public class GetRoomListQuery : IRequest<PagedResponse<RoomLookup>>
    {
        public SieveModel PaginationModel { get; set; }
    }
}
