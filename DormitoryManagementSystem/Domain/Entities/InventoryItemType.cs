namespace Domain.Entities
{
    public class InventoryItemType
    {
        public int Id { get; set; }

        public string InventoryNumber { get; set; }

        public string Name { get; set; }

        public decimal PricePerPiece { get; set; }

        public int TotalQuantity { get; set; }
    }
}
