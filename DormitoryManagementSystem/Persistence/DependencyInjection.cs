using Application.Common.Interfaces;
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
            services.AddDbContext<DormitoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DormitoryDb")));

            services.AddTransient<IDormitoryDbContext, DormitoryDbContext>();

            services.AddIdentity<AppUser, IdentityRole>(o =>
            {
                o.Password.RequiredLength = 6;
                o.Password.RequireDigit = true;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DormitoryDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
