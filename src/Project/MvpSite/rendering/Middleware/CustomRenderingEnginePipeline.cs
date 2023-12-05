using Microsoft.AspNetCore.Builder;
using Mvp.Project.MvpSite.Extensions;
using Sitecore.AspNet.RenderingEngine.Middleware;

namespace Mvp.Project.MvpSite.Middleware
{
    public class CustomRenderingEnginePipeline:RenderingEnginePipeline
    {
            //
            // Summary:
            //     Adds the Sitecore Rendering Engine features to the given Microsoft.AspNetCore.Builder.IApplicationBuilder.
            //
            //
            // Parameters:
            //   app:
            //     The Microsoft.AspNetCore.Builder.IApplicationBuilder to add features to.
            public override void Configure(IApplicationBuilder app)
            {
                app.UseCustomSitecoreRenderingEngine();
            }
    }
}
