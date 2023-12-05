namespace Mvp.Project.MvpSite
{
    public class Constants
    {
        public static class GraphQlQueries
        {
            public const string GetItem = "\r\n                query LayoutQuery($path: String!, $language: String!, $site: String!) {\r\n                    layout(routePath: $path, language: $language, site: $site) {\r\n                        item {\r\n                            rendered\r\n                        }\r\n                    }\r\n                }";
        }
    }
}
