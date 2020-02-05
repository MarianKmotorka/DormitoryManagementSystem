using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.AccomodationRequests.Commands.CreateAccomodationRequest
{
    public class CreateAccomodationRequestCommandHandler : IRequestHandler<CreateAccomodationRequestCommand>
    {
        private readonly IDormitoryDbContext _db;

        public CreateAccomodationRequestCommandHandler(IDormitoryDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(CreateAccomodationRequestCommand request, CancellationToken cancellationToken)
        {
            var requestor = await _db.Guests.SingleOrNotFoundAsync(x => x.Id == request.RequestorId, cancellationToken);

            var accomodationRequest = new AccomodationRequest
            {
                AccomodationStartDateUtc = request.AccomodationStartDateUtc,
                AccomodationEndDateUtc = request.AccomodationEndDateUtc,
                Requester = requestor,
                RequesterMessage = request.RequestorMessage,
                RequestPlacedUtc = DateTime.UtcNow,
                State = AccomodationRequestState.Active
            };

            _db.AccomodationRequests.Add(accomodationRequest);
            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
