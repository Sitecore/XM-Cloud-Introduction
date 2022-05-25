using Mvp.Feature.Social.Models;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Mvp.Feature.Social.FeedReader;

namespace Mvp.Feature.Social.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static void AddFeatureSocialServices(this IServiceCollection services)
        {
            services.AddTransient<IFeedReader, RssFeedReader>();
        }

        public static RenderingEngineOptions AddFeatureSocial(this RenderingEngineOptions options)
        {
            options.AddModelBoundView<Rss>("Rss");
            return options;
        }
    }
}