using Application.Common.Interfaces;
using Application.Common.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Officers.Queries.GetOfficerList
{
    public class GetOfficerListQueryHandler : IRequestHandler<GetOfficerListQuery, PagedResponse<OfficerLookup>>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;

        public GetOfficerListQueryHandler(IDormitoryDbContext db, IMapper mapper, IPaginationService paginationService)
        {
            _db = db;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<OfficerLookup>> Handle(GetOfficerListQuery request, CancellationToken cancellationToken)
        {
            var officers = _db.Officers.AsNoTracking()
                .Where(x => x.AppUser.EmailConfirmed)
                .ProjectTo<OfficerLookup>(_mapper.ConfigurationProvider);

            return await _paginationService.GetPagedAsync(officers, request.PaginationModel);
        }
    }
}
