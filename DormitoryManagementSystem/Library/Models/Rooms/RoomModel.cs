using System.Collections.Generic;
using Library.Models.Guests;

namespace Library.Models.Rooms
{
    public class RoomModel
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public int Capacity { get; set; }

        public int FreeBeds { get; set; }

        public IEnumerable<GuestLookup> Guests { get; set; }

        public IEnumerable<RoomItemTypeLookup> Items { get; set; }
    }
}
