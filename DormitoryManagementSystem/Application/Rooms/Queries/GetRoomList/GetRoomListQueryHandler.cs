using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rooms.Queries.GetRoomList
{
    public class GetRoomListQueryHandler : IRequestHandler<GetRoomListQuery, PagedResponse<RoomLookup>>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;

        public GetRoomListQueryHandler(IDormitoryDbContext db, IMapper mapper, IPaginationService paginationService)
        {
            _db = db;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<RoomLookup>> Handle(GetRoomListQuery request, CancellationToken cancellationToken)
        {
            var rooms = _db.Rooms.AsNoTracking().ProjectTo<RoomLookup>(_mapper.ConfigurationProvider);
            return await _paginationService.GetPagedAsync(rooms, request.PaginationModel);
        }
    }
}
