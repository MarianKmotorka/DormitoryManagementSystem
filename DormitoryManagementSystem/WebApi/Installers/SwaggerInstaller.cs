using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WebApi.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public IServiceCollection Install(IServiceCollection services, IConfiguration configuration)
        {
            var description = @"GUEST: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb3JlZm9vZDg4QGdtYWlsLmNvbSIsImp0aSI6IjIyZjc5MTg2LTkxYWMtNGYyMi1iZTUxLTMyNzEwNWRlYjE2MyIsImVtYWlsIjoibW9yZWZvb2Q4OEBnbWFpbC5jb20iLCJhcHBVc2VySWQiOiJmM2ZhNDY3Yy1jOGEwLTQxOWEtYTA0NC02MDc0ZjZlZTU5NWUiLCJyb2xlIjoiR3Vlc3QiLCJuYmYiOjE1ODA4OTg0ODcsImV4cCI6MjQ0NDg5ODQ4NywiaWF0IjoxNTgwODk4NDg3fQ.yorwmaJQCCMtn3obvCDki5LdKaxeu8AXfV6QwPRNXuM
                                ADMIN: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBZG1pbkBhZG1pbi5jb20iLCJqdGkiOiIyOTY4MTgyMS0yYTA1LTQyNjQtOWQ5Ni1lN2QyODNkYWI3YTIiLCJlbWFpbCI6IkFkbWluQGFkbWluLmNvbSIsImFwcFVzZXJJZCI6IjRkNWY4NjhhLWI4ZDAtNDY5Yy05ZjU4LTgyYmRhMzAwYTc2NiIsInJvbGUiOiJTeXNBZG1pbiIsIm5iZiI6MTU4MDA0MTc2MCwiZXhwIjoyNDQ0MDQxNzYwLCJpYXQiOjE1ODAwNDE3NjB9.OsXjFu6xkzmdV4vQ2kSGi1N6etr8fQljMjl-nHFCv5U";

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Dormitory Api", Version = "v1" });

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = description,
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
