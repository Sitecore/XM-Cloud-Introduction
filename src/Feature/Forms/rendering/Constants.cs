namespace Mvp.Feature.Forms
{
    public class Constants
    {
        public const string MVP_COUNTRY_ROOT_ITEM_SHORT_ID = "D3B2AC74F73D450DA040B40D53E314EC";
        public const string MVP_COUNTRY_TEMPLATE_SHORT_ID = "1FC8657FF73747D6A52B67E8BFF8A390";
        public const string MVP_EMPLOYMENT_STATUS_TEMPLATE_SHORT_ID = "37421902C3C549E3BF4AA7EE73312993";
        public const string MVP_CATEGORY_TEMPLATE_SHORT_ID = "CDD8427EFA864549A647D665164F2F77";
        public const int MVP_COUNTRY_PAGE_SIZE = 200;

        public static class GraphQlQueries
        {
            public const string GetApplicationListData = @"
                        query (
                            $pageSize: Int!
                            $countryPath: String!
                            $countryTemplate: String!
                            $employmentStatusTemplate: String!
                            $mvpCategoryTemplate: String!
                          ) 
                          {
                            search(
                              where: {
                                AND: [
                                  {
                                    name: ""_path"", 
                                    value: $countryPath
                                    operator: CONTAINS
                                  }
                                ],
                                OR: [
                                  {
                                    name: ""_templates"", 
                                    value: $countryTemplate
                                    operator: CONTAINS
                                  },  
                                  {
                                    name: ""_templates"", 
                                    value: $employmentStatusTemplate
                                    operator: CONTAINS
                                  }, 
                                  {
                                    name: ""_templates"", 
                                    value: $mvpCategoryTemplate
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
                                template {
                                  id,
                                  name
                                }
                                id,
                                ... on Country {
                                  displayName,
                                  description {
                                    value
                                  }
                                },
                                ... on EmploymentStatus {
                                  displayName
                                }
                                ... on MVPCategory {
                                  displayName,
                                  active {
                                    boolValue
                                  }
                                }
                              }
                            }
                          }
            ";
        }
    }
}
