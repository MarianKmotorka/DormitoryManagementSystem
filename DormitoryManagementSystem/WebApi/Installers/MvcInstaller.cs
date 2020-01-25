using Application.Common.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Installers
{
    public class MvcInstaller : IInstaller
    {
        public IServiceCollection Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<IDormitoryDbContext>());

            services.AddHttpContextAccessor();

            //suppress ModelStateInvalidFilter from throwing BadRequestException to enable custom Exception throwing
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}
