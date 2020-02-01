using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.AppUsers.Commands.ChangeForgottenPassword
{
    public class ChangeForgottenPasswordCommandHandler : IRequestHandler<ChangeForgottenPasswordCommand>
    {
        private readonly IIdentityService _identityService;

        public ChangeForgottenPasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Unit> Handle(ChangeForgottenPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ChangeForgottenPasswordAsync(request.Email, request.ResetToken, request.NewPassword);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors);

            return Unit.Value;
        }
    }
}
