using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.AppUsers.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var (result, jwt, refreshToken) = await _identityService.LoginUserAsync(request.Email, request.Password);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors);

            return new LoginResponse
            {
                Jwt = jwt,
                RefreshToken = refreshToken
            };
        }
    }
}
