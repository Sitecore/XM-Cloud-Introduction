using Microsoft.Extensions.Configuration;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Mvp.Foundation.DataFetching.GraphQL;
using Mvp.Project.MvpSite.Models;
using Sitecore.LayoutService.Client.Request;
using Sitecore.LayoutService.Client.RequestHandlers.GraphQL;
using System;
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

                string str = graphQlResponse.Data.Layout.Item.Rendered.ToString();
                if (str == null) return false;
            }
            catch(Exception ex)
            {
                _ = ex.Message;//log this?
                return false;
            }
            
            return true;
        }

        public async Task<List<Result>> GetSitemap()
        {
            try
            {
                var client = _graphQLClientFactory.CreateGraphQlClient();
                var query = Constants.GraphQlQueries.GetSitemapQuery;
                var variables = (object)new
                {
                    rootItemId = "{94DE9AC3-A9F7-40AB-AE90-ACDA364B9C40}",
                    language = "en"
                };

                var request = _graphQLRequestBuilder.BuildRequest(query, variables);
                var graphQlResponse = await client.SendQueryAsync<SitemapData>(request);

                return graphQlResponse.Data.Search.Results;
            }
            catch (Exception ex)
            {
                _ = ex.Message;//log this?
                return null;
            }

        }
    }
}



