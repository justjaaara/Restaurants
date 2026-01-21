
using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        var stopwatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds / 1000 > 4) 
            logger.LogWarning("Request [{Verb}] at {Path} took {Time}s", context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds / 1000);

        }
}
