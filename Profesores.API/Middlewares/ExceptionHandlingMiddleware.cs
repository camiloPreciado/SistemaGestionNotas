using Profesores.Application.Common;

namespace Profesores.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación.");

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = 400,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado.");

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = 500,
                    message = "Ha ocurrido un error interno en el servidor."
                });
            }
        }
    }
}
