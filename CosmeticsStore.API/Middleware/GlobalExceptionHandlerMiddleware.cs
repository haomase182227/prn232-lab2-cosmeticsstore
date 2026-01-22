using CosmeticsStore.API.Models;
using System.Net;
using System.Text.Json;

namespace CosmeticsStore.API.Middleware;

/// <summary>
/// Global Exception Handler Middleware
/// Following PRN232 requirement: Handle all exceptions globally without try-catch in controllers
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            KeyNotFoundException => new
            {
                statusCode = (int)HttpStatusCode.NotFound,
                response = ApiResponse.ErrorResponse("Resource not found", exception.Message)
            },
            UnauthorizedAccessException => new
            {
                statusCode = (int)HttpStatusCode.Unauthorized,
                response = ApiResponse.ErrorResponse("Unauthorized access", exception.Message)
            },
            ArgumentException or ArgumentNullException => new
            {
                statusCode = (int)HttpStatusCode.BadRequest,
                response = ApiResponse.ErrorResponse("Invalid request", exception.Message)
            },
            InvalidOperationException => new
            {
                statusCode = (int)HttpStatusCode.BadRequest,
                response = ApiResponse.ErrorResponse("Invalid operation", exception.Message)
            },
            _ => new
            {
                statusCode = (int)HttpStatusCode.InternalServerError,
                response = ApiResponse.ErrorResponse(
                    "An error occurred while processing your request",
                    "Internal server error")
            }
        };

        context.Response.StatusCode = response.statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response.response, options));
    }
}

/// <summary>
/// Extension method to register the middleware
/// </summary>
public static class GlobalExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
