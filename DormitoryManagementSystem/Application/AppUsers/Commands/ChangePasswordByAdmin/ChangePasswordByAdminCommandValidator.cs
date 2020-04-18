using System.Linq;
using Application.Common.Models;
using FluentValidation;

namespace Application.AppUsers.Commands.ChangePasswordByAdmin
{
    public class ChangePasswordByAdminCommandValidator : AbstractValidator<ChangePasswordByAdminCommand>
    {
        public ChangePasswordByAdminCommandValidator()
        {
            RuleFor(x => x.NewPassword).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .MinimumLength(6).WithMessage(ErrorMessages.MinLength).WithState(_ => 6)
                .MaximumLength(30).WithMessage(ErrorMessages.MinLength).WithState(_ => 30)
                .Must(x => x.ToList().Any(char.IsLetter)).WithMessage(ErrorMessages.MustContainLetter);
        }
    }
}
