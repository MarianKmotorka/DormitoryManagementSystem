using Application.Common.Enums;
using Infrastracture.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Infrastracture.DependecyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(x =>
            {
                x.AddPolicy(PolicyNames.Guest, builder =>
                {
                    builder.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(ClaimTypes.Role, AppRoleNames.Guest.ToString())
                        || context.User.HasClaim(ClaimTypes.Role, AppRoleNames.SysAdmin.ToString());
                    });
                });

                x.AddPolicy(PolicyNames.Officer, builder =>
                {
                    builder.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(ClaimTypes.Role, AppRoleNames.Officer.ToString())
                        || context.User.HasClaim(ClaimTypes.Role, AppRoleNames.SysAdmin.ToString());
                    });
                });

                x.AddPolicy(PolicyNames.Repairer, builder =>
                {
                    builder.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(ClaimTypes.Role, AppRoleNames.Repairer.ToString())
                        || context.User.HasClaim(ClaimTypes.Role, AppRoleNames.SysAdmin.ToString());
                    });
                });

                x.AddPolicy(PolicyNames.Admin, builder => builder.RequireClaim(ClaimTypes.Role, AppRoleNames.SysAdmin.ToString()));
            });

            return services;
        }
    }
}
