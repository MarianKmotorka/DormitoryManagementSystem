using System;
using MediatR;
using Newtonsoft.Json;

namespace Application.AccomodationRequests.Commands.CreateAccomodationRequest
{
    public class CreateAccomodationRequestCommand : IRequest
    {
        [JsonIgnore]
        public string RequesterId { get; set; }

        public DateTime AccomodationStartDateUtc { get; set; }

        public DateTime AccomodationEndDateUtc { get; set; }

        public string RequesterMessage { get; set; }
    }
}
