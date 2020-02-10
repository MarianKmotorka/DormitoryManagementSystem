using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RepairRequests.Queries.GetRepairRequestDetail
{
    public class GetRepairRequestDetailQueryHandler : IRequestHandler<GetRepairRequestDetailQuery, RepairRequestDetail>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;

        public GetRepairRequestDetailQueryHandler(IDormitoryDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<RepairRequestDetail> Handle(GetRepairRequestDetailQuery request, CancellationToken cancellationToken)
        {
            var query = _db.RepairRequests.AsNoTracking();

            if (!string.IsNullOrEmpty(request.GuestId))
                query = query.Where(x => x.Guest.Id == request.GuestId);

            return await query
                .ProjectTo<RepairRequestDetail>(_mapper.ConfigurationProvider)
                .SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}
