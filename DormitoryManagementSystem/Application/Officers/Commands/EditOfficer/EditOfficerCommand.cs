using MediatR;
using Newtonsoft.Json;

namespace Application.Officers.Commands.EditOfficer
{
    public class EditOfficerCommand : IRequest
    {
        [JsonIgnore]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string PostCode { get; set; }

        public string OfficeNumber { get; set; }
    }
}
