using MediatR;
using Newtonsoft.Json;

namespace Application.Officers.Commands.RespondToAccomodationRequest
{
    public class RespondToAccomodationRequestCommand : IRequest
    {
        [JsonIgnore]
        public int AccomodationRequestId { get; set; }

        public bool IsAccomodationRequestApproved { get; set; }

        public int RoomId { get; set; }

        public string AdditionalMessage { get; set; }
    }
}
