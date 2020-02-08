namespace Domain.Entities
{
    public class RoomItemType
    {
        public int Id { get; set; }

        public Room Room { get; set; }

        public InventoryItemType InventoryItemType { get; set; }

        public int Quantity { get; set; }
    }
}
