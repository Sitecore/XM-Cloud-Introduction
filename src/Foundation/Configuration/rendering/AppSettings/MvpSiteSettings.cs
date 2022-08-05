namespace Mvp.Foundation.Configuration.Rendering.AppSettings
{
    public class MvpSiteSettings
    {
        public static readonly string Key = "Sitecore";

        public Uri? InstanceUri { get; set; }
        public string LayoutServicePath { get; set; } = "/sitecore/api/layout/render/jss";
        public string? DefaultSiteName { get; set; }
        public Uri? RenderingHostUri { get; set; }
        public bool EnableExperienceEditor { get; set; }
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
