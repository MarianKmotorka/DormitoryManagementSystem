using MediatR;
using Newtonsoft.Json;

namespace Application.AppUsers.Commands.ChangePasswordByAdmin
{
    public class ChangePasswordByAdminCommand : IRequest
    {
        [JsonIgnore]
        public string Id { get; set; }

        public string NewPassword { get; set; }
    }
}
