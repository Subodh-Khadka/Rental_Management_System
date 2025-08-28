using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rental_Management_System.Server.DTOs;
using Rental_Management_System.Server.Exceptions;
using System.Net;

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
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    BadRequestException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var response = ApiResponse<string>.Fail(ex.Message);
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
