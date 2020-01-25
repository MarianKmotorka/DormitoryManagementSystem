using MediatR;

namespace Application.AppUsers.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public string ExpiredJwt { get; set; }
        public string RefreshToken { get; set; }
    }
}
