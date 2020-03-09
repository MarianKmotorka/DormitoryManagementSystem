using System;
using Library.Models.Rooms;

namespace Library.Models.RepairRequests
{
    public class RepairRequestModel
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public string FixedByDisplayName { get; set; }

        public string ReportedByDisplayName { get; set; }

        public RoomItemTypeLookup RoomItemType { get; set; }

        public RepairRequestState State { get; set; }

        public DateTime? WillBeFixedOn { get; set; }

        public DateTime? FixedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ProblemDesciption { get; set; }

        public string RepairerReply { get; set; }
    }
}
