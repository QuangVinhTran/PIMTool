using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PIMTool.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                //await context.Response.WriteAsync(new
                //{
                //    Code = 500,
                //    Message = ex.Message
                //}.ToString() ?? string.Empty);

                ProblemDetails problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Type = "Server error",
                    Title = "An internal server error has occurred",
                    Detail = ex.Message,
                };

                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
        }
    }
}