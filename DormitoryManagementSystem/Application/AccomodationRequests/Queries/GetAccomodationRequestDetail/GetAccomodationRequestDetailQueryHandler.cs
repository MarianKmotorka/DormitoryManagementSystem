using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AccomodationRequests.Queries.GetAccomodationRequestDetail
{
    public class GetAccomodationRequestDetailQueryHandler : IRequestHandler<GetAccomodationRequestDetailQuery, AccomodationRequestDetail>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IMapper _mapper;

        public GetAccomodationRequestDetailQueryHandler(IDormitoryDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<AccomodationRequestDetail> Handle(GetAccomodationRequestDetailQuery request, CancellationToken cancellationToken)
        {
            var accomodationRequest = await _db.AccomodationRequests.AsNoTracking()
                .ProjectTo<AccomodationRequestDetail>(_mapper.ConfigurationProvider)
                .SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            return accomodationRequest;
        }
    }
}
