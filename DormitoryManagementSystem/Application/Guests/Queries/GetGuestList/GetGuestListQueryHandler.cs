using Application.Common.Interfaces;
using Application.Common.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Guests.Queries.GetGuestList
{
    public class GetGuestListQueryHandler : IRequestHandler<GetGuestListQuery, PagedResponse<GuestLookup>>
    {
        private readonly IPaginationService _paginationService;
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;

        public GetGuestListQueryHandler(IPaginationService paginationService, IDormitoryDbContext db, IMapper mapper)
        {
            _paginationService = paginationService;
            _db = db;
            _mapper = mapper;
        }

        public async Task<PagedResponse<GuestLookup>> Handle(GetGuestListQuery request, CancellationToken cancellationToken)
        {
            var guests = _db.Guests.AsNoTracking()
                .Where(x => x.AppUser.EmailConfirmed)
                .ProjectTo<GuestLookup>(_mapper.ConfigurationProvider);

            return await _paginationService.GetPagedAsync(guests, request.PaginationModel);
        }
    }
}
