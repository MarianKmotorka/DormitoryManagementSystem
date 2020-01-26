using MediatR;

namespace Application.AppUsers.Commands.SendChangePasswordEmail
{
    public class SendChangePasswordEmailCommand : IRequest
    {
        public string AdditionalMessage { get; set; }
        public string Email { get; set; }
    }
}
