using MediatR;
using Newtonsoft.Json;

namespace Application.RepairRequests.Commands.CreateRepairRequest
{
    public class CreateRepairRequestCommand : IRequest
    {
        [JsonIgnore]
        public string GuestId { get; set; }

        public int RoomItemTypeId { get; set; }

        public string ProblemDescription { get; set; }
    }
}
