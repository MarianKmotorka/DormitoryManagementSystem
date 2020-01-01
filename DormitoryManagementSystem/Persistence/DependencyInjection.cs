using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DormitoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DormitoryDb")));

            services.AddDefaultIdentity<AppUser>(o =>
            {
                o.Password.RequiredLength = 6;
                o.Password.RequireDigit = true;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DormitoryContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
