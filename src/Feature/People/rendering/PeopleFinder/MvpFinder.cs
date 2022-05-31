using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mvp.Feature.People.GraphQL;
using Mvp.Feature.People.Models;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp.Feature.People.PeopleFinder
{
    public class MvpFinder : IPeopleFinder
    {
        private readonly IGraphQLRequestBuilder graphQLRequestBuilder;
        private readonly IGraphQLClientFactory graphQLClientFactory;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<MvpFinder> logger;
        private MvpSiteSettings Configuration { get; }

        public MvpFinder(IGraphQLRequestBuilder graphQLRequestBuilder, IGraphQLClientFactory graphQLClientFactory, IMemoryCache memoryCache, IConfiguration configuration, ILogger<MvpFinder> logger)
        {
            this.graphQLRequestBuilder = graphQLRequestBuilder;
            this.graphQLClientFactory = graphQLClientFactory;
            this.memoryCache = memoryCache;
            this.logger = logger;
            Configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        }

        public async Task<PeopleSearchResults> FindPeople(SearchParams searchParams)
        {
            var pageSize = 21;
            var currentPage = searchParams.CurrentPage > 1 ? searchParams.CurrentPage : 1;
            var mvps = await GetAllMvps();
            var filteredMvps = ApplyFiltering(mvps, searchParams);
            var resultPage = ApplyPaging(filteredMvps, currentPage, pageSize);

            var people = new List<Person>();
            foreach (var mvpSearchResult in resultPage)
            {
                people.Add(GeneratePersonRecord(mvpSearchResult));
            }

            return new PeopleSearchResults
            {
                CurrentPage = currentPage,
                PageSize = filteredMvps.Count < pageSize ? filteredMvps.Count : pageSize,
                TotalCount = filteredMvps.Count,
                People = people
            };
        }

        private IList<MvpSearchResult> ApplyFiltering(IList<MvpSearchResult> mvps, SearchParams searchParams)
        {
            var keyword = string.IsNullOrWhiteSpace(searchParams.Keyword) ? searchParams.Keyword : searchParams.Keyword.ToLowerInvariant();
            return mvps.Where(x => string.IsNullOrWhiteSpace(searchParams.Keyword) || x.FirstName.Value.ToLowerInvariant().Contains(keyword) || x.LastName.Value.ToLowerInvariant().Contains(keyword)).ToList();
        }

        private static Person GeneratePersonRecord(MvpSearchResult mvpSearchResult)
        {
            var latestAward = mvpSearchResult.Awards.TargetItems.OrderBy(x => x.Parent.Name).First();
            return new Person
            {
                FirstName = mvpSearchResult.FirstName.Value,
                LastName = mvpSearchResult.LastName.Value,
                Country = mvpSearchResult.Country.Name,
                Email = mvpSearchResult.Email.Value,
                MvpCategory = latestAward.Field.TargetItem.Field.Value,
                MvpYear = latestAward.Parent.Name,
                Url = mvpSearchResult.Path
            };
        }

        public IList<MvpSearchResult> ApplyPaging(IList<MvpSearchResult> mvps, int currentPage, int pageSize)
        {
            if(currentPage > 1)
            {
                var startIndex = (currentPage - 1) * pageSize;
                return mvps.Skip(startIndex).Take(pageSize).ToList();
            }
            else
            {
                return mvps.Take(pageSize).ToList();
            }
        }

        private async Task<IList<MvpSearchResult>> GetAllMvps()
        {
            string key = $"All_MVP_DATA_{Configuration.MvpDirectoryGraphQLQueryCacheTimeout}";

            if (!memoryCache.TryGetValue(key, out IList<MvpSearchResult> mvps))
            {
                var client = graphQLClientFactory.CreateGraphQlClient();
                var allPeople = await GetAllPeople(client, string.Empty);
                mvps = allPeople.Where(x => x.Awards != null & x.Awards.TargetItems.Any())
                                .OrderBy(x => x.FirstName.Value + x.LastName.Value)
                                .ToList();

                memoryCache.Set(key, mvps, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Configuration.MvpDirectoryGraphQLQueryCacheTimeout)));
            }

            return mvps;
        }

        private async Task<IList<MvpSearchResult>> GetAllPeople(IGraphQLClient client, string endCursor)
        {
            var mvps = new List<MvpSearchResult>();
            var request = graphQLRequestBuilder.BuildRequest(endCursor);

            logger.LogInformation($"Making GraphQL Request for MVPs, endCursor: '{endCursor}'");
            var response = await client.SendQueryAsync<MvpSearchResponse>(request);
            mvps.AddRange(response.Data.Search.Results);

            if(response.Data.Search.PageInfo.hasNextPage)
            {
                mvps.AddRange(await GetAllPeople(client, response.Data.Search.PageInfo.endCursor));
            }

            return mvps;
        }
    }
}