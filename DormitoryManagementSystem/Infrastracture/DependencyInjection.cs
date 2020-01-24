using Application.Common.Interfaces;
using Infrastracture.Identity;
using Infrastracture.Messaging;
using Infrastracture.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastracture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastracture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IEmailService, EmailService>();

            services.Configure<EmailServiceOptions>(configuration.GetSection(nameof(EmailServiceOptions)));

            return services;
        }
    }
}
