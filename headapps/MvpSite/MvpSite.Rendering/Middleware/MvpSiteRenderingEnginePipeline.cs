using MvpSite.Rendering.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Middleware;

namespace MvpSite.Rendering.Middleware;

public class MvpSiteRenderingEnginePipeline
    : RenderingEnginePipeline
{
    public override void Configure(IApplicationBuilder app)
    {
        app.UseSitecoreRenderingEngine();
        app.UseNotFoundRouting();
    }
}