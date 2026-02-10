using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rental_Management_System.Server.DTOs;

namespace Rental_Management_System.Server.Filters
{
    public class AuthExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode = 500; // default to Internal Server Error
            string message = context.Exception.Message;

            // Customize status code based on the exception type
            if (context.Exception is ArgumentNullException)
                statusCode = 400; // Bad Request
            else if (context.Exception is UnauthorizedAccessException)
                statusCode = 401; // Unauthorized
            else if (context.Exception is KeyNotFoundException)
                statusCode = 404; // Not Found
                                  // You can add more custom exceptions as needed

            var response = ApiResponse<string>.FailResponse(message);

            context.Result = new JsonResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true; // Mark exception as handled
        }
    }
}
