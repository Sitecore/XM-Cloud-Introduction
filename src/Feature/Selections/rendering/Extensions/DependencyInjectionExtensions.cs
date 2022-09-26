using Microsoft.Extensions.DependencyInjection;
using Mvp.Feature.Selections.Providers;
using Mvp.Feature.Selections.ViewComponents;
using Mvp.Feature.Selections.ViewComponents.Admin;
using Mvp.Feature.Selections.ViewComponents.Apply;
using Mvp.Selections.Client.Extensions;
using Mvp.Selections.Client.Interfaces;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Mvp.Feature.Selections.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddFeatureSelectionsServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenProvider, HttpContextTokenProvider>();
            services.AddMvpSelectionsApiClient();
            return services;
        }

        public static RenderingEngineOptions AddFeatureSelections(this RenderingEngineOptions options)
        {
            options.AddViewComponent(UsersOverviewViewComponent.ViewComponentName);
            options.AddViewComponent(UserEditViewComponent.ViewComponentName);
            options.AddViewComponent(SystemRolesOverviewViewComponent.ViewComponentName);
            options.AddViewComponent(RegionsOverviewViewComponent.ViewComponentName);
            options.AddViewComponent(CountriesOverviewViewComponent.ViewComponentName);
            options.AddViewComponent(ApplicationOverviewViewComponent.ViewComponentName);
            options.AddViewComponent(MvpTypesOverviewViewComponent.ViewComponentName);
            options.AddViewComponent(DashboardViewComponent.ViewComponentName);
            options.AddViewComponent(ApplicationFormViewComponent.ViewComponentName);
            return options;
        }
    }
}
