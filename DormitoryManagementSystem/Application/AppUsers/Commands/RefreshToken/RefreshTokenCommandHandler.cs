using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.AppUsers.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var (result, jwt, refreshToken) = await _identityService.RefreshAsync(request.ExpiredJwt, request.RefreshToken);

            if (!result.Succeeded) throw new BadRequestException(result.Errors);

            return new RefreshTokenResponse
            {
                RefreshToken = refreshToken,
                Jwt = jwt
            };
        }
    }
}
