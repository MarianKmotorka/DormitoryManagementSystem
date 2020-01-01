using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DormitoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DormitoryDb")));

            services.AddDefaultIdentity<AppUser>();

            return services;
        }
    }
}
