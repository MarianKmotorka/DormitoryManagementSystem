using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Guests.Commands.CreateGuest
{
    public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
    {
        public CreateGuestCommandValidator(IDormitoryDbContext db)
        {
            RuleFor(x => x.DistanceFromHome).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Invalid)
                .GreaterThanOrEqualTo(0).WithMessage(ErrorMessages.Invalid);

            RuleFor(x => x.FirstName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.LastName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .Matches(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$").WithMessage(ErrorMessages.Invalid);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .MinimumLength(6).WithMessage(ErrorMessages.MinLength(6));

            RuleFor(x => x.Street).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.HouseNumber).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.City).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.Country).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PostCode).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .EmailAddress().WithMessage(ErrorMessages.Invalid)
                .MustAsync(async (email, token) => !await db.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower(), token)).WithMessage(ErrorMessages.EmailNotUnique);

            RuleFor(x => x.IdCardNumber).NotEmpty().WithMessage(ErrorMessages.Required);
        }
    }
}
