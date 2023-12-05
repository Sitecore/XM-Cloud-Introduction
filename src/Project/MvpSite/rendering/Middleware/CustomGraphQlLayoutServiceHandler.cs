using Microsoft.Extensions.Configuration;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Mvp.Foundation.DataFetching.GraphQL;
using Sitecore.LayoutService.Client.Request;
using Sitecore.LayoutService.Client.RequestHandlers.GraphQL;
using System;
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

        public async Task<bool> CheckLayoutExists(
          SitecoreLayoutRequest layoutrequest)
        {
            try
            {
                var client = _graphQLClientFactory.CreateGraphQlClient();
                var query = Constants.GraphQlQueries.GetItem;
                var variables = (object)new
                {
                    path = layoutrequest.Path(),
                    language = layoutrequest.Language(),
                    site = _configuration.DefaultSiteName
                };

                var request = _graphQLRequestBuilder.BuildRequest(query, variables);
                var graphQlResponse = await client.SendQueryAsync<LayoutQueryResponse>(request);

                string str = graphQlResponse?.Data?.Layout?.Item?.Rendered.ToString();
                if (str == null) return false;
            }
            catch(Exception ex)
            {
                _ = ex.Message;//log this?
                return false;
            }
            
            return true;
        }
    }
}



