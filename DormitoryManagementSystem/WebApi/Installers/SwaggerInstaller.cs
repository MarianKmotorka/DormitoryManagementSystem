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
            var description = @"GUEST: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJqb2huQGd1ZXN0LmNvbSIsImp0aSI6IjBkMzY4MGIwLTFiOTItNGQwNC05ODgxLTllZWZlNzA5NjEzMCIsImVtYWlsIjoiam9obkBndWVzdC5jb20iLCJhcHBVc2VySWQiOiJiNDJmNWQ5OS1iMzEzLTQ1ZDAtODhmMy0zODQyN2Y5OGMxMDAiLCJyb2xlIjoiR3Vlc3QiLCJuYmYiOjE1ODA5MTM1NjgsImV4cCI6MjQ0NDkxMzU2OCwiaWF0IjoxNTgwOTEzNTY4fQ.3GNp_PPTD-1D-m0-d9kLJeRrDoBr9C06mbSjYcNH8hA

                                ADMIN: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbkBhZG1pbi5jb20iLCJqdGkiOiJkYjhlMmQ1MS02ZjdhLTQ5YTEtYTg0Zi0zZWEzMmFkZDU3MDkiLCJlbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImFwcFVzZXJJZCI6ImQ1MWYzOWVjLTg4ODItNDkxNy04OTlkLWNhZDRhMjRmMTEzZCIsInJvbGUiOiJTeXNBZG1pbiIsIm5iZiI6MTU4MDkwODM4NSwiZXhwIjoyNDQ0OTA4Mzg1LCJpYXQiOjE1ODA5MDgzODV9.qbRjSjy7yBl4uOlmIFw8--1HKWPQieMLOg6L2jUIovE

                                OFFICER: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJqb2huQG9mZmljZXIuY29tIiwianRpIjoiM2I0YzgzYjctNGQxZC00MmY0LWFlNjUtYTIyZDIxODY0Y2JiIiwiZW1haWwiOiJqb2huQG9mZmljZXIuY29tIiwiYXBwVXNlcklkIjoiZWI1MjNlODYtZmVjOS00NzQ1LTg4NWEtNGJkMWY0NTA4YmI1Iiwicm9sZSI6Ik9mZmljZXIiLCJuYmYiOjE1ODA5MTM2MDIsImV4cCI6MjQ0NDkxMzYwMiwiaWF0IjoxNTgwOTEzNjAyfQ.970H85CECuQ801rDqGHNVFcNQ04-tDULEgmo8CLDcPM";

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
