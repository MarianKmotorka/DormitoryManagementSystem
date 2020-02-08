using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rooms.Queries.GetRoomDetail
{
    public class GetRoomDetailQueryHandler : IRequestHandler<GetRoomDetailQuery, RoomDetail>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;

        public GetRoomDetailQueryHandler(IDormitoryDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<RoomDetail> Handle(GetRoomDetailQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Rooms.AsNoTracking().ProjectTo<RoomDetail>(_mapper.ConfigurationProvider);

            var room = new RoomDetail();
            if (request.Id != null)
                room = await query.SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);
            else
                room = await query.SingleOrNotFoundAsync(x => x.Guests.Any(g => g.Id == request.GuestId), cancellationToken);

            return room;
        }
    }
}
