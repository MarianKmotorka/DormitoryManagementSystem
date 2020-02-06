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
                .WithMany()
                .HasForeignKey("AppUserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.AccomodationRequests)
                .WithOne(x => x.Requester)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Room)
                .WithMany(x => x.Guests)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.IdCardNumber).IsRequired();
            builder.Property(x => x.DistanceFromHome).IsRequired();
        }
    }
}
