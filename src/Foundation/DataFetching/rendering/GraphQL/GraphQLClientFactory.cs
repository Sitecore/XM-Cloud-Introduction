using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace Mvp.Foundation.DataFetching.GraphQL
{
    public class GraphQLClientFactory : IGraphQLClientFactory
    {
        private readonly HttpClient _httpClient;
        
        private readonly MvpSiteSettings _configuration;

        public GraphQLClientFactory(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        }

        public IGraphQLClient CreateGraphQlClient()
        {
            GraphQLHttpClientOptions graphQLHttpClientOptions = new ()
            {
                EndPoint = _configuration.LayoutServiceUri
            };

            return new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), _httpClient);
        }
    }
}
