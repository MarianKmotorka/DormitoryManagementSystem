using Application.Common.Models;
using FluentValidation;

namespace Application.Guests.Commands.CreateGuest
{
    public class CommandValidator : AbstractValidator<CreateGuestCommand>
    {
        public CommandValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.DistanceFromHome)
                .NotEmpty().WithMessage(ErrorMessages.Invalid)
                .Must(x => x >= 0).WithMessage(ErrorMessages.Invalid);
        }
    }
}
