namespace Mvp.Foundation.Configuration.Rendering.AppSettings
{
    public class MvpSiteSettings
    {
        public static readonly string Key = "Sitecore";

        public Uri? InstanceUri { get; set; }

        public string LayoutServicePath { get; set; } = "/sitecore/api/layout/render/jss";

        public string? DefaultSiteName { get; set; }

        public string? RootItemId { get; set; }

        public string? NotFoundPage { get; set; }

        public string? DefaultAcceptLanguageHeader { get; set; }

        public string? DefaultLanguage { get; set; }

        public Uri? RenderingHostUri { get; set; }

        public bool EnableExperienceEditor { get; set; }

        public string JssEditingSecret { get; set; } = string.Empty;

        public string? ExperienceEdgeToken { get; set; }

        public int MvpDirectoryGraphQLQueryPageSize { get; set; }

        public int MvpDirectoryGraphQLQueryCacheTimeout { get; set; }

        public int MvpApplicationListsDataGraphQLQueryCacheTimeout { get; set; }

        public Uri? LayoutServiceUri
        {
            get
            {
                if (InstanceUri == null) return null;

                return new Uri(InstanceUri, LayoutServicePath);
            }
        }
    }
}
