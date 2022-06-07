using Microsoft.Extensions.Configuration;
using Mvp.Foundation.Configuration.Rendering.AppSettings;
using System.Collections.Generic;

namespace Mvp.Feature.People.GraphQL
{
    public class GraphQLRequestBuilder : IGraphQLRequestBuilder
    {
        private MvpSiteSettings Configuration { get; }

        public GraphQLRequestBuilder(IConfiguration configuration)
        {
            Configuration = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        }

        public GraphQLHttpRequestWithHeaders BuildRequest(string endCursor)
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
                    },
                    awards {
                      ... on MultilistField {
                        targetItems{
                          name,
                          parent {
                            name
                          }
                          field(name:""type"") {
                            ... on LookupField{
                              targetItem {
                                field(name: ""Name"") {
                                  value
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }

                query(
                  $pageSize: Int!
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
                    ] 
                    }   
                    first: $pageSize
                    after: $endCursor
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
                pageSize = Configuration.MvpDirectoryGraphQLQueryPageSize,
                endCursor = endCursor,
                mvpPeopleRoot = Constants.MVP_PEOPLE_ROOT_ITEM_SHORT_ID,
                mvpPersonTemplate = Constants.MVP_PERSON_TEMPLATE_SHORT_ID
            };

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
