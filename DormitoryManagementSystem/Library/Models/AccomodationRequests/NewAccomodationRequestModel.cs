using System;

namespace Library.Models.AccomodationRequests
{
    public class NewAccomodationRequestModel
    {
        public DateTime AccomodationStartDateUtc { get; set; } = DateTime.UtcNow.AddDays(1);

        public DateTime AccomodationEndDateUtc { get; set; } = DateTime.UtcNow.AddDays(1);

        public string RequesterMessage { get; set; }
    }
}
