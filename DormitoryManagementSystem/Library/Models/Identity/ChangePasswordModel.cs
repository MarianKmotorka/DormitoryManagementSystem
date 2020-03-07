using System.ComponentModel.DataAnnotations;

namespace Library.Models.Identity
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Required")]
        public string NewPassword { get; set; }
    }
}
