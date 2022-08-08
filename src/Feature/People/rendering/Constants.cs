using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvp.Feature.People
{
    public class Constants
    {
        public const string MVP_PEOPLE_ROOT_ITEM_SHORT_ID = "64F31E3A20404E69B9A76830CBE669D2";
        public const string MVP_PERSON_TEMPLATE_SHORT_ID = "AD9C783786604360BA2B7ADDF4163685";

        public static class QueryParameters
        {
            public const string FacetPrefix = "fc_";
            public const string FacetAward = "fc_Type";
            public const string FacetYear = "fc_Year"; 
            public const string FacetCountry = "fc_Country";
            public const string Page = "pg";
            public const string Query = "q";
        }

        public static class GraphQlQueries
        {
            public const string GetMvps = @"
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
                                field(name: ""DisplayName"") {
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
        }
    }
}
