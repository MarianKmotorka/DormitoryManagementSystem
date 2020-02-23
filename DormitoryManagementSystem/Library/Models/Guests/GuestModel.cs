using System.ComponentModel.DataAnnotations;

namespace Library.Models.Guests
{
    public class GuestModel
    {
        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }

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

        [Required(ErrorMessage = "Required")]
        public string IdCardNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        public int DistanceFromHome { get; set; }

        public string RoomNumber { get; set; }

        public string DormitoryCardNumber { get; set; }
    }
}
