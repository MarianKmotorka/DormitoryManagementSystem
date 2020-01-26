﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class OfficerConfiguration : IEntityTypeConfiguration<Officer>
    {
        public void Configure(EntityTypeBuilder<Officer> builder)
        {
            builder.HasOne(x => x.AppUser)
               .WithMany()
               .HasForeignKey("AppUserId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.OfficeNumber).IsRequired();
            builder.Property(x => x.IdCardNumber).IsRequired();
        }
    }
}
