using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Officers.Commands.EditOfficer
{
    public class EditOfficerCommandValidator : AbstractValidator<EditOfficerCommand>
    {
        private readonly IDormitoryDbContext _db;

        public EditOfficerCommandValidator(IDormitoryDbContext db) : this()
        {
            _db = db;
        }

        public EditOfficerCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.LastName).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.OfficeNumber).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .MustAsync(OfficeExists).WithMessage(ErrorMessages.Invalid)
                .MustAsync(BeAvailable).WithMessage(ErrorMessages.OfficeMustBeAvailable);

            RuleFor(x => x.Street).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.HouseNumber).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.City).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.Country).NotEmpty().WithMessage(ErrorMessages.Required);

            RuleFor(x => x.PostCode).NotEmpty().WithMessage(ErrorMessages.Required);
        }
        private async Task<bool> BeAvailable(string officeNumber, CancellationToken cancellationToken)
        {
            var office = await _db.Offices.SingleOrNotFoundAsync(x => x.Number == officeNumber, cancellationToken);
            return office.Capacity - office.Officers.Count > 0;
        }

        private async Task<bool> OfficeExists(string officeNumber, CancellationToken cancellationToken)
        {
            var exists = await _db.Offices.AnyAsync(x => x.Number == officeNumber, cancellationToken);
            return exists;
        }
    }
}

