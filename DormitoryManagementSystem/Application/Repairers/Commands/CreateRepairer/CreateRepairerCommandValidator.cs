using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Repairers.Commands.CreateRepairer
{
    public class CreateRepairerCommandValidator : AbstractValidator<CreateRepairerCommand>
    {
        public CreateRepairerCommandValidator(IDormitoryDbContext db)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.LastName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.Street).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.HouseNumber).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.City).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.Country).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PostCode).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .EmailAddress().WithMessage(ErrorMessages.Invalid)
                .MustAsync(async (email, token) => !await db.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower(), token)).WithMessage(ErrorMessages.EmailNotUnique);
        }
    }
}
