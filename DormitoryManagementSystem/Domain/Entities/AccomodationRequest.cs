﻿using System;
using Domain.Enums;

namespace Domain.Entities
{
    public class AccomodationRequest
    {
        public int Id { get; set; }

        public DateTime RequestPlacedUtc { get; set; }

        public Guest Requester { get; set; }

        public DateTime AccomodationStartDateUtc { get; set; }

        public DateTime AccomodationEndDateUtc { get; set; }

        public AccomodationRequestState State { get; set; }

        public string RequesterMessage { get; set; }
    }
}
