using System.ComponentModel.DataAnnotations;
using Library.Models.Users;

namespace Library.Models.Identity
{
    public class ChangePasswordByAdminModel
    {
        [Required(ErrorMessage = "Required")]
        public UserLookup User { get; set; }

        [Required(ErrorMessage = "Required")]
        public string NewPassword { get; set; }
    }
}
