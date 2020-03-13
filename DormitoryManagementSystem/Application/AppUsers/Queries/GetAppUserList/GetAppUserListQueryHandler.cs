using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AppUsers.Queries.GetAppUserList
{
    public class GetAppUserListQueryHandler : IRequestHandler<GetAppUserListQuery, PagedResponse<AppUserLookup>>
    {
        private readonly IPaginationService _paginationService;
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;

        public GetAppUserListQueryHandler(IPaginationService paginationService, IDormitoryDbContext db, IMapper mapper)
        {
            _paginationService = paginationService;
            _db = db;
            _mapper = mapper;
        }

        public async Task<PagedResponse<AppUserLookup>> Handle(GetAppUserListQuery request, CancellationToken cancellationToken)
        {
            var users = _db.Users.AsNoTracking().ProjectTo<AppUserLookup>(_mapper.ConfigurationProvider);

            return await _paginationService.GetPagedAsync(users, request.PaginationModel);
        }
    }
}
