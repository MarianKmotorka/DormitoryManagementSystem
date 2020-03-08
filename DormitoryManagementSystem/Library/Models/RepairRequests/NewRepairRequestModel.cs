using System.ComponentModel.DataAnnotations;
using Library.Models.Rooms;

namespace Library.Models.RepairRequests
{
    public class NewRepairRequestModel
    {
        [Required(ErrorMessage = "Required")]
        public RoomItemTypeLookup RoomItemType { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ProblemDescription { get; set; }
    }
}
