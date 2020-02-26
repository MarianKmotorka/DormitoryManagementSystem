using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.AccomodationRequests.Commands.RespondToAccomodationRequest
{
    public class RespondToAccomodationRequestCommandValidator : AbstractValidator<RespondToAccomodationRequestCommand>
    {
        private readonly IDormitoryDbContext _db;

        public RespondToAccomodationRequestCommandValidator(IDormitoryDbContext db) : this()
        {
            _db = db;
        }

        private RespondToAccomodationRequestCommandValidator()
        {
            RuleFor(x => x.RoomId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Required).When(x => x.IsAccomodationRequestApproved)
                .MustAsync(BeAvailable).WithMessage(ErrorMessages.RoomMustBeAvailable).When(x => x.IsAccomodationRequestApproved);
        }

        private async Task<bool> BeAvailable(int roomId, CancellationToken cancellationToken)
        {
            var room = await _db.Rooms.AsNoTracking().Include(x => x.Guests).SingleOrNotFoundAsync(x => x.Id == roomId, cancellationToken);
            return room.Capacity - room.Guests.Count > 0;
        }
    }
}
