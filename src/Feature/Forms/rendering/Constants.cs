namespace Mvp.Feature.Forms
{
    public class Constants
    {
        public const string MVP_COUNTRY_ROOT_ITEM_SHORT_ID = "D18D6FFEDEF44D7BAEA013F06B9030A3";
        public const string MVP_COUNTRY_TEMPLATE_SHORT_ID = "1FC8657FF73747D6A52B67E8BFF8A390";
        public const int MVP_COUNTRY_PAGE_SIZE = 200;

        public static class GraphQlQueries
        {
            public const string GetApplicationCountries = @"
                      query (
                        $pageSize: Int!
                        $countryPath: String!
                        $countryTemplate: String!
                      ) 
                      {
                        search(
                          where: {
                            AND: [
                              {
                                name: ""_path"", 
                                value: $countryPath
                                operator: CONTAINS
                              },
                              {
                                name: ""_templates"", 
                                value: $countryTemplate
                                operator: CONTAINS
                              }
                            ]
                          }
                          first: $pageSize
                        ) 
                        {
                        total,
                          pageInfo {
                            endCursor,
                            hasNext
                          },
                          results {
                            name,
                            id,
                            ... on Country {
                                displayName,
                              description {
                                    value
                              }
                            }
                        }
                    }
                  }
            ";
        }
    }
}
