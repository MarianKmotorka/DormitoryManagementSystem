using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;

namespace Application.Repairers.Commands.EditRepairer
{
    public class EditRepairerCommandValidator : AbstractValidator<EditRepairerCommand>
    {
        public EditRepairerCommandValidator(IDormitoryDbContext dormitoryDbContext)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.LastName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .Matches(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$").WithMessage(ErrorMessages.Invalid);

            RuleFor(x => x.Street).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.HouseNumber).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.City).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.Country).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PostCode).NotEmpty().WithMessage(ErrorMessages.Required);
        }
    }
}
