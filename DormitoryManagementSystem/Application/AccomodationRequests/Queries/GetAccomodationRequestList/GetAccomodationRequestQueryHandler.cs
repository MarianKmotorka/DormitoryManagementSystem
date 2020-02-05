using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AccomodationRequests.Queries.GetAccomodationRequestList
{
    public class GetAccomodationRequestQueryHandler : IRequestHandler<GetAccomodationRequestListQuery, PagedResponse<AccomodationRequestLookup>>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IPaginationService _paginationService;
        private readonly IMapper _mapper;

        public GetAccomodationRequestQueryHandler(IDormitoryDbContext db, IPaginationService paginationService, IMapper mapper)
        {
            _db = db;
            _paginationService = paginationService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<AccomodationRequestLookup>> Handle(GetAccomodationRequestListQuery request, CancellationToken cancellationToken)
        {
            var requests = _db.AccomodationRequests.AsNoTracking().ProjectTo<AccomodationRequestLookup>(_mapper.ConfigurationProvider);
            return await _paginationService.GetPagedAsync(requests, request.PaginationModel);
        }
    }
}
