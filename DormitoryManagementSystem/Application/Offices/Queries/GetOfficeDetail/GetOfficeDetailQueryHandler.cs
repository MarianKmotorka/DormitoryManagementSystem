using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Offices.Queries.GetOfficeDetail
{
    public class GetOfficeDetailQueryHandler : IRequestHandler<GetOfficeDetailQuery, OfficeDetail>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;

        public GetOfficeDetailQueryHandler(IDormitoryDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OfficeDetail> Handle(GetOfficeDetailQuery request, CancellationToken cancellationToken)
        {
            return await _db.Offices
                .AsNoTracking()
                .ProjectTo<OfficeDetail>(_mapper.ConfigurationProvider)
                .SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}
