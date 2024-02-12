////using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
////using Microsoft.Extensions.Logging;
using Mvp.Feature.People.Facets;
using Mvp.Feature.People.Models;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
////using Mvp.Foundation.DataFetching.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
////using GraphQL;
using Mvp.Foundation.DataFetching.GraphQL.Models;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;

namespace Mvp.Feature.People.PeopleFinder
{
    public class MvpFinder(
        ////IGraphQLRequestBuilder graphQLRequestBuilder,
        ////IGraphQLClientFactory graphQLClientFactory,
        IMemoryCache memoryCache,
        IConfiguration configuration,
        IFacetBuilder facetBuilder,
        ////ILogger<MvpFinder> logger,
        MvpSelectionsApiClient apiClient)
        : IPeopleFinder
    {
        private MvpSiteSettings Configuration { get; } = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();

        public async Task<PeopleSearchResults> FindPeople(SearchParams searchParams)
        {
            int pageSize = 21;
            int currentPage = searchParams.CurrentPage > 1 ? searchParams.CurrentPage : 1;
            IList<MvpSearchResult> mvps = await GetAllMvps();
            IList<MvpSearchResult> filteredMvps = ApplyFilteringToMvpListing(mvps, searchParams);
            IDictionary<string, List<Facet>> facets = facetBuilder.CalculateFacets(filteredMvps, searchParams);
            IList<MvpSearchResult> filteredAndFacetedMvps = ApplyFacetingToMvpListing(filteredMvps, searchParams);
            IList<MvpSearchResult> resultPage = ApplyPaging(filteredAndFacetedMvps, currentPage, pageSize);

            List<Person> people = new ();
            foreach (MvpSearchResult mvpSearchResult in resultPage)
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

            string[] selectedAwards = string.IsNullOrWhiteSpace(searchParams.Award) ? Array.Empty<string>() : searchParams.Award.Split('|');
            string[] selectedYears = string.IsNullOrWhiteSpace(searchParams.Year) ? Array.Empty<string>() : searchParams.Year.Split('|');
            string[] selectedCountries = string.IsNullOrWhiteSpace(searchParams.Country) ? Array.Empty<string>() : searchParams.Country.Split('|');

            List<MvpSearchResult> facetedMvps = new ();
            foreach (MvpSearchResult mvp in mvps)
            {
                if (DoesMvpMatchSelectedFacets(selectedAwards, selectedYears, selectedCountries, mvp))
                    facetedMvps.Add(mvp);
            }
            return facetedMvps;           
        }

        private static bool DoesMvpMatchSelectedFacets(string[] selectedAwards, string[] selectedYears, string[] selectedCountries, MvpSearchResult mvp)
        {
            if(!MvpMatchesCountryFacet(selectedCountries, mvp))
            {
                return false;
            }

            foreach(Awards award in mvp.Awards.TargetItems)
            {
                if (AwardMatchesYearFacet(selectedYears, award) && AwardMatchesAwardTypeFacet(selectedAwards, award))
                    return true;
            }

            return false;
        }

        private static bool MvpMatchesCountryFacet(string[] selectedCountries, MvpSearchResult mvp)
        {
            return selectedCountries.Length == 0 
                || selectedCountries.Any(x => x.ToLowerInvariant() == mvp.Country.TargetItem?.Name.ToLowerInvariant());
        }

        private static bool AwardMatchesYearFacet(string[] selectedYears, Awards award)
        {
            return selectedYears.Length == 0
                || selectedYears.Any(y => y.ToLowerInvariant() == award.Parent.Name.ToLowerInvariant());
        }

        private static bool AwardMatchesAwardTypeFacet(string[] selectedAwards, Awards award)
        {
            return selectedAwards.Length == 0
                || selectedAwards.Any(y => y.ToLowerInvariant() == award.Field.TargetItem.Field.Value.ToLowerInvariant());
        }

        private IList<MvpSearchResult> ApplyFilteringToMvpListing(IList<MvpSearchResult> mvps, SearchParams searchParams)
        {
            return mvps.Where(
                mvp => string.IsNullOrWhiteSpace(searchParams.Keyword) ||
                DoesMvpFullnameMatchKeywords(searchParams.Keyword, mvp)).ToList();
        }

        private static bool DoesMvpFullnameMatchKeywords(string keyword, MvpSearchResult x)
        {
            return $"{x.FirstName.Value.ToLowerInvariant()}{x.LastName.Value.ToLowerInvariant()}".Contains(keyword.ToLowerInvariant().Replace(" ", string.Empty));
        }

        private static Person GeneratePersonRecord(MvpSearchResult mvpSearchResult)
        {
            Awards latestAward = mvpSearchResult.Awards.TargetItems.OrderByDescending(x => x.Parent.Name).First();
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
                int startIndex = (currentPage - 1) * pageSize;
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
                ////IGraphQLClient client = graphQLClientFactory.CreateGraphQlClient();
                IList<MvpSearchResult> allPeople = await GetAllPeopleFromApi();
                mvps = allPeople.Where(x => x.Awards?.TargetItems != null && x.Awards.TargetItems.Length != 0)
                                .OrderBy(x => x.FirstName.Value + x.LastName.Value)
                                .ToList();

                memoryCache.Set(key, mvps, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Configuration.MvpDirectoryGraphQLQueryCacheTimeout)));
            }

            return mvps;
        }

        // TODO [IVA] This is a temp solution to get the 2024 MVPs displayed
        private async Task<IList<MvpSearchResult>> GetAllPeopleFromApi()
        {
            List<MvpSearchResult> result = [];
            List<Title> allTitles = [];
            Response<IList<Title>> titlesResponse;
            int page = 1;
            do
            {
                titlesResponse = await apiClient.GetTitlesAsync(page: page, pageSize: 1000);
                if (titlesResponse.StatusCode == HttpStatusCode.OK && titlesResponse.Result?.Count > 0)
                {
                    allTitles.AddRange(titlesResponse.Result);
                    page++;
                }
            } while (titlesResponse.Result?.Count > 0);

            foreach (Title title in allTitles)
            {
                MvpSearchResult mvp =
                    result.SingleOrDefault(r => r.Name == title.Application.Applicant.Name);
                if (mvp != null)
                {
                    mvp.Awards.TargetItems =
                    [
                        ..mvp.Awards.TargetItems,
                        new Awards
                        {
                            Field = new TargetItemFieldValueItem
                            {
                                TargetItem = new FieldValueItem { Field = new ValueItem { Value = title.MvpType.Name } }
                            },
                            Parent = new NameItem { Name = title.Application.Selection.Year.ToString() }
                        }
                    ];
                    if (title.Application.Applicant.CreatedOn > DateTime.Parse(mvp.Path))
                    {
                        mvp.Email.Value = GetImageUri(title.Application.Applicant);
                        mvp.Country.TargetItem.Name = title.Application.Country.Name;
                    }
                }
                else
                {
                    mvp = new MvpSearchResult
                    {
                        Id = title.Application.Applicant.Id.ToString("N"),
                        FirstName = new ValueItem { Value = string.Empty },
                        LastName = new ValueItem { Value = title.Application.Applicant.Name },
                        Name = title.Application.Applicant.Name,
                        Country = new CountryData
                            { TargetItem = new TargetItem { Name = title.Application.Country.Name } },
                        Email = new ValueItem { Value = GetImageUri(title.Application.Applicant) },
                        Awards = new AwardData
                        {
                            TargetItems =
                            [
                                new Awards
                                {
                                    Field = new TargetItemFieldValueItem
                                        { TargetItem = new FieldValueItem { Field = new ValueItem { Value = title.MvpType.Name } } },
                                    Parent = new NameItem { Name = title.Application.Selection.Year.ToString() }
                                }
                            ]
                        },
                        Path = title.Application.Applicant.CreatedOn.ToString("O")
                    };
                    result.Add(mvp);
                }
            }

            return result;
        }

        private static string GetImageUri(User applicant)
        {
            string result;
            switch (applicant.ImageType)
            {
                case ImageType.Gravatar:
                    result = applicant.ImageUri?.ToString();
                    // ReSharper disable StringLiteralTypo - Url Encoding
                    result = !string.IsNullOrWhiteSpace(result) ? $"{result}?s=253&d=https%3A%2F%2Fmvp.sitecore.net%2Fimages%2Fmvp-base-user-grey.png" : string.Empty;
                    // ReSharper restore StringLiteralTypo - Url Encoding
                    break;
                default:
                    result = applicant.ImageUri?.ToString() ?? string.Empty;
                    break;
            }
            
            return  result;
        }

        ////private async Task<IList<MvpSearchResult>> GetAllPeople(IGraphQLClient client, string endCursor)
        ////{
        ////    List<MvpSearchResult> mvps = new ();
        ////    var variables = new
        ////    {
        ////        pageSize = Configuration.MvpDirectoryGraphQLQueryPageSize,
        ////        endCursor,
        ////        mvpPeopleRoot = Constants.MVP_PEOPLE_ROOT_ITEM_SHORT_ID,
        ////        mvpPersonTemplate = Constants.MVP_PERSON_TEMPLATE_SHORT_ID
        ////    };

        ////    GraphQLHttpRequestWithHeaders request = graphQLRequestBuilder.BuildRequest(Constants.GraphQlQueries.GetMvps, variables);
        ////    logger.LogInformation($"Making GraphQL Request for MVPs, endCursor: '{endCursor}'");

        ////    GraphQLResponse<MvpSearchResponse> response = await client.SendQueryAsync<MvpSearchResponse>(request);
        ////    mvps.AddRange(response.Data.Search.Results);

        ////    if(response.Data.Search.PageInfo.hasNextPage)
        ////    {
        ////        mvps.AddRange(await GetAllPeople(client, response.Data.Search.PageInfo.endCursor));
        ////    }

        ////    return mvps;
        ////}
    }
}