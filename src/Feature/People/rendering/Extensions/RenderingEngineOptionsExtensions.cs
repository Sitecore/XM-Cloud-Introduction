using Microsoft.Extensions.DependencyInjection;
using Mvp.Feature.People.Facets;
using Mvp.Feature.People.Models;
using Mvp.Feature.People.PeopleFinder;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Mvp.Feature.People.Exntesions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static IServiceCollection AddFeaturePeopleServices(this IServiceCollection services)
        {
            services.AddTransient<IPeopleFinder, MvpFinder>();
            services.AddTransient<IFacetBuilder, FacetBuilder>();
            return services;
        }

        public static RenderingEngineOptions AddFeaturePeople(this RenderingEngineOptions options)
        {
            options.AddModelBoundView<SearchParams>("GraphQLPeopleList");
            return options;
        }
    }
}