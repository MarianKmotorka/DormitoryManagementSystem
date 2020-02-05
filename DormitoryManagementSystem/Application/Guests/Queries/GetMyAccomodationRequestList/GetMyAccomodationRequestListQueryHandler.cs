using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.AccomodationRequests.Queries.GetAccomodationRequestList;
using Application.Common.Interfaces;
using Application.Common.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guests.Queries.GetMyAccomodationRequestList
{
    public class GetMyAccomodationRequestListQueryHandler : IRequestHandler<GetMyAccomodationRequestListQuery, PagedResponse<AccomodationRequestLookup>>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;

        public GetMyAccomodationRequestListQueryHandler(IDormitoryDbContext db, IMapper mapper, IPaginationService paginationService)
        {
            _db = db;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<AccomodationRequestLookup>> Handle(GetMyAccomodationRequestListQuery request, CancellationToken cancellationToken)
        {
            var requests = _db.AccomodationRequests.AsNoTracking()
                .Where(x => x.Requester.Id == request.GuestId)
                .ProjectTo<AccomodationRequestLookup>(_mapper.ConfigurationProvider);

            return await _paginationService.GetPagedAsync(requests, request.PaginationModel);
        }
    }
}
