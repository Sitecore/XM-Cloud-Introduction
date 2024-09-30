using MvpSite.Rendering.AppSettings;
using Sitecore.AspNetCore.SDK.RenderingEngine;

namespace MvpSite.Rendering.Middleware;

public class MvpProfileRoutingMiddleware(
    RequestDelegate next,
    IConfiguration configuration)
{
    private readonly MvpSiteSettings _settings = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>() ?? new MvpSiteSettings();

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.RouteValues[RenderingEngineConstants.RouteValues.SitecoreRoute] = _settings.MvpProfilePageItemPath;
        await next(context);
    }
}