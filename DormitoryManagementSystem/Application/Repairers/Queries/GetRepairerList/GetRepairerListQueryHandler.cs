using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Repairers.Queries.GetRepairerList
{
    public class GetRepairerListQueryHandler : IRequestHandler<GetRepairerListQuery, PagedResponse<RepairerLookup>>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;

        public GetRepairerListQueryHandler(IDormitoryDbContext db, IMapper mapper, IPaginationService paginationService)
        {
            _db = db;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<RepairerLookup>> Handle(GetRepairerListQuery request, CancellationToken cancellationToken)
        {
            var repairers = _db.Repairers.AsNoTracking()
                .Where(x => x.AppUser.EmailConfirmed)
                .ProjectTo<RepairerLookup>(_mapper.ConfigurationProvider);

            return await _paginationService.GetPagedAsync(repairers, request.PaginationModel);
        }
    }
}
