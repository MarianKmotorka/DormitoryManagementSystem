using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class IdentityConfiguration
    {
        public class UsersConfiguration : IEntityTypeConfiguration<AppUser>
        {
            public void Configure(EntityTypeBuilder<AppUser> builder)
            {
                builder.ToTable("AppUsers");
                builder.OwnsOne(x => x.Address);
                builder.Property(x => x.FirstName).IsRequired();
                builder.Property(x => x.LastName).IsRequired();
            }
        }

        public class RolesConfiguration : IEntityTypeConfiguration<IdentityRole>
        {
            public void Configure(EntityTypeBuilder<IdentityRole> builder)
            {
                builder.ToTable("AppRoles");
            }
        }

        public class UserRolesConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
        {
            public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
            {
                builder.ToTable("AppUserAppRoles");
            }
        }

        public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
        {
            public void Configure(EntityTypeBuilder<RefreshToken> builder)
            {
                builder.HasKey(x => x.Token);

                builder
                    .HasOne(x => x.AppUser)
                    .WithMany()
                    .HasForeignKey(x => x.AppUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Property(x => x.JwtId).IsRequired();
            }
        }
    }
}
