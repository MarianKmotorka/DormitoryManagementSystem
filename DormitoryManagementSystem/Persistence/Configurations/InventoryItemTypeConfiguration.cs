using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class InventoryItemTypeConfiguration : IEntityTypeConfiguration<InventoryItemType>
    {
        public void Configure(EntityTypeBuilder<InventoryItemType> builder)
        {
            builder.HasIndex(x => x.InventoryNumber).IsUnique();
            builder.Property(x => x.InventoryNumber).IsRequired();
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.PricePerPiece).HasColumnType("decimal(18,4)");
        }
    }
}
