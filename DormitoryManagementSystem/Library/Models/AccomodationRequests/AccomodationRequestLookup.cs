using System;

namespace Library.Models.AccomodationRequests
{
    public class AccomodationRequestLookup
    {
        public int Id { get; set; }

        public int RequesterDistanceFromHome { get; set; }

        public DateTime AccomodationStartDateUtc { get; set; }

        public DateTime AccomodationEndDateUtc { get; set; }

        public AccomodationRequestState RequestState { get; set; }
    }
}
