using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class RoomItemTypeConfiguration : IEntityTypeConfiguration<RoomItemType>
    {
        public void Configure(EntityTypeBuilder<RoomItemType> builder)
        {
            builder.HasOne(x => x.InventoryItemType)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Room)
                .WithMany(x => x.Items)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
