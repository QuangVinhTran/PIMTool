using System.Net;
using Application.Exceptions;
using Domain.Entities.ErrorModel;

namespace WebAPI.Middlewares
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        private async Task WriteErrorResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(new ErrorDetail
            {
                StatusCode = statusCode,
                Message = message
            }.ToString());
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await WriteErrorResponse(context, 500, "Internal error");
            }
        }
    }
}
