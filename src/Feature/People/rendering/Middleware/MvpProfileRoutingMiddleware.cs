using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using System.Threading.Tasks;
using Sitecore.AspNet.RenderingEngine;

namespace Mvp.Feature.People.Middleware
{
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
}
