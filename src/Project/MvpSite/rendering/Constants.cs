namespace Mvp.Project.MvpSite
{
    public class Constants
    {
        public static class GraphQlQueries
        {
            //the following query tested to return lookupfield displaynames
            public const string GetSitemapQuery = @"query SitemapQuery(
              $rootItemId: String
              $language: String
              $pageSize: Int = 100
              $hasLayout: String = ""true""
              $after: String
            ) {
              search(
                where: {
                  AND: [
                    { name: ""_path"", value: $rootItemId, operator: CONTAINS } 
                    { name: ""_language"", value: $language }
                    { name: ""_hasLayout"", value: $hasLayout }
                    { name: ""IncludeinSitemap"", value: ""true"" }
                  ]
                }
                first: $pageSize
                after: $after
              ) {
                total
                pageInfo {
                  endCursor
                  hasNext
                }
                results {
                  updateddatetime: field(name: ""__updated"") {
                    value
                  }
                  url {
                    path
                  }
                  name
                  ... on _Sitemap {
                    priority {
                      targetItem {
                        displayName
                      }
                    }
                    changeFrequency {
                      targetItem {
                        displayName
                      }
                    }
                  }
                }
              }
            }";
        }
    }
}
