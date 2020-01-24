using MediatR;

namespace Application.AppUsers.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
