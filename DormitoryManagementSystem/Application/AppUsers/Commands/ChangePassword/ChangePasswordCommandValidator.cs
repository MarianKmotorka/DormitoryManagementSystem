using Application.Common.Models;
using FluentValidation;

namespace Application.AppUsers.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .EmailAddress().WithMessage(ErrorMessages.Invalid);

            RuleFor(x => x.NewPassword).MinimumLength(6).WithMessage(ErrorMessages.MinLength(6));
        }
    }
}
