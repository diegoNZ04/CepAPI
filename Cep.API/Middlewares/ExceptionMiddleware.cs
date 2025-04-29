using System.Text.Json;
using Cep.Application.Exceptions;

namespace Cep.API.Middlewares;

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
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            ExternalServiceException => (StatusCodes.Status503ServiceUnavailable, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno no servidor")
        };

        context.Response.StatusCode = statusCode;

        var response = _env.IsDevelopment()
            ? JsonSerializer.Serialize(new
            {
                error = message,
                stackTrace = exception.StackTrace
            })
            : JsonSerializer.Serialize(new
            {
                error = message
            });

        await context.Response.WriteAsync(response);
    }
}
