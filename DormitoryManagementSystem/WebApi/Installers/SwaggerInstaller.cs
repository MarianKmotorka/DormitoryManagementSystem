using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace WebApi.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public IServiceCollection Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Dormitory Api", Version = "v1" });

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBZG1pbkBhZG1pbi5jb20iLCJqdGkiOiIyOTY4MTgyMS0yYTA1LTQyNjQtOWQ5Ni1lN2QyODNkYWI3YTIiLCJlbWFpbCI6IkFkbWluQGFkbWluLmNvbSIsImFwcFVzZXJJZCI6IjRkNWY4NjhhLWI4ZDAtNDY5Yy05ZjU4LTgyYmRhMzAwYTc2NiIsInJvbGUiOiJTeXNBZG1pbiIsIm5iZiI6MTU4MDA0MTc2MCwiZXhwIjoyNDQ0MDQxNzYwLCJpYXQiOjE1ODAwNDE3NjB9.OsXjFu6xkzmdV4vQ2kSGi1N6etr8fQljMjl-nHFCv5U",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            return services;
        }
    }
}
