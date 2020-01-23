using Application.Common.Interfaces;
using Infrastracture.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastracture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastracture(this IServiceCollection services)
        {
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }
    }
}
