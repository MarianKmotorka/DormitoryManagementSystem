using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.AppUsers.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public ChangePasswordCommandValidator(UserManager<AppUser> userManager, ICurrentUserService currentUserService) : this()
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public ChangePasswordCommandValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .MustAsync(BeValid).WithMessage(ErrorMessages.Invalid);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(ErrorMessages.Required)
                .MinimumLength(6).WithMessage(ErrorMessages.MinLength(6));
        }

        private async Task<bool> BeValid(ChangePasswordCommand command, string currentPassword, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.FindByIdAsync(_currentUserService.UserId);

            return await _userManager.CheckPasswordAsync(appUser, currentPassword);
        }
    }
}
