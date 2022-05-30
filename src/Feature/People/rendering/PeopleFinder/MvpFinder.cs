using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Mvp.Feature.People.GraphQL;
using Mvp.Feature.People.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mvp.Feature.People.PeopleFinder
{
    public class MvpFinder : IPeopleFinder
    {
        private readonly HttpClient httpClient;

        public MvpFinder(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<PeopleSearchResults> FindPeople(SearchParams searchParams)
        {
            var query = @"
                fragment personItemFields on Item {
                  id,
                  name,
                  path,
                  ... on Person {
                    firstName {
                      value
                    },
                    lastName {
                      value
                    }, 
                    email {
                      value
                    },
                    country{
                      targetItem
                      {
                        name
                      }
                    }
                  }
                }

                query(
                  $endCursor: String!
                  $mvpPeopleRoot: String!
                  $mvpPersonTemplate: String!
                ) 
                {
                  search(
                    where: {
                     AND: [
                      {
                        name: ""_path"", 
                        value: $mvpPeopleRoot
                        operator: CONTAINS
                      },
                      {
                        name: ""_templates"", 
                        value: $mvpPersonTemplate
                        operator: CONTAINS
                      }
                      ,
                      {
                        name: ""awards"", 
                        value:""""
                        operator: NEQ
                      }
                    ] 
                    }   
                    first: 21
                    after: $endCursor
                    orderBy: { name: ""firstName"", direction: DESC }
	                ) 
                  {
                    total,
                    pageInfo {
                      endCursor,
                      hasNext
                    },
                    results {
                      ... personItemFields
                    } 
                  }
                }
            ";

            var variables = new
            {
                endCursor = "",
                mvpPeopleRoot = "64F31E3A20404E69B9A76830CBE669D2",
                mvpPersonTemplate = "AD9C783786604360BA2B7ADDF4163685"
            };

            var request = new GraphQLHttpRequestWithHeaders
            {
                Query = query,
                Variables = variables,
                Headers = new Dictionary<string, string>
                {
                    { "sc_apikey", "{E2F3D43E-B1FD-495E-B4B1-84579892422A}" }
                }
            };
            var graphQLHttpClientOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://cm/sitecore/api/graph/edge"),
            };

            var client = new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);

            var response = await client.SendQueryAsync<MvpSearchResponse>(request);

            var mvps = new List<Person>();
            foreach(var mvpSearchResult in response.Data.Search.Results)
            {
                mvps.Add(new Person
                {
                    FirstName = mvpSearchResult.FirstName.Value,
                    LastName = mvpSearchResult.LastName.Value,
                    Country = mvpSearchResult.Country.Name,
                    Email = mvpSearchResult.Email.Value,
                    MvpCategory = "TBC",
                    MvpYear = "TBC",
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
