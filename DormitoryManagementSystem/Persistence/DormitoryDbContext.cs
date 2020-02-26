using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DormitoryDbContext : IdentityDbContext<AppUser>, IDormitoryDbContext
    {
        public DormitoryDbContext(DbContextOptions<DormitoryDbContext> options) : base(options) { }

        public DbSet<Guest> Guests { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Officer> Officers { get; set; }

        public DbSet<AccomodationRequest> AccomodationRequests { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<RoomItemType> RoomItemTypes { get; set; }

        public DbSet<InventoryItemType> InventoryItemTypes { get; set; }

        public DbSet<Repairer> Repairers { get; set; }

        public DbSet<RepairRequest> RepairRequests { get; set; }

        public DbSet<Office> Offices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(DormitoryDbContext).Assembly);
        }
    }
}
