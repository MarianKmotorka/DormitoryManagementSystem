using MediatR;
using Newtonsoft.Json;

namespace Application.AppUsers.Commands.SendChangeForgottenPasswordEmail
{
    public class SendChangeForgottenPasswordEmailCommand : IRequest
    {
        [JsonIgnore]
        public string ChangeForgottenPasswordRoute { get; } = "api/AppUsers/forgotten-password";
        public string Email { get; set; }
    }
}
