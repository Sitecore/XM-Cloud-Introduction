namespace Mvp.Project.MvpSite
{
    public class Constants
    {
        public static class GraphQlQueries
        {
            public const string GetItem = "\r\n                query LayoutQuery($path: String!, $language: String!, $site: String!) {\r\n                    layout(routePath: $path, language: $language, site: $site) {\r\n                        item {\r\n                            rendered\r\n                        }\r\n                    }\r\n                }";

            //the following query tested to return lookupfield displaynames
            public const string GetSitemapQuery = "query SitemapQuery(\r\n  $rootItemId: String = \"{A43C6692-60F4-4743-A1BE-6424DE611DFA}\"\r\n  $language: String = \"en\"\r\n  $pageSize: Int = 100\r\n  $hasLayout: String = \"true\"\r\n  $after: String\r\n) {\r\n  search(\r\n    where: {\r\n      AND: [\r\n        { name: \"_path\", value: $rootItemId, operator: CONTAINS }\r\n        { name: \"_language\", value: $language }\r\n        { name: \"_hasLayout\", value: $hasLayout }\r\n        { name: \"IncludeinSitemap\", value: \"true\" }\r\n      ]\r\n    }\r\n    first: $pageSize\r\n    after: $after\r\n  ) {\r\n    total\r\n    pageInfo {\r\n      endCursor\r\n      hasNext\r\n    }\r\n    results {\r\n      updateddatetime: field(name: \"__updated\") {\r\n        value\r\n      }\r\n      url {\r\n        path\r\n      }\r\n      name\r\n      ... on _Sitemap {\r\n        priority {\r\n          targetItem {\r\n            displayName\r\n          }\r\n        }\r\n        changeFrequency {\r\n          targetItem {\r\n            displayName\r\n          }\r\n        }\r\n      }\r\n    }\r\n  }\r\n}\r\n";
        }
    }
}
