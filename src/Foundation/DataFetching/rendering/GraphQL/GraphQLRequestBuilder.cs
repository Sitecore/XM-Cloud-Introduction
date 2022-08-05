using Microsoft.Extensions.Configuration;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Mvp.Foundation.DataFetching.GraphQL;
using System.Collections.Generic;

namespace Mvp.Foundation.DataFetching.GraphQL
{
    public class GraphQLRequestBuilder : IGraphQLRequestBuilder
    {
        private MvpSiteSettings Configuration { get; }

        public GraphQLRequestBuilder(IConfiguration configuration)
        {
            Configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        }

        public GraphQLHttpRequestWithHeaders BuildRequest(string query, object variables)
        {
            return new GraphQLHttpRequestWithHeaders
            {
                Query = query,
                Variables = variables,
                Headers = new Dictionary<string, string>
                {
                    { "sc_apikey", Configuration.ExperienceEdgeToken }
                }
            };
        }
    }
}
