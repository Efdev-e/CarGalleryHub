using CarGalleryHub.Application.Exceptions;
using System.Text.Json;

namespace CarGalleryHub.Api.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
                _logger.LogError(ex, "İşlenmeyen hata:{Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }


        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            context.Response.ContentType = "application/json";
            var (statusCode, message) = exception switch
            {

                Unsuccessful ve => (422, ve.Message),
                NotFound ne => (404, ne.Message),
                UnAuthorized ue => (403, ue.Message),
                AppException ae => (ae.StatusCode, ae.Message),
                _ => (500, "Beklenmeyen Hata")

            };
            context.Response.StatusCode = statusCode;
            var response = new
            {
                success = false,
                message,
            };
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await context.Response.WriteAsync(json);


        }
    }
}
