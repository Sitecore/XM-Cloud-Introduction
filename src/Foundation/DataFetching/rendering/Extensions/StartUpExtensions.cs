using Microsoft.Extensions.DependencyInjection;
using Mvp.Foundation.DataFetching.GraphQL;

namespace Mvp.Feature.People.Exntesions
{
    public static class StartUpExtensions
    {
        public static IServiceCollection AddFoundationDataFetchingServices(this IServiceCollection services)
        {
            services.AddTransient<IGraphQLClientFactory, GraphQLClientFactory>();
            services.AddTransient<IGraphQLRequestBuilder, GraphQLRequestBuilder>();
            services.AddHttpClient<GraphQLClientFactory>();
            return services;
        }
    }
}