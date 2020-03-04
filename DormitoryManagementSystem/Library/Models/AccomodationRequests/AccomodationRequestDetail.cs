using System;

namespace Library.Models.AccomodationRequests
{
    public class AccomodationRequestDetail
    {
        public int Id { get; set; }

        public string RequesterId { get; set; }

        public string RequesterFirstName { get; set; }

        public string RequesterLastName { get; set; }

        public string RequesterEmail { get; set; }

        public string RequesterMessage { get; set; }

        public int RequesterDistanceFromHome { get; set; }

        public DateTime AccomodationStartDateUtc { get; set; }

        public DateTime AccomodationEndDateUtc { get; set; }

        public AccomodationRequestState RequestState { get; set; }
    }
}
