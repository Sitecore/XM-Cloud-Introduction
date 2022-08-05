using GraphQL;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mvp.Feature.Forms.Models;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Mvp.Foundation.DataFetching.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp.Feature.Forms.ApplicationData
{
    public class ApplicationDataService : IApplicationDataService
    {
        private readonly IGraphQLRequestBuilder graphQLRequestBuilder;
        private readonly IGraphQLClientFactory graphQLClientFactory;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<ApplicationDataService> logger;
        private MvpSiteSettings Configuration { get; }

        public ApplicationDataService(IGraphQLRequestBuilder graphQLRequestBuilder, IConfiguration configuration, IGraphQLClientFactory graphQLClientFactory, IMemoryCache memoryCache, ILogger<ApplicationDataService> logger)
        {
            this.graphQLRequestBuilder = graphQLRequestBuilder;
            this.graphQLClientFactory = graphQLClientFactory;
            this.memoryCache = memoryCache;
            this.logger = logger;
            Configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        }

        public async Task<ApplicationLists> GetApplicationListDataAsync()
        {
            string key = $"MVP_APPLICATION_LIST_DATA_{Configuration.MvpDirectoryGraphQLQueryCacheTimeout}";

            if (!memoryCache.TryGetValue(key, out ApplicationLists applicationLists))
            {
                logger.LogInformation($"MVP Application List Data not in memory, building collection...");
                var variables = new
                {
                    pageSize = Constants.MVP_COUNTRY_PAGE_SIZE,
                    countryPath = Constants.MVP_COUNTRY_ROOT_ITEM_SHORT_ID,
                    countryTemplate = Constants.MVP_COUNTRY_TEMPLATE_SHORT_ID,
                    employmentStatusTemplate = Constants.MVP_EMPLOYMENT_STATUS_TEMPLATE_SHORT_ID,
                    mvpCategoryTemplate = Constants.MVP_CATEGORY_TEMPLATE_SHORT_ID
                };

                var request = graphQLRequestBuilder.BuildRequest(Constants.GraphQlQueries.GetApplicationListData, variables);
                var client = graphQLClientFactory.CreateGraphQlClient();

                logger.LogInformation($"Making GraphQL Request for MVP Application List Data");
                var response = await client.SendQueryAsync<ApplicationListDataSearchResponse>(request);

                applicationLists = new ApplicationLists
                {
                    Country = GetCountriesFromResponse(response),
                    EmploymentStatus = GetEmploymentStatusesFromResponse(response),
                    MvpCategory = GetActiveMvpStatusesFromResponse(response)
                };

                memoryCache.Set(key, applicationLists, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Configuration.MvpApplicationListsDataGraphQLQueryCacheTimeout)));
            }
            else
            {
                logger.LogInformation($"MVP Application List Data is in memory, data not requested from Edge");
            }

            return applicationLists;

        }

        private static IEnumerable<ApplicationListData> GetActiveMvpStatusesFromResponse(GraphQLResponse<ApplicationListDataSearchResponse> response)
        {
            return response.Data.Search.Results.Where(x => x.Template.Id == Constants.MVP_CATEGORY_TEMPLATE_SHORT_ID && x.Active.BoolValue).OrderBy(x => x.DisplayName).ToList();
        }

        private static IEnumerable<ApplicationListData> GetEmploymentStatusesFromResponse(GraphQLResponse<ApplicationListDataSearchResponse> response)
        {
            return response.Data.Search.Results.Where(x => x.Template.Id == Constants.MVP_EMPLOYMENT_STATUS_TEMPLATE_SHORT_ID).OrderBy(x => x.DisplayName).ToList();
        }

        private static IEnumerable<ApplicationListData> GetCountriesFromResponse(GraphQLResponse<ApplicationListDataSearchResponse> response)
        {
            return response.Data.Search.Results.Where(x => x.Template.Id == Constants.MVP_COUNTRY_TEMPLATE_SHORT_ID).OrderBy(x => x.DisplayName).ToList();
        }
    }
}
