using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.General
{


    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string message;

            switch (exception)
            {
                case ArgumentException argEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = argEx.Message;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            _logger.LogError(exception, "Unhandled exception");

            var response = new ResponseDto<object>
            {
                Data = null,
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }


}
