using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Pagination.CustomFilteringAndSorting;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddScoped<ISieveCustomSortMethods, SortMethods>();
            services.AddScoped<ISieveCustomFilterMethods, FilterMethods>();

            return services;
        }
    }
}
