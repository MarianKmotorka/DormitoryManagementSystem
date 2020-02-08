using System.Collections.Generic;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int Capacity { get; set; }

        public ICollection<Guest> Guests { get; set; }

        public ICollection<RoomItemType> Items { get; set; }
    }
}
