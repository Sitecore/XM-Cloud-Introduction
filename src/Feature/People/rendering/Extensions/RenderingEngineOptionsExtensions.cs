using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp.Feature.People.Configuration;
using Mvp.Feature.People.ViewComponents;
using Sitecore.AspNetCore.SDK.RenderingEngine.Configuration;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;

namespace Mvp.Feature.People.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static IServiceCollection AddFeaturePeopleServices(this IServiceCollection services)
        {
            services.AddOptions<MvpPeopleOptions>().Configure<IConfiguration>((options, configuration) =>
                configuration.GetSection(MvpPeopleOptions.MvpPeople).Bind(options));

            return services;
        }

        public static RenderingEngineOptions AddFeaturePeople(this RenderingEngineOptions options)
        {
            options.AddViewComponent(DirectoryViewComponent.ViewComponentName);
            options.AddViewComponent(ProfileViewComponent.ViewComponentName);
            return options;
        }
    }
}