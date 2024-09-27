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
}