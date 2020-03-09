using System;

namespace Library.Models.RepairRequests
{
    public class RepairRequestLookup
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? WillBeFixedOn { get; set; }

        public DateTime? FixedOn { get; set; }

        public RepairRequestState State { get; set; }
    }
}
