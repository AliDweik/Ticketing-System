using System;
using System.Net;
using System.Text.Json;
using TicketingSystem.Data.Exceptions;

namespace TicketingSystem.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            
            switch (ex)
            {
                case AppException eeex:
                    _logger.LogError(ex, ex.Message+" exception occurred");
                    result = JsonSerializer.Serialize(new
                    {
                        Error = ex.Message+" error occurred",
                        ExceptionId = Guid.NewGuid()
                    });
                    context.Response.StatusCode = Convert.ToInt32(eeex.ErrorCode);
                    break;
                case KeyNotFoundException exx :
                    _logger.LogError(ex, ex.Message + " exception occurred");
                    result = JsonSerializer.Serialize(new
                    {
                        Error = exx.Message,
                        ExceptionId = Guid.NewGuid()
                    });
                    context.Response.StatusCode = 400;
                    break;
                default:
                    _logger.LogError(ex, "Unhandled exception occurred");
                    result = JsonSerializer.Serialize(new
                    {
                        Error = "An unexpected error occurred",
                        ExceptionId = Guid.NewGuid()
                    });
                    context.Response.StatusCode = 500;
                    break;
            }

            context.Response.ContentType = "application/json";

            if (string.IsNullOrEmpty(result))
            {
                result = JsonSerializer.Serialize(new { error = ex.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
