using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Guests.Queries.GetGuestDetail
{
    public class GetGuestDetailQueryHandler : IRequestHandler<GetGuestDetailQuery, GuestDetail>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;

        public GetGuestDetailQueryHandler(IDormitoryDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GuestDetail> Handle(GetGuestDetailQuery request, CancellationToken cancellationToken)
        {
            var guest = await _db.Guests.AsNoTracking()
                .Where(x => x.AppUser.EmailConfirmed)
                .ProjectTo<GuestDetail>(_mapper.ConfigurationProvider)
                .SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            return guest;
        }
    }
}
