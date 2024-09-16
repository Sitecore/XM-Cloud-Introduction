using Microsoft.AspNetCore.Builder;
using Mvp.Project.MvpSite.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Middleware;

namespace Mvp.Project.MvpSite.Middleware
{
    public class MvpSiteRenderingEnginePipeline
        : RenderingEnginePipeline
    {
        public override void Configure(IApplicationBuilder app)
        {
            app.UseSitecoreRenderingEngine();
            app.UseNotFoundRouting();
        }
    }
}
