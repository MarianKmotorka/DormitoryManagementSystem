using System.ComponentModel.DataAnnotations;

namespace Library.Models.Repairers
{
    public class RepairerModel
    {
        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Required")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        public string PostCode { get; set; }
    }
}
