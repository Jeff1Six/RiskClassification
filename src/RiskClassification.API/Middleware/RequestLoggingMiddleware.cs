using System.Diagnostics;

namespace RiskClassification.API.Middleware;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            await _next(context);
            sw.Stop();

            _logger.LogInformation(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                context.Request.Method,
                context.Request.Path.Value,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds
            );
        }
        catch (Exception)
        {
            sw.Stop();

            _logger.LogWarning(
                "HTTP {Method} {Path} failed in {ElapsedMs}ms",
                context.Request.Method,
                context.Request.Path.Value,
                sw.ElapsedMilliseconds
            );

            throw;
        }
    }
}