using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.Exceptions;
using System.Net;
using System.Text.Json;

namespace Rental_Management_System.Server.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
            private static Task HandleExceptionAsync(HttpContext context, Exception ex)
            {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = "Something went wrong. Please try again later.",
                detail = ex.Message // later remove this in production so that we do not expose important details to the client
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
 
}
