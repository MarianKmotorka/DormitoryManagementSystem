using MediatR;

namespace Application.Officers.Commands.CreateOfficer
{
    public class CreateOfficerCommand : IRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string OfficeNumber { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string PostCode { get; set; }

        public string IdCardNumber { get; set; }
    }
}
//TODO send email to officer to change their password