using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp.Feature.Selections.Configuration;
using Mvp.Feature.Selections.Providers;
using Mvp.Feature.Selections.ViewComponents.Admin;
using Mvp.Feature.Selections.ViewComponents.Any;
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
            services.AddOptions<MvpSelectionsOptions>().Configure<IConfiguration>((options, configuration) =>
                configuration.GetSection(MvpSelectionsOptions.MvpSelections).Bind(options));
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
            options.AddViewComponent(MyDataEditViewComponent.ViewComponentName);
            options.AddViewComponent(MyProfilesFormViewComponent.ViewComponentName);
            options.AddViewComponent(ScoreCardsViewComponent.ViewComponentName);
            options.AddViewComponent(ScoreCardDetailViewComponent.ViewComponentName);
            options.AddViewComponent(AwardViewComponent.ViewComponentName);
            return options;
        }
    }
}
