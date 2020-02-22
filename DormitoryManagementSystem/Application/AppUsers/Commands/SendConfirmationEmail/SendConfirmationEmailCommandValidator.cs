using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.AppUsers.Commands.SendConfirmationEmail
{
    public class SendConfirmationEmailCommandValidator : AbstractValidator<SendConfirmationEmailCommand>
    {
        private readonly IDormitoryDbContext _db;

        public SendConfirmationEmailCommandValidator(IDormitoryDbContext db)
        {
            _db = db;

            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .EmailAddress().WithMessage(ErrorMessages.InvalidEmail)
                .MustAsync(Exist).WithMessage(ErrorMessages.EmailNotFound)
                .MustAsync(NotBeConfirmed).WithMessage(ErrorMessages.AlreadyConfirmed);
        }

        private async Task<bool> Exist(string email, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(x => x.Email == email);
        }

        private async Task<bool> NotBeConfirmed(string email, CancellationToken cancellationToken)
        {
            var appUser = await _db.Users.SingleAsync(x => x.Email == email);
            return !appUser.EmailConfirmed;
        }
    }
}
