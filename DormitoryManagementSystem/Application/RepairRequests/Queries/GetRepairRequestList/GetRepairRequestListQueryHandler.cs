using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RepairRequests.Queries.GetRepairRequestList
{
    public class GetRepairRequestListQueryHandler : IRequestHandler<GetRepairRequestListQuery, PagedResponse<RepairRequestLookup>>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IPaginationService _paginationService;
        private readonly IMapper _mapper;

        public GetRepairRequestListQueryHandler(IDormitoryDbContext db, IPaginationService paginationService, IMapper mapper)
        {
            _db = db;
            _paginationService = paginationService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<RepairRequestLookup>> Handle(GetRepairRequestListQuery request, CancellationToken cancellationToken)
        {
            var query = _db.RepairRequests.AsNoTracking();

            if (!string.IsNullOrEmpty(request.GuestId))
                query = query.Where(x => x.Guest.Id == request.GuestId);

            return await _paginationService.GetPagedAsync(query.ProjectTo<RepairRequestLookup>(_mapper.ConfigurationProvider), request.PaginationModel);
        }
    }
}
