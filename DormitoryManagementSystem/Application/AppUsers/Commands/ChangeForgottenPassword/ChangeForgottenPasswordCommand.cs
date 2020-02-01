using MediatR;

namespace Application.AppUsers.Commands.ChangeForgottenPassword
{
    public class ChangeForgottenPasswordCommand : IRequest
    {
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }
}
