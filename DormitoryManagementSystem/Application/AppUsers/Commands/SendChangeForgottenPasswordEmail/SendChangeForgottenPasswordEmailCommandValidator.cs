using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.AppUsers.Commands.SendChangeForgottenPasswordEmail
{
    public class SendChangeForgottenPasswordEmailCommandValidator : AbstractValidator<SendChangeForgottenPasswordEmailCommand>
    {
        private readonly IDormitoryDbContext _db;

        public SendChangeForgottenPasswordEmailCommandValidator(IDormitoryDbContext db)
        {
            _db = db;

            RuleFor(x => x.Email).MustAsync(UserMustExist).WithMessage(ErrorMessages.EmailNotFound);
        }

        private async Task<bool> UserMustExist(string email, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(x => x.Email == email);
        }
    }
}
