using MediatR;
using Newtonsoft.Json;

namespace Application.AppUsers.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }

        public string NewPassword { get; set; }

        public string CurrentPassword { get; set; }
    }
}
