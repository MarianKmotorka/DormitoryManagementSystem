using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Offices.Queries.GetOfficeList
{
    public class GetOfficeListQueryHandler : IRequestHandler<GetOfficeListQuery, PagedResponse<OfficeLookup>>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;

        public GetOfficeListQueryHandler(IDormitoryDbContext db, IMapper mapper, IPaginationService paginationService)
        {
            _db = db;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<OfficeLookup>> Handle(GetOfficeListQuery request, CancellationToken cancellationToken)
        {
            var offices = _db.Offices.AsNoTracking().ProjectTo<OfficeLookup>(_mapper.ConfigurationProvider);
            return await _paginationService.GetPagedAsync(offices, request.PaginationModel);
        }
    }
}
