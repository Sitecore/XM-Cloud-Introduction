using Mvp.Feature.Navigation.Models;
using Sitecore.AspNetCore.SDK.RenderingEngine.Configuration;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;

namespace Mvp.Feature.Navigation.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static RenderingEngineOptions AddFeatureNavigation(this RenderingEngineOptions options)
        {
            options.AddModelBoundView<TopLinks>("TopLinks")
                   .AddModelBoundView<MainNav>("MainNav")
                   .AddModelBoundView<Footer>("Footer");
            return options;
        }
    }
}