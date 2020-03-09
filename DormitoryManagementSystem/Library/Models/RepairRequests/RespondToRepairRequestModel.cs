using System;

namespace Library.Models.RepairRequests
{
    public class RespondToRepairRequestModel
    {
        public RepairRequestState RepairRequestState { get; set; } = RepairRequestState.Accepted;

        public string RepairerReply { get; set; } = string.Empty;

        public DateTime WillBeFixedOn { get; set; } = DateTime.UtcNow;
    }
}
