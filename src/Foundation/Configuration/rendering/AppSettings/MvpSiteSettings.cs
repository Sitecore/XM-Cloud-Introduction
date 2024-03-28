namespace Mvp.Foundation.Configuration.Rendering.AppSettings
{
    public class MvpSiteSettings
    {
        public static readonly string Key = "Sitecore";

        public Uri? InstanceUri { get; set; }

        public string LayoutServicePath { get; set; } = "/sitecore/api/layout/render/jss";

        public string? DefaultSiteName { get; set; }

        public string? NotFoundPage { get; set; }

        public string MvpProfilePageItemPath { get; set; } = "/Directory/Profile/*";

        public bool EnableExperienceEditor { get; set; }

        public string JssEditingSecret { get; set; } = string.Empty;

        public string? ExperienceEdgeToken { get; set; }

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
