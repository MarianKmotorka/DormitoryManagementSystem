using MediatR;
using Newtonsoft.Json;

namespace Application.AppUsers.Commands.SendConfirmationEmail
{
    public class SendConfirmationEmailCommand : IRequest
    {
        [JsonIgnore]
        public string ConfirmationRoute { get; } = "api/AppUsers/confirm";
        public string Email { get; set; }
    }
}
