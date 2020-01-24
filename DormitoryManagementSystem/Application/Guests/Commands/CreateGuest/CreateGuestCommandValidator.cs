using Application.Common.Models;
using FluentValidation;

namespace Application.Guests.Commands.CreateGuest
{
    public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
    {
        public CreateGuestCommandValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.DistanceFromHome)
                .NotEmpty().WithMessage(ErrorMessages.Invalid)
                .Must(x => x >= 0).WithMessage(ErrorMessages.Invalid);
        }
    }
}
