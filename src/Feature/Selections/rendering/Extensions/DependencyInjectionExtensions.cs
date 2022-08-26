using Microsoft.Extensions.DependencyInjection;
using Mvp.Feature.Selections.ViewComponents.Admin;
using Mvp.Selections.Client.Extensions;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Mvp.Feature.Selections.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddFeatureSelectionsServices(this IServiceCollection services)
        {
            services.AddMvpSelectionsApiClient();
            return services;
        }

        public static RenderingEngineOptions AddFeatureSelections(this RenderingEngineOptions options)
        {
            options.AddViewComponent(UsersOverviewViewComponent.ViewComponentName);
            options.AddViewComponent(SystemRolesOverviewViewComponent.ViewComponentName);
            options.AddViewComponent(RegionsOverviewViewComponent.ViewComponentName);
            options.AddViewComponent(CountriesOverviewViewComponent.ViewComponentName);
            return options;
        }
    }
}
