using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Mvp.Feature.People.GraphQL;
using Mvp.Feature.People.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mvp.Feature.People.PeopleFinder
{
    public class MvpFinder : IPeopleFinder
    {
        private readonly IGraphQLRequestBuilder graphQLRequestBuilder;
        private readonly HttpClient httpClient;

        public MvpFinder(IGraphQLRequestBuilder graphQLRequestBuilder, HttpClient httpClient)
        {
            this.graphQLRequestBuilder = graphQLRequestBuilder;
            this.httpClient = httpClient;
        }

        public async Task<PeopleSearchResults> FindPeople(SearchParams searchParams)
        {
            var request = graphQLRequestBuilder.BuildRequest(string.Empty);

            var graphQLHttpClientOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://cm/sitecore/api/graph/edge"),
            };

            var client = new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);

            var response = await client.SendQueryAsync<MvpSearchResponse>(request);

            var mvps = new List<Person>();
            foreach(var mvpSearchResult in response.Data.Search.Results.Where(x => x.Awards != null & x.Awards.TargetItems.Any()))
            {
                mvps.Add(new Person
                {
                    FirstName = mvpSearchResult.FirstName.Value,
                    LastName = mvpSearchResult.LastName.Value,
                    Country = mvpSearchResult.Country.Name,
                    Email = mvpSearchResult.Email.Value,
                    MvpCategory = mvpSearchResult.Awards.TargetItems.FirstOrDefault().Field.TargetItem.Field.Value,
                    MvpYear = mvpSearchResult.Awards.TargetItems.FirstOrDefault().Parent.Name,
                    Url = mvpSearchResult.Path
                });
            }

            return new PeopleSearchResults
            {
                StartCursor = 1,
                EndCursor = 2,
                TotalCount = response.Data.Search.Total,
                People = mvps
            };
        }
    }
}
