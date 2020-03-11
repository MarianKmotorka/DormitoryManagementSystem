using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.RepairRequests.Commands.CreateRepairRequest
{
    public class CreateRepairRequestCommandHandler : IRequestHandler<CreateRepairRequestCommand>
    {
        private readonly IDormitoryDbContext _db;

        public CreateRepairRequestCommandHandler(IDormitoryDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(CreateRepairRequestCommand request, CancellationToken cancellationToken)
        {
            var guest = await _db.Guests.SingleOrNotFoundAsync(x => x.Id == request.GuestId, cancellationToken);
            var roomItemType = await _db.RoomItemTypes.SingleOrNotFoundAsync(x => x.Id == request.RoomItemTypeId, cancellationToken);

            var repairRequest = new RepairRequest
            {
                Guest = guest,
                CreatedOn = DateTime.UtcNow.Date,
                ProblemDesciption = request.ProblemDescription,
                RoomItemType = roomItemType,
                State = RepairRequestState.Pending
            };

            _db.RepairRequests.Add(repairRequest);
            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
