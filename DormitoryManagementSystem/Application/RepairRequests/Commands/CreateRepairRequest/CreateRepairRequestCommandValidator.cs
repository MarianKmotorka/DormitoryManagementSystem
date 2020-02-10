using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.RepairRequests.Commands.CreateRepairRequest
{
    public class CreateRepairRequestCommandValidator : AbstractValidator<CreateRepairRequestCommand>
    {
        private readonly IDormitoryDbContext _db;

        public CreateRepairRequestCommandValidator(IDormitoryDbContext db) : this()
        {
            _db = db;
        }

        private CreateRepairRequestCommandValidator()
        {
            RuleFor(x => x.RoomItemTypeId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .MustAsync(BeValidRoomItemType).WithMessage(ErrorMessages.Invalid)
                .MustAsync(RepairIsNotAlreadyRequested).WithMessage(ErrorMessages.RepairAlreadyRequested);

            RuleFor(x => x.ProblemDescription).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .MinimumLength(5).WithMessage(ErrorMessages.MinLength(5));
        }

        private async Task<bool> RepairIsNotAlreadyRequested(int roomItemTypeId, CancellationToken cancellationToken)
        {
            return !await _db.RepairRequests.AsNoTracking().AnyAsync(x => x.RoomItemType.Id == roomItemTypeId && x.State != Domain.Enums.RepairRequestState.Fixed);
        }

        private async Task<bool> BeValidRoomItemType(CreateRepairRequestCommand command, int roomItemTypeId, CancellationToken cancellationToken)
        {
            var roomItemType = await _db.RoomItemTypes.Include(x => x.Room.Guests).SingleOrDefaultAsync(x => x.Id == roomItemTypeId);

            if (roomItemType == null)
                return false;

            return roomItemType.Room.Guests.Any(x => x.Id == command.GuestId);
        }
    }
}
