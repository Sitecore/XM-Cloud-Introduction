using Sitecore.AspNetCore.SDK.RenderingEngine.Configuration;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;

namespace Mvp.Feature.User.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static RenderingEngineOptions AddFeatureUser(this RenderingEngineOptions options)
        {
            options.AddPartialView("~/Views/Shared/Components/SignIn.cshtml");
            return options;
        }
    }
}