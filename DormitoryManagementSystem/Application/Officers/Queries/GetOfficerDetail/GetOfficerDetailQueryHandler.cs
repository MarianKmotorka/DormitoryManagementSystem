using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Officers.Queries.GetOfficerDetail
{
    public class GetOfficerDetailQueryHandler : IRequestHandler<GetOfficerDetailQuery, OfficerDetail>
    {
        private readonly IMapper _mapper;
        private readonly IDormitoryDbContext _db;

        public GetOfficerDetailQueryHandler(IDormitoryDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OfficerDetail> Handle(GetOfficerDetailQuery request, CancellationToken cancellationToken)
        {
            var officer = await _db.Officers.AsNoTracking()
                .ProjectTo<OfficerDetail>(_mapper.ConfigurationProvider)
                .SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            return officer;
        }
    }
}
