using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Guests.Commands.EditGuest
{
    public class EditGuestCommandValidator : AbstractValidator<EditGuestCommand>
    {
        private readonly IDormitoryDbContext _db;

        public EditGuestCommandValidator(IDormitoryDbContext db) : this()
        {
            _db = db;
        }

        public EditGuestCommandValidator()
        {
            RuleFor(x => x.RoomNumber).Cascade(CascadeMode.StopOnFirstFailure)
                .MustAsync(BeNullOrExist).WithMessage(ErrorMessages.Invalid)
                .MustAsync(BeFree).WithMessage(ErrorMessages.RoomMustBeAvailable);

            RuleFor(x => x.DistanceFromHome).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Invalid)
                .GreaterThanOrEqualTo(0).WithMessage(ErrorMessages.Invalid);

            RuleFor(x => x.DormitoryCardNumber)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .When(x => !string.IsNullOrEmpty(x.RoomNumber));

            RuleFor(x => x.FirstName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.LastName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.Street).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.HouseNumber).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.City).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.Country).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PostCode).NotEmpty().WithMessage(ErrorMessages.Required);
        }

        private async Task<bool> BeFree(EditGuestCommand command, string roomNumber, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
                return true;

            var room = await _db.Rooms.Include(x => x.Guests).SingleAsync(x => x.Number == roomNumber, cancellationToken);

            if (room.Guests.Any(x => x.Id == command.Id))
                return true;

            var isRoomFree = room.Capacity - room.Guests.Count > 0;
            return isRoomFree;
        }

        private async Task<bool> BeNullOrExist(string roomNumber, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(roomNumber))
                return true;

            var roomExists = await _db.Rooms.AnyAsync(x => x.Number == roomNumber, cancellationToken);
            return roomExists;
        }
    }
}
