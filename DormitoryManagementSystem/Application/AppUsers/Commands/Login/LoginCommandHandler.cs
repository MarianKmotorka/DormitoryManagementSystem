using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AppUsers.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IDormitoryDbContext _db;

        public LoginCommandHandler(IIdentityService identityService, IDormitoryDbContext db)
        {
            _identityService = identityService;
            _db = db;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var (result, jwt, refreshToken) = await _identityService.LoginUserAsync(request.Email, request.Password);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors);

            var (_, role) = await _identityService.GetRoleAsync(request.Email);

            var user = await _db.Users.SingleAsync(x => x.Email == request.Email, cancellationToken);

            return new LoginResponse
            {
                Jwt = jwt,
                RefreshToken = refreshToken,
                Role = role,
                UserName = $"{user.FirstName} {user.LastName}"
            };
        }
    }
}
