using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.HasOne(x => x.AppUser)
                .WithOne()
                .HasForeignKey<Guest>("AppUserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.IdCardNumber).IsRequired();
            builder.Property(x => x.DistanceFromHome).IsRequired();
        }
    }
}
