using System;
using System.Net;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApi.Common
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(ErrorResponseUtil.CreateBadRequestErrorResponse(validationException.Errors));
                    break;
                case BadRequestException _:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(ErrorResponseUtil.CreateBadRequestErrorResponse(exception.Message));
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    result = JsonConvert.SerializeObject(ErrorResponseUtil.CreateNotFoundErrorResponse(exception.Message));
                    break;
                case ArgumentNullException argumentNullException when argumentNullException.Source == "MediatR":
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(ErrorResponseUtil.CreateBadRequestErrorResponse("Request is in bad format"));
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (code == HttpStatusCode.InternalServerError)
                _logger.LogError(exception, string.Empty);

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}