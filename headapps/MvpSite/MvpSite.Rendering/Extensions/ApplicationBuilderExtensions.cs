using System.Text;
using Microsoft.Extensions.Primitives;
using MvpSite.Rendering.Middleware;

namespace MvpSite.Rendering.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder EnsureAcceptLanguageHeader(this IApplicationBuilder app)
    {
        app.UseMiddleware<EnsureAcceptLanguageHeaderMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseNotFoundRouting(this IApplicationBuilder app)
    {
        app.UseMiddleware<NotFoundRoutingMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseMvpProfileRouting(this IApplicationBuilder app)
    {
        app.UseMiddleware<MvpProfileRoutingMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        IApplicationBuilder result = app.Use(static async (context, next) =>
        {
            StringBuilder cspBuilder = new();
            cspBuilder.Append("default-src 'self'; ");

            cspBuilder.Append("script-src 'self' 'unsafe-inline' 'unsafe-eval' ");
            cspBuilder.Append("https://www.googletagmanager.com ");
            cspBuilder.Append("https://www.google-analytics.com ");
            cspBuilder.Append("https://cdn.stat-track.com ");
            cspBuilder.Append("https://code.jquery.com ");
            cspBuilder.Append("https://cdn.jsdelivr.net ");
            cspBuilder.Append("https://stackpath.bootstrapcdn.com ");
            cspBuilder.Append("https://cdnjs.cloudflare.com ");
            cspBuilder.Append("https://www.w3.org ");
            cspBuilder.Append("https://edge.sitecorecloud.io; ");

            cspBuilder.Append("style-src 'self' 'unsafe-inline' ");
            cspBuilder.Append("https://stackpath.bootstrapcdn.com ");
            cspBuilder.Append("https://cdnjs.cloudflare.com ");
            cspBuilder.Append("https://fonts.googleapis.com; ");

            cspBuilder.Append("img-src 'self' data: ");
            cspBuilder.Append("https://www.googletagmanager.com ");
            cspBuilder.Append("https://www.google-analytics.com ");
            cspBuilder.Append("https://*.sitecorecloud.io ");   
            cspBuilder.Append("https://www.gravatar.com ");            
            cspBuilder.Append("https://community.sitecore.com ");
            cspBuilder.Append("https://delivery-sitecore.sitecorecontenthub.cloud; ");

            cspBuilder.Append("font-src 'self' ");
            cspBuilder.Append("https://fonts.gstatic.com ");
            cspBuilder.Append("https://cdnjs.cloudflare.com; ");

            cspBuilder.Append("connect-src 'self' ");
            cspBuilder.Append("https://www.google-analytics.com ");
            cspBuilder.Append("https://www.googletagmanager.com ");
            cspBuilder.Append("https://cdn.stat-track.com ");
            cspBuilder.Append("https://edge.sitecorecloud.io ");
            cspBuilder.Append("https://*.sitecorecloud.io; ");

            cspBuilder.Append("frame-src 'self'; ");

            cspBuilder.Append("frame-ancestors 'self' ");
            cspBuilder.Append("https://*.sitecorecloud.io; ");

            cspBuilder.Append("base-uri 'self'; ");
            cspBuilder.Append("form-action 'self'; ");
            cspBuilder.Append("object-src 'none'");

            context.Response.Headers.ContentSecurityPolicy = new StringValues(cspBuilder.ToString());

            // ReSharper disable once StringLiteralTypo - specific value for X-Content-Type-Options header
            context.Response.Headers.XContentTypeOptions = new StringValues("nosniff");

            context.Response.Headers.XFrameOptions = new StringValues("SAMEORIGIN");

            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

            context.Response.Headers.Append("Permissions-Policy", "camera=(), microphone=(), geolocation=()");

            await next(context);
        });

        return result;
    }
}