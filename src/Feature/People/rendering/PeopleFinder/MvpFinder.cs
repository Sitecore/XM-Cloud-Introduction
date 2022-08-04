using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mvp.Feature.People.Facets;
using Mvp.Feature.People.Models;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using Mvp.Foundation.DataFetching.GraphQL;
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
        private readonly IFacetBuilder facetBuilder;
        private readonly ILogger<MvpFinder> logger;
        private MvpSiteSettings Configuration { get; }

        public MvpFinder(IGraphQLRequestBuilder graphQLRequestBuilder, IGraphQLClientFactory graphQLClientFactory, IMemoryCache memoryCache, IConfiguration configuration, IFacetBuilder facetBuilder, ILogger<MvpFinder> logger)
        {
            this.graphQLRequestBuilder = graphQLRequestBuilder;
            this.graphQLClientFactory = graphQLClientFactory;
            this.memoryCache = memoryCache;
            this.facetBuilder = facetBuilder;
            this.logger = logger;
            Configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        }

        public async Task<PeopleSearchResults> FindPeople(SearchParams searchParams)
        {
            var pageSize = 21;
            var currentPage = searchParams.CurrentPage > 1 ? searchParams.CurrentPage : 1;
            var mvps = await GetAllMvps();
            var filteredMvps = ApplyFilteringToMvpListing(mvps, searchParams);
            var facets = facetBuilder.CalculateFacets(filteredMvps, searchParams);
            var filteredAndFacetedMvps = ApplyFacetingToMvpListing(filteredMvps, searchParams);
            var resultPage = ApplyPaging(filteredAndFacetedMvps, currentPage, pageSize);

            var people = new List<Person>();
            foreach (var mvpSearchResult in resultPage)
            {
                people.Add(GeneratePersonRecord(mvpSearchResult));
            }

            return new PeopleSearchResults
            {
                CurrentPage = currentPage,
                PageSize = filteredAndFacetedMvps.Count < pageSize ? filteredAndFacetedMvps.Count : pageSize,
                TotalCount = filteredAndFacetedMvps.Count,
                People = people,
                Facets = facets
            };
        }

        private IList<MvpSearchResult> ApplyFacetingToMvpListing(IList<MvpSearchResult> mvps, SearchParams searchParams)
        {
            if (string.IsNullOrWhiteSpace(searchParams.Award) && string.IsNullOrWhiteSpace(searchParams.Year) && string.IsNullOrWhiteSpace(searchParams.Country))
                return mvps;

            var selectedAwards = string.IsNullOrWhiteSpace(searchParams.Award) ? Array.Empty<string>() : searchParams.Award.Split('|');
            var selectedYears = string.IsNullOrWhiteSpace(searchParams.Year) ? Array.Empty<string>() : searchParams.Year.Split('|');
            var selectedCountries = string.IsNullOrWhiteSpace(searchParams.Country) ? Array.Empty<string>() : searchParams.Country.Split('|');

            var facetedMvps = new List<MvpSearchResult>();
            foreach (var mvp in mvps)
            {
                if (DoesMvpMatchSelectedFacets(selectedAwards, selectedYears, selectedCountries, mvp))
                    facetedMvps.Add(mvp);
            }
            return facetedMvps;           
        }

        private static bool DoesMvpMatchSelectedFacets(string[] selectedAwards, string[] selectedYears, string[] selectedCountries, MvpSearchResult mvp)
        {
            return MvpMatchesAwardTypeFacet(selectedAwards, mvp) 
                && MvpMatchesYearFacet(selectedYears, mvp) 
                && MvpMatchesCountryFacet(selectedCountries, mvp);
        }

        private static bool MvpMatchesCountryFacet(string[] selectedCountries, MvpSearchResult mvp)
        {
            return selectedCountries.Length == 0 
                || selectedCountries.Any(x => x.ToLowerInvariant() == mvp.Country.TargetItem?.Name.ToLowerInvariant());
        }

        private static bool MvpMatchesYearFacet(string[] selectedYears, MvpSearchResult mvp)
        {
            return selectedYears.Length == 0 
                || mvp.Awards.TargetItems.Any(x => selectedYears.Any(y => y.ToLowerInvariant() == x.Parent.Name.ToLowerInvariant()));
        }

        private static bool MvpMatchesAwardTypeFacet(string[] selectedAwards, MvpSearchResult mvp)
        {
            return selectedAwards.Length == 0 
                || mvp.Awards.TargetItems.Any(x => selectedAwards.Any(y => y.ToLowerInvariant() == x.Field.TargetItem.Field.Value.ToLowerInvariant()));
        }

        private IList<MvpSearchResult> ApplyFilteringToMvpListing(IList<MvpSearchResult> mvps, SearchParams searchParams)
        {
            var keyword = string.IsNullOrWhiteSpace(searchParams.Keyword) ? searchParams.Keyword : searchParams.Keyword.ToLowerInvariant();
            return mvps.Where(x => string.IsNullOrWhiteSpace(searchParams.Keyword) || x.FirstName.Value.ToLowerInvariant().Contains(keyword) || x.LastName.Value.ToLowerInvariant().Contains(keyword)).ToList();
        }

        private static Person GeneratePersonRecord(MvpSearchResult mvpSearchResult)
        {
            var latestAward = mvpSearchResult.Awards.TargetItems.OrderByDescending(x => x.Parent.Name).First();
            return new Person
            {
                FirstName = mvpSearchResult.FirstName.Value,
                LastName = mvpSearchResult.LastName.Value,
                Country = mvpSearchResult.Country?.TargetItem?.Name,
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
            var variables = new
            {
                pageSize = Configuration.MvpDirectoryGraphQLQueryPageSize,
                endCursor = endCursor,
                mvpPeopleRoot = Constants.MVP_PEOPLE_ROOT_ITEM_SHORT_ID,
                mvpPersonTemplate = Constants.MVP_PERSON_TEMPLATE_SHORT_ID
            };

            var request = graphQLRequestBuilder.BuildRequest(Constants.GraphQlQueries.GetMvps, variables);
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