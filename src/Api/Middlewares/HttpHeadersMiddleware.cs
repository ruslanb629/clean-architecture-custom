using System.Diagnostics;
using System.Globalization;

namespace Api.Middlewares;

/// <summary>
/// A class representing middleware for adding custom HTTP response headers. This class cannot be inherited.
/// </summary>
public sealed class HttpHeadersMiddleware
{
    /// <summary>
    /// The delegate for the next part of the pipeline. This field is read-only.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpHeadersMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate for the next part of the pipeline.</param>
    public HttpHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware asynchronously.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the actions performed by the middleware.
    /// </returns>
    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-Powered-By");

            context.Response.Headers.Remove("Referrer-Policy");
            context.Response.Headers.Remove("X-Content-Type-Options");
            context.Response.Headers.Remove("X-Frame-Options");
            context.Response.Headers.Remove("X-XSS-Protection");
            context.Response.Headers.Remove("X-Request-Id");
            context.Response.Headers.Remove("X-Request-Duration");

            context.Response.Headers.Add("Referrer-Policy", "no-referrer");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

            context.Response.Headers.Add("X-Request-Id", context.TraceIdentifier);

            stopwatch.Stop();

            string duration = stopwatch.Elapsed.TotalMilliseconds.ToString(
                "0.00ms",
                CultureInfo.InvariantCulture);

            context.Response.Headers.Add("X-Request-Duration", duration);

            return Task.CompletedTask;
        });

        await _next(context).ConfigureAwait(false);
    }
}


public static class HttpHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseHttpHeadersMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HttpHeadersMiddleware>();
    }
}

