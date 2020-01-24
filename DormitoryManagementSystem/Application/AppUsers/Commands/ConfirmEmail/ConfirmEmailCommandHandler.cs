using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.AppUsers.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly IIdentityService _identityService;

        public ConfirmEmailCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var success = await _identityService.ConfirmEmailAsync(request.Email, request.Token);

            if (!success) throw new BadRequestException(ErrorMessages.Invalid);

            return Unit.Value;
        }
    }
}
//TODO zmen stringy v exceptins na ErrorMEssages.blablabla