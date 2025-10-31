
using IpBlock.Exceptions;
using System.Net;
using System.Text.Json;

namespace IpBlock.Middleware
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode status;
            string message = ex.Message;

            switch (ex)
            {
                case AppException appEx:
                    status = appEx.StatusCode;
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                    break;
            }

            context.Response.StatusCode = (int)status;

            var errorResponse = new
            {
                success = false,
                statusCode = context.Response.StatusCode,
                message,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
