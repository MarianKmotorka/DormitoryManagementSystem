using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Officers.Commands.RespondToAccomodationRequest
{
    public class RespondToAccomodationRequestCommandHandler : IRequestHandler<RespondToAccomodationRequestCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IEmailService _emailService;

        public RespondToAccomodationRequestCommandHandler(IDormitoryDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(RespondToAccomodationRequestCommand request, CancellationToken cancellationToken)
        {
            var accomodationRequest = await _db.AccomodationRequests
                .Include(x => x.Requester)
                .Include(x => x.Requester.Room)
                .Include(x => x.Requester.AppUser)
                .SingleOrNotFoundAsync(x => x.Id == request.AccomodationRequestId, cancellationToken);

            accomodationRequest.State = request.IsAccomodationRequestApproved ? AccomodationRequestState.Approved : AccomodationRequestState.Refused;

            if (request.IsAccomodationRequestApproved)
                accomodationRequest.Requester.Room = await _db.Rooms.SingleOrNotFoundAsync(x => x.Id == request.RoomId, cancellationToken);
            else
                accomodationRequest.Requester.Room = null;

            await _db.SaveChangesAsync(cancellationToken);

            var emailMessage = @$"<p>Your accomodation request was {accomodationRequest.State.ToString().ToLower()}.</p>
                                  {(request.IsAccomodationRequestApproved ? $"<p> Your room nuber is {accomodationRequest.Requester.Room.Number}.</p>" : "")}
                                  {(!string.IsNullOrWhiteSpace(request.AdditionalMessage) ? $"<p>{request.AdditionalMessage}</p>" : "")}";

            await _emailService.SendAsync(emailMessage, accomodationRequest.Requester.AppUser.Email, "Accomodation request response");

            return Unit.Value;
        }
    }
}
