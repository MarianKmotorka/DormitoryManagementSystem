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
            var requester = await _db.Guests.SingleOrNotFoundAsync(x => x.Id == request.RequesterId, cancellationToken);

            var accomodationRequest = new AccomodationRequest
            {
                AccomodationStartDateUtc = request.AccomodationStartDateUtc,
                AccomodationEndDateUtc = request.AccomodationEndDateUtc,
                Requester = requester,
                RequesterMessage = request.RequesterMessage,
                RequestPlacedUtc = DateTime.UtcNow.Date,
                State = AccomodationRequestState.Active
            };

            _db.AccomodationRequests.Add(accomodationRequest);
            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
