﻿using System;
using Domain.Enums;

namespace Domain.Entities
{
    public class RepairRequest
    {
        public int Id { get; set; }

        public RoomItemType RoomItemType { get; set; }

        public Guest Guest { get; set; }

        public Repairer FixedBy { get; set; }

        public RepairRequestState State { get; set; }

        public DateTime? WillBeFixedOn { get; set; }

        public DateTime? FixedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ProblemDesciption { get; set; }

        public string RepairerReply { get; set; }
    }
}
