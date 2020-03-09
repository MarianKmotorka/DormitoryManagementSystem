using System;
using Domain.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.RepairRequests.Commands.RespondToRepairRequest
{
    public class RespondToRepairRequestCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public string FixedById { get; set; }

        public RepairRequestState RepairRequestState { get; set; }

        public string RepairerReply { get; set; }

        public DateTime? WillBeFixedOn { get; set; }
    }
}
