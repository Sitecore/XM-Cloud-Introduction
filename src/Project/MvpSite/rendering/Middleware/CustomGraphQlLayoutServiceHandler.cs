using Microsoft.Extensions.Configuration;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Mvp.Foundation.DataFetching.GraphQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mvp.Project.MvpSite.Middleware
{
    public class CustomGraphQlLayoutServiceHandler 
    {
        private readonly IGraphQLRequestBuilder _graphQLRequestBuilder;
        private readonly IGraphQLClientFactory _graphQLClientFactory;
        private readonly MvpSiteSettings _configuration;

        public CustomGraphQlLayoutServiceHandler(IConfiguration configuration, IGraphQLRequestBuilder graphQLRequestBuilder, IGraphQLClientFactory graphQLClientFactory)
        {
            _graphQLRequestBuilder = graphQLRequestBuilder;
            _graphQLClientFactory = graphQLClientFactory;
            _configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        }

        public async Task<List<Result>> GetSitemap()
        {
            var client = _graphQLClientFactory.CreateGraphQlClient();
            var query = Constants.GraphQlQueries.GetSitemapQuery;
            var variables = (object)new
            {
                rootItemId = _configuration.RootItemId,
                language = _configuration.DefaultLanguage
            };

            var request = _graphQLRequestBuilder.BuildRequest(query, variables);
            var graphQlResponse = await client.SendQueryAsync<SitemapData>(request);

            return graphQlResponse.Data.Search.Results;
        }
    }
}



