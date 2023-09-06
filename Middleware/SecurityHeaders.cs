using System.Collections.Specialized;

namespace MyDotNetAPI.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public SecurityHeadersMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        NameValueCollection headersToAdd = new NameValueCollection
        {
            ["Access-Control-Allow-Origin"] = _configuration["AllowedOrigins"],
            ["Content-Security-Policy"] = "default-src 'self';",
            ["Permissions-Policy"] = "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()",
            ["Referrer-Policy"] = "same-origin",
            ["X-Content-Type-Options"] = "nosniff",
            ["X-Frame-Options"] = "DENY",
            ["X-Permitted-Cross-Domain-Policies"] = "none",
            ["X-Xss-Protection"] = "1; mode=block"
        };

        foreach (string header in headersToAdd)
        {
            if (context.Response.Headers.ContainsKey(header)){
                continue;
            }

            context.Response.Headers.Add(header, headersToAdd[header]);
        }

        string[] headersToRemove = new string[]{
            "X-Powered-By",
        };

        foreach (string header in headersToRemove)
        {
            if (!context.Response.Headers.ContainsKey(header)){
                continue;
            }
            context.Response.Headers.Remove(header);
        }


        await _next(context);
    }
}

public static class SecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityHeadersMiddleware>();
    }
}