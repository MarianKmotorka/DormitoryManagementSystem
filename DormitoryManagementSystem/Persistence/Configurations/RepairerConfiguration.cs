using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class RepairerConfiguration : IEntityTypeConfiguration<Repairer>
    {
        public void Configure(EntityTypeBuilder<Repairer> builder)
        {
            builder.HasOne(x => x.AppUser)
               .WithMany()
               .HasForeignKey("AppUserId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
