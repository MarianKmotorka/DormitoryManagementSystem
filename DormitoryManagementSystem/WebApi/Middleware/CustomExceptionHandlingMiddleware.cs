using System;
using System.Net;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApi.Common;

namespace WebApi.Middleware
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
            var code = HttpStatusCode.BadRequest;
            ErrorResponse errorResponse;

            switch (exception)
            {
                case ValidationException validationException:
                    errorResponse = ErrorResponseUtil.CreateBadRequestErrorResponse(validationException.Errors);
                    break;
                case BadRequestException _:
                    errorResponse = ErrorResponseUtil.CreateBadRequestErrorResponse(exception.Message);
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    errorResponse = ErrorResponseUtil.CreateNotFoundErrorResponse(exception.Message);
                    break;
                case ArgumentNullException argumentNullException when argumentNullException.Source == "MediatR":
                    errorResponse = ErrorResponseUtil.CreateBadRequestErrorResponse("Request is in bad format");
                    break;
                default:
                    _logger.LogError(exception, string.Empty);
                    errorResponse = ErrorResponseUtil.CreateBadRequestErrorResponse("Processing error");
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var result = JsonConvert.SerializeObject(errorResponse,
                new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } });

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