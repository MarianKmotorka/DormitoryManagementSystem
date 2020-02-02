using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
                .Include(x => x.AppUser)
                .ProjectTo<GuestDetail>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (guest == null)
                throw new NotFoundException(nameof(Guest), request.Id);

            return guest;
        }
    }
}
