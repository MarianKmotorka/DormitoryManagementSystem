using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;

namespace Application.RepairRequests.Commands.RespondToRepairRequest
{
    public class RespondToRepairRequestCommandHandler : IRequestHandler<RespondToRepairRequestCommand>
    {
        private readonly IDormitoryDbContext _db;

        public RespondToRepairRequestCommandHandler(IDormitoryDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(RespondToRepairRequestCommand request, CancellationToken cancellationToken)
        {
            var repairRequest = await _db.RepairRequests.SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            if (repairRequest.State == RepairRequestState.Fixed)
                throw new BadRequestException(ErrorMessages.CannotModifyFixed);

            if (request.RepairRequestState == RepairRequestState.Fixed)
            {
                var repairer = await _db.Repairers.SingleOrNotFoundAsync(x => x.Id == request.FixedById, cancellationToken);
                repairRequest.FixedOn = DateTime.UtcNow;
                repairRequest.FixedBy = repairer;
            }

            if (request.RepairRequestState == RepairRequestState.Accepted)
                repairRequest.WillBeFixedOn = request.WillBeFixedOn;

            repairRequest.RepairerReply = request.RepairerReply;
            repairRequest.State = request.RepairRequestState;

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
