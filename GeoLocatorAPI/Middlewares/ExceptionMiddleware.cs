using GeoLocatorAPI.Errors;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace GeoLocatorAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                //await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                // Log Ex in DataBase

                context.Response.ContentType = "application/json";
                var statusCode = ex switch
                {
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    ValidationException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                context.Response.StatusCode = statusCode;

                var response = _env.IsDevelopment()
                    ? new ApiExceptionResponse(statusCode, ex.Message, ex.StackTrace)
                    : new ApiExceptionResponse(statusCode);



                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
