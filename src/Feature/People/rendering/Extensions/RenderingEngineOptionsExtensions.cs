using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp.Feature.People.Configuration;
using Mvp.Feature.People.Facets;
using Mvp.Feature.People.Models;
using Mvp.Feature.People.PeopleFinder;
using Mvp.Feature.People.ViewComponents;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Mvp.Feature.People.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static IServiceCollection AddFeaturePeopleServices(this IServiceCollection services)
        {
            services.AddOptions<MvpPeopleOptions>().Configure<IConfiguration>((options, configuration) =>
                configuration.GetSection(MvpPeopleOptions.MvpPeople).Bind(options));

            services.AddTransient<IPeopleFinder, MvpFinder>();
            services.AddTransient<IFacetBuilder, FacetBuilder>();
            return services;
        }

        public static RenderingEngineOptions AddFeaturePeople(this RenderingEngineOptions options)
        {
            options.AddModelBoundView<SearchParams>("GraphQLPeopleList");
            options.AddViewComponent(DirectoryViewComponent.ViewComponentName);
            options.AddViewComponent(ProfileViewComponent.ViewComponentName);
            return options;
        }
    }
}