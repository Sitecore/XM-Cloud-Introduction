using Microsoft.AspNetCore.Builder;
using Mvp.Project.MvpSite.Extensions;
using Sitecore.AspNet.RenderingEngine.Extensions;
using Sitecore.AspNet.RenderingEngine.Middleware;

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
