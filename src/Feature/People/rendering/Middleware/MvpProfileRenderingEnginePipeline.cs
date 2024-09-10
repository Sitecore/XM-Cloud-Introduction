using Microsoft.AspNetCore.Builder;
using Mvp.Feature.People.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Middleware;

namespace Mvp.Feature.People.Middleware
{
    public class MvpProfileRenderingEnginePipeline
        : RenderingEnginePipeline
    {
        public override void Configure(IApplicationBuilder app)
        {
            app.UseMvpProfileRouting();
            app.UseSitecoreRenderingEngine();
        }
    }
}
