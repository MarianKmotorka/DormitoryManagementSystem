using MediatR;

namespace Application.AppUsers.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest
    {
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
    }
}
