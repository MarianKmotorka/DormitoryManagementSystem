using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class GuestRegistrationModel
    {
        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string PostCode { get; set; }

        public string IdCardNumber { get; set; }

        public int DistanceFromHome { get; set; }
    }
}
