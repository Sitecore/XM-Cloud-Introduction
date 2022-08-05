using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using System.Net.Http;

namespace Mvp.Foundation.DataFetching.GraphQL
{
    public class GraphQLClientFactory : IGraphQLClientFactory
    {
        private readonly HttpClient httpClient;
        private MvpSiteSettings configuration;

        public GraphQLClientFactory(IConfiguration configuration, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        }

        public IGraphQLClient CreateGraphQlClient()
        {
            var graphQLHttpClientOptions = new GraphQLHttpClientOptions
            {
                EndPoint = this.configuration.LayoutServiceUri
            };

            return new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);
        }
    }
}
