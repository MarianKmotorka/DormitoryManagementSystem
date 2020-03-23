using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Repairers.Queries.GetRepairerDetail
{
    public class GetRepairerDetailQueryHandler : IRequestHandler<GetRepairerDetailQuery, RepairerDetail>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;

        public GetRepairerDetailQueryHandler(IDormitoryDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<RepairerDetail> Handle(GetRepairerDetailQuery request, CancellationToken cancellationToken)
        {
            var repairer = await _db.Repairers.AsNoTracking()
                .ProjectTo<RepairerDetail>(_mapper.ConfigurationProvider)
                .SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            return repairer;
        }
    }
}
