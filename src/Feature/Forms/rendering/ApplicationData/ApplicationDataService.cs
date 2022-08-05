using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mvp.Feature.Forms.Models;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Mvp.Foundation.DataFetching.GraphQL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp.Feature.Forms.ApplicationData
{
    public class ApplicationDataService : IApplicationDataService
    {
        private readonly IGraphQLRequestBuilder graphQLRequestBuilder;
        private readonly IGraphQLClientFactory graphQLClientFactory;
        private readonly ILogger<ApplicationDataService> logger;
        private MvpSiteSettings Configuration { get; }

        public ApplicationDataService(IGraphQLRequestBuilder graphQLRequestBuilder, IConfiguration configuration, IGraphQLClientFactory graphQLClientFactory, ILogger<ApplicationDataService> logger)
        {
            this.graphQLRequestBuilder = graphQLRequestBuilder;
            this.graphQLClientFactory = graphQLClientFactory;
            this.logger = logger;
            Configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        }

        public async Task<ApplicationLists> GetApplicationListDataAsync()
        {
            //string key = $"MVP_COUNTRY_DATA_{Configuration.MvpDirectoryGraphQLQueryCacheTimeout}";

            //if (!memoryCache.TryGetValue(key, out IList<MvpSearchResult> mvps))
            //{
                
                //var allPeople = await GetAllPeople(client, string.Empty);
                //mvps = allPeople.Where(x => x.Awards != null & x.Awards.TargetItems.Any())
                //                .OrderBy(x => x.FirstName.Value + x.LastName.Value)
                //                .ToList();

            //    memoryCache.Set(key, mvps, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Configuration.MvpDirectoryGraphQLQueryCacheTimeout)));
            //}

            //return mvps;



            var applicationLists = new ApplicationLists();
            var variables = new
            {
                pageSize = Constants.MVP_COUNTRY_PAGE_SIZE,
                countryPath = Constants.MVP_COUNTRY_ROOT_ITEM_SHORT_ID,
                countryTemplate = Constants.MVP_COUNTRY_TEMPLATE_SHORT_ID                    
            };

            var request = graphQLRequestBuilder.BuildRequest(Constants.GraphQlQueries.GetApplicationCountries, variables);
            logger.LogInformation($"Making GraphQL Request for MVPs Application Country Data");

            var client = graphQLClientFactory.CreateGraphQlClient();
            var response = await client.SendQueryAsync<CountrySearchResponse>(request);
            var countries = response.Data.Search.Results;
           

            return new ApplicationLists
            {
                Country = countries.OrderBy(x => x.DisplayName),
                EmploymentStatus = new List<EmploymentStatus>() { new EmploymentStatus { Name = "Employed", Description = "Employed", ID = System.Guid.Empty } },
                MvpCategory = new List<MvpCategory>() { new MvpCategory { Name = "Technology", Active = true } }
            };
        }
    }
}
