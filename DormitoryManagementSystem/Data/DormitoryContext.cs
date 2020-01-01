using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    public class DormitoryContext : IdentityDbContext<AppUser>
    {
        public DormitoryContext(DbContextOptions<DormitoryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(DormitoryContext).Assembly);
        }
    }
}
